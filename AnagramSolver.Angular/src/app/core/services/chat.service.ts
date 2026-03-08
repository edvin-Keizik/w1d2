import { Injectable, inject, signal } from '@angular/core';
import { ApiService } from './api.service';
import { UxState } from '../models/ux-state.model';
import {
  ChatMessage,
  ChatRequest,
  ChatResponse,
  ActiveSessions,
  ChatHistory,
} from '../models/chat.model';

const SESSION_KEY = 'chat_session_id';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private readonly _api = inject(ApiService);

  readonly messages = signal<ChatMessage[]>([]);
  readonly sendState = signal<UxState<void>>({ stage: 'idle' });
  readonly sessionId = signal<string | null>(localStorage.getItem(SESSION_KEY));
  readonly sessionsState = signal<UxState<ActiveSessions>>({ stage: 'idle' });
  readonly historyState = signal<UxState<ChatHistory>>({ stage: 'idle' });

  send(message: string): void {
    const userMsg: ChatMessage = {
      role: 'user',
      content: message,
      timestamp: Date.now() / 1000,
    };
    this.messages.update((msgs) => [...msgs, userMsg]);
    this.sendState.set({ stage: 'loading' });

    const request: ChatRequest = { message, sessionId: this.sessionId() ?? undefined };
    this._api.post<ChatResponse, ChatRequest>('ai/chat', request).subscribe({
      next: (res) => {
        if (res.sessionId) {
          this.sessionId.set(res.sessionId);
          localStorage.setItem(SESSION_KEY, res.sessionId);
        }
        const assistantMsg: ChatMessage = {
          role: 'assistant',
          content: res.response,
          timestamp: Date.now() / 1000,
        };
        this.messages.update((msgs) => [...msgs, assistantMsg]);
        this.sendState.set({ stage: 'idle' });
      },
      error: (err) => {
        this.sendState.set({ stage: 'error', error: err.message ?? 'Chat failed' });
      },
    });
  }

  loadHistory(sessionId: string): void {
    this.historyState.set({ stage: 'loading' });
    this._api
      .get<ChatHistory>(`ai/chat/${encodeURIComponent(sessionId)}/history`)
      .subscribe({
        next: (data) => this.historyState.set({ stage: 'loaded', data }),
        error: (err) =>
          this.historyState.set({ stage: 'error', error: err.message ?? 'Failed to load history' }),
      });
  }

  loadActiveSessions(): void {
    this.sessionsState.set({ stage: 'loading' });
    this._api.get<ActiveSessions>('ai/chat/sessions/active').subscribe({
      next: (data) => this.sessionsState.set({ stage: 'loaded', data }),
      error: (err) =>
        this.sessionsState.set({
          stage: 'error',
          error: err.message ?? 'Failed to load sessions',
        }),
    });
  }

  deleteSession(sessionId: string): void {
    this._api
      .delete<{ success: boolean }>(`ai/chat/${encodeURIComponent(sessionId)}/history`)
      .subscribe({
        next: () => this.loadActiveSessions(),
        error: () => {},
      });
  }

  startNewSession(): void {
    this.sessionId.set(null);
    localStorage.removeItem(SESSION_KEY);
    this.messages.set([]);
    this.sendState.set({ stage: 'idle' });
  }

  hasSession(): boolean {
    return this.sessionId() !== null;
  }
}
