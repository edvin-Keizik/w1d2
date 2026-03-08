import { Component, inject, OnInit, signal } from '@angular/core';
import { ChatService } from '../../../core/services/chat.service';
import { ChatHistory } from '../../../core/models/chat.model';
import { SessionListComponent } from '../components/session-list.component';
import { SessionDetailModalComponent } from '../components/session-detail-modal.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorDisplayComponent } from '../../../shared/components/error-display/error-display.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-chat-history-page',
  standalone: true,
  imports: [
    SessionListComponent,
    SessionDetailModalComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent,
  ],
  templateUrl: './chat-history-page.component.html',
})
export class ChatHistoryPageComponent implements OnInit {
  protected readonly chatSvc = inject(ChatService);

  readonly selectedHistory = signal<ChatHistory | null>(null);

  readonly sessions = () => {
    const s = this.chatSvc.sessionsState();
    return s.stage === 'loaded' ? s.data.sessions : [];
  };

  readonly sessionsError = () => {
    const s = this.chatSvc.sessionsState();
    return s.stage === 'error' ? s.error : '';
  };

  ngOnInit(): void {
    this.chatSvc.loadActiveSessions();
  }

  onSelect(sessionId: string): void {
    this.chatSvc.loadHistory(sessionId);
    // Watch for history to load
    const check = setInterval(() => {
      const hs = this.chatSvc.historyState();
      if (hs.stage === 'loaded') {
        this.selectedHistory.set(hs.data);
        clearInterval(check);
      } else if (hs.stage === 'error') {
        clearInterval(check);
      }
    }, 100);
    setTimeout(() => clearInterval(check), 10000);
  }

  onDelete(sessionId: string): void {
    if (confirm(`Delete session ${sessionId}?`)) {
      this.chatSvc.deleteSession(sessionId);
    }
  }
}
