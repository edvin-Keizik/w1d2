import {
  ApiService,
  Injectable,
  inject,
  setClassMetadata,
  signal,
  ɵɵdefineInjectable
} from "./chunk-5B3AD7EP.js";

// src/app/core/services/chat.service.ts
var SESSION_KEY = "chat_session_id";
var ChatService = class _ChatService {
  _api = inject(ApiService);
  messages = signal([]);
  sendState = signal({ stage: "idle" });
  sessionId = signal(localStorage.getItem(SESSION_KEY));
  sessionsState = signal({ stage: "idle" });
  historyState = signal({ stage: "idle" });
  send(message) {
    const userMsg = {
      role: "user",
      content: message,
      timestamp: Date.now() / 1e3
    };
    this.messages.update((msgs) => [...msgs, userMsg]);
    this.sendState.set({ stage: "loading" });
    const request = { message, sessionId: this.sessionId() ?? void 0 };
    this._api.post("ai/chat", request).subscribe({
      next: (res) => {
        if (res.sessionId) {
          this.sessionId.set(res.sessionId);
          localStorage.setItem(SESSION_KEY, res.sessionId);
        }
        const assistantMsg = {
          role: "assistant",
          content: res.response,
          timestamp: Date.now() / 1e3
        };
        this.messages.update((msgs) => [...msgs, assistantMsg]);
        this.sendState.set({ stage: "idle" });
      },
      error: (err) => {
        this.sendState.set({ stage: "error", error: err.message ?? "Chat failed" });
      }
    });
  }
  loadHistory(sessionId) {
    this.historyState.set({ stage: "loading" });
    this._api.get(`ai/chat/${encodeURIComponent(sessionId)}/history`).subscribe({
      next: (data) => this.historyState.set({ stage: "loaded", data }),
      error: (err) => this.historyState.set({ stage: "error", error: err.message ?? "Failed to load history" })
    });
  }
  loadActiveSessions() {
    this.sessionsState.set({ stage: "loading" });
    this._api.get("ai/chat/sessions/active").subscribe({
      next: (data) => this.sessionsState.set({ stage: "loaded", data }),
      error: (err) => this.sessionsState.set({
        stage: "error",
        error: err.message ?? "Failed to load sessions"
      })
    });
  }
  deleteSession(sessionId) {
    this._api.delete(`ai/chat/${encodeURIComponent(sessionId)}/history`).subscribe({
      next: () => this.loadActiveSessions(),
      error: () => {
      }
    });
  }
  startNewSession() {
    this.sessionId.set(null);
    localStorage.removeItem(SESSION_KEY);
    this.messages.set([]);
    this.sendState.set({ stage: "idle" });
  }
  hasSession() {
    return this.sessionId() !== null;
  }
  static \u0275fac = function ChatService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ChatService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _ChatService, factory: _ChatService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ChatService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

export {
  ChatService
};
//# sourceMappingURL=chunk-LU74ZHSU.js.map
