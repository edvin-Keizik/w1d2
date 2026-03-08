import {
  EmptyStateComponent
} from "./chunk-PN7V6CON.js";
import {
  ErrorDisplayComponent,
  LoadingSpinnerComponent
} from "./chunk-OTOFTWZT.js";
import {
  ChatService
} from "./chunk-LU74ZHSU.js";
import {
  TimeAgoPipe
} from "./chunk-K5NYSZAZ.js";
import {
  Component,
  inject,
  input,
  output,
  setClassMetadata,
  signal,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵclassProp,
  ɵɵconditional,
  ɵɵdefineComponent,
  ɵɵelement,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵgetCurrentView,
  ɵɵlistener,
  ɵɵnextContext,
  ɵɵpipe,
  ɵɵpipeBind1,
  ɵɵproperty,
  ɵɵrepeater,
  ɵɵrepeaterCreate,
  ɵɵrepeaterTrackByIdentity,
  ɵɵrepeaterTrackByIndex,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate,
  ɵɵtextInterpolate1
} from "./chunk-5B3AD7EP.js";

// src/app/features/chat-history/components/session-list.component.ts
function SessionListComponent_For_2_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "div", 1)(1, "span", 2);
    \u0275\u0275listener("click", function SessionListComponent_For_2_Template_span_click_1_listener() {
      const session_r2 = \u0275\u0275restoreView(_r1).$implicit;
      const ctx_r2 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r2.select.emit(session_r2));
    });
    \u0275\u0275text(2);
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(3, "div", 3)(4, "button", 4);
    \u0275\u0275listener("click", function SessionListComponent_For_2_Template_button_click_4_listener() {
      const session_r2 = \u0275\u0275restoreView(_r1).$implicit;
      const ctx_r2 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r2.select.emit(session_r2));
    });
    \u0275\u0275text(5, "View");
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(6, "button", 5);
    \u0275\u0275listener("click", function SessionListComponent_For_2_Template_button_click_6_listener() {
      const session_r2 = \u0275\u0275restoreView(_r1).$implicit;
      const ctx_r2 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r2.deleteSession.emit(session_r2));
    });
    \u0275\u0275text(7, "Delete");
    \u0275\u0275elementEnd()()();
  }
  if (rf & 2) {
    const session_r2 = ctx.$implicit;
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(session_r2);
  }
}
var SessionListComponent = class _SessionListComponent {
  sessions = input.required();
  select = output();
  deleteSession = output();
  static \u0275fac = function SessionListComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _SessionListComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _SessionListComponent, selectors: [["app-session-list"]], inputs: { sessions: [1, "sessions"] }, outputs: { select: "select", deleteSession: "deleteSession" }, decls: 3, vars: 0, consts: [[1, "session-list"], [1, "session-card"], [1, "session-id", 3, "click"], [1, "session-actions"], [1, "btn", "btn-secondary", "btn-sm", 3, "click"], [1, "btn", "btn-danger", "btn-sm", 3, "click"]], template: function SessionListComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0);
      \u0275\u0275repeaterCreate(1, SessionListComponent_For_2_Template, 8, 1, "div", 1, \u0275\u0275repeaterTrackByIdentity);
      \u0275\u0275elementEnd();
    }
    if (rf & 2) {
      \u0275\u0275advance();
      \u0275\u0275repeater(ctx.sessions());
    }
  }, styles: ["\n\n.session-list[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n.session-card[_ngcontent-%COMP%] {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  padding: 0.75rem 1rem;\n  background: white;\n  border: 1px solid #e5e7eb;\n  border-radius: 6px;\n}\n.session-id[_ngcontent-%COMP%] {\n  cursor: pointer;\n  font-family: monospace;\n  font-size: 0.85rem;\n  color: #3b82f6;\n}\n.session-id[_ngcontent-%COMP%]:hover {\n  text-decoration: underline;\n}\n.session-actions[_ngcontent-%COMP%] {\n  display: flex;\n  gap: 0.5rem;\n}\n.btn-sm[_ngcontent-%COMP%] {\n  padding: 0.25rem 0.5rem;\n  font-size: 0.75rem;\n}\n/*# sourceMappingURL=session-list.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(SessionListComponent, [{
    type: Component,
    args: [{ selector: "app-session-list", standalone: true, template: '<div class="session-list">\r\n  @for (session of sessions(); track session) {\r\n    <div class="session-card">\r\n      <span class="session-id" (click)="select.emit(session)">{{ session }}</span>\r\n      <div class="session-actions">\r\n        <button class="btn btn-secondary btn-sm" (click)="select.emit(session)">View</button>\r\n        <button class="btn btn-danger btn-sm" (click)="deleteSession.emit(session)">Delete</button>\r\n      </div>\r\n    </div>\r\n  }\r\n</div>\r\n', styles: ["/* src/app/features/chat-history/components/session-list.component.scss */\n.session-list {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n.session-card {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  padding: 0.75rem 1rem;\n  background: white;\n  border: 1px solid #e5e7eb;\n  border-radius: 6px;\n}\n.session-id {\n  cursor: pointer;\n  font-family: monospace;\n  font-size: 0.85rem;\n  color: #3b82f6;\n}\n.session-id:hover {\n  text-decoration: underline;\n}\n.session-actions {\n  display: flex;\n  gap: 0.5rem;\n}\n.btn-sm {\n  padding: 0.25rem 0.5rem;\n  font-size: 0.75rem;\n}\n/*# sourceMappingURL=session-list.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(SessionListComponent, { className: "SessionListComponent", filePath: "src/app/features/chat-history/components/session-list.component.ts", lineNumber: 9 });
})();

// src/app/features/chat-history/components/session-detail-modal.component.ts
function SessionDetailModalComponent_For_9_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 6)(1, "strong");
    \u0275\u0275text(2);
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(3, "p");
    \u0275\u0275text(4);
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(5, "span", 7);
    \u0275\u0275text(6);
    \u0275\u0275pipe(7, "timeAgo");
    \u0275\u0275elementEnd()();
  }
  if (rf & 2) {
    const msg_r1 = ctx.$implicit;
    \u0275\u0275classProp("user", msg_r1.role === "user");
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(msg_r1.role);
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(msg_r1.content);
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(\u0275\u0275pipeBind1(7, 5, msg_r1.timestamp * 1e3));
  }
}
var SessionDetailModalComponent = class _SessionDetailModalComponent {
  data = input.required();
  close = output();
  static \u0275fac = function SessionDetailModalComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _SessionDetailModalComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _SessionDetailModalComponent, selectors: [["app-session-detail-modal"]], inputs: { data: [1, "data"] }, outputs: { close: "close" }, decls: 10, vars: 1, consts: [[1, "modal-overlay", 3, "click"], [1, "modal-content", 3, "click"], [1, "modal-header"], [1, "close-btn", 3, "click"], [1, "modal-body"], [1, "history-msg", 3, "user"], [1, "history-msg"], [1, "msg-time"]], template: function SessionDetailModalComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0);
      \u0275\u0275listener("click", function SessionDetailModalComponent_Template_div_click_0_listener() {
        return ctx.close.emit();
      });
      \u0275\u0275elementStart(1, "div", 1);
      \u0275\u0275listener("click", function SessionDetailModalComponent_Template_div_click_1_listener($event) {
        return $event.stopPropagation();
      });
      \u0275\u0275elementStart(2, "div", 2)(3, "h3");
      \u0275\u0275text(4);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(5, "button", 3);
      \u0275\u0275listener("click", function SessionDetailModalComponent_Template_button_click_5_listener() {
        return ctx.close.emit();
      });
      \u0275\u0275text(6, "\xD7");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(7, "div", 4);
      \u0275\u0275repeaterCreate(8, SessionDetailModalComponent_For_9_Template, 8, 7, "div", 5, \u0275\u0275repeaterTrackByIndex);
      \u0275\u0275elementEnd()()();
    }
    if (rf & 2) {
      \u0275\u0275advance(4);
      \u0275\u0275textInterpolate1("Session: ", ctx.data().sessionId, "");
      \u0275\u0275advance(4);
      \u0275\u0275repeater(ctx.data().messages);
    }
  }, dependencies: [TimeAgoPipe], styles: ["\n\n.modal-overlay[_ngcontent-%COMP%] {\n  position: fixed;\n  inset: 0;\n  background: rgba(0, 0, 0, 0.5);\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  z-index: 500;\n}\n.modal-content[_ngcontent-%COMP%] {\n  background: white;\n  border-radius: 8px;\n  max-width: 600px;\n  width: 90%;\n  max-height: 80vh;\n  overflow-y: auto;\n}\n.modal-header[_ngcontent-%COMP%] {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  padding: 1rem 1.5rem;\n  border-bottom: 1px solid #e5e7eb;\n}\n.modal-header[_ngcontent-%COMP%]   h3[_ngcontent-%COMP%] {\n  font-size: 0.9rem;\n  font-family: monospace;\n}\n.close-btn[_ngcontent-%COMP%] {\n  background: none;\n  border: none;\n  font-size: 1.5rem;\n  cursor: pointer;\n  color: #6b7280;\n}\n.modal-body[_ngcontent-%COMP%] {\n  padding: 1rem 1.5rem;\n}\n.history-msg[_ngcontent-%COMP%] {\n  padding: 0.5rem 0;\n  border-bottom: 1px solid #f3f4f6;\n}\n.history-msg.user[_ngcontent-%COMP%]   strong[_ngcontent-%COMP%] {\n  color: #3b82f6;\n}\n.history-msg[_ngcontent-%COMP%]   strong[_ngcontent-%COMP%] {\n  text-transform: capitalize;\n}\n.history-msg[_ngcontent-%COMP%]   p[_ngcontent-%COMP%] {\n  margin: 0.25rem 0;\n}\n.msg-time[_ngcontent-%COMP%] {\n  font-size: 0.7rem;\n  color: #9ca3af;\n}\n/*# sourceMappingURL=session-detail-modal.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(SessionDetailModalComponent, [{
    type: Component,
    args: [{ selector: "app-session-detail-modal", standalone: true, imports: [TimeAgoPipe], template: `<div class="modal-overlay" (click)="close.emit()">\r
  <div class="modal-content" (click)="$event.stopPropagation()">\r
    <div class="modal-header">\r
      <h3>Session: {{ data().sessionId }}</h3>\r
      <button class="close-btn" (click)="close.emit()">&times;</button>\r
    </div>\r
    <div class="modal-body">\r
      @for (msg of data().messages; track $index) {\r
        <div class="history-msg" [class.user]="msg.role === 'user'">\r
          <strong>{{ msg.role }}</strong>\r
          <p>{{ msg.content }}</p>\r
          <span class="msg-time">{{ msg.timestamp * 1000 | timeAgo }}</span>\r
        </div>\r
      }\r
    </div>\r
  </div>\r
</div>\r
`, styles: ["/* src/app/features/chat-history/components/session-detail-modal.component.scss */\n.modal-overlay {\n  position: fixed;\n  inset: 0;\n  background: rgba(0, 0, 0, 0.5);\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  z-index: 500;\n}\n.modal-content {\n  background: white;\n  border-radius: 8px;\n  max-width: 600px;\n  width: 90%;\n  max-height: 80vh;\n  overflow-y: auto;\n}\n.modal-header {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  padding: 1rem 1.5rem;\n  border-bottom: 1px solid #e5e7eb;\n}\n.modal-header h3 {\n  font-size: 0.9rem;\n  font-family: monospace;\n}\n.close-btn {\n  background: none;\n  border: none;\n  font-size: 1.5rem;\n  cursor: pointer;\n  color: #6b7280;\n}\n.modal-body {\n  padding: 1rem 1.5rem;\n}\n.history-msg {\n  padding: 0.5rem 0;\n  border-bottom: 1px solid #f3f4f6;\n}\n.history-msg.user strong {\n  color: #3b82f6;\n}\n.history-msg strong {\n  text-transform: capitalize;\n}\n.history-msg p {\n  margin: 0.25rem 0;\n}\n.msg-time {\n  font-size: 0.7rem;\n  color: #9ca3af;\n}\n/*# sourceMappingURL=session-detail-modal.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(SessionDetailModalComponent, { className: "SessionDetailModalComponent", filePath: "src/app/features/chat-history/components/session-detail-modal.component.ts", lineNumber: 12 });
})();

// src/app/features/chat-history/containers/chat-history-page.component.ts
function ChatHistoryPageComponent_Case_2_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function ChatHistoryPageComponent_Case_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function ChatHistoryPageComponent_Case_4_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-empty-state", 3);
  }
}
function ChatHistoryPageComponent_Case_4_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-session-list", 5);
    \u0275\u0275listener("select", function ChatHistoryPageComponent_Case_4_Conditional_1_Template_app_session_list_select_0_listener($event) {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext(2);
      return \u0275\u0275resetView(ctx_r1.onSelect($event));
    })("deleteSession", function ChatHistoryPageComponent_Case_4_Conditional_1_Template_app_session_list_deleteSession_0_listener($event) {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext(2);
      return \u0275\u0275resetView(ctx_r1.onDelete($event));
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext(2);
    \u0275\u0275property("sessions", ctx_r1.sessions());
  }
}
function ChatHistoryPageComponent_Case_4_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275template(0, ChatHistoryPageComponent_Case_4_Conditional_0_Template, 1, 0, "app-empty-state", 3)(1, ChatHistoryPageComponent_Case_4_Conditional_1_Template, 1, 1, "app-session-list", 4);
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275conditional(ctx_r1.sessions().length === 0 ? 0 : 1);
  }
}
function ChatHistoryPageComponent_Case_5_Template(rf, ctx) {
  if (rf & 1) {
    const _r3 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-error-display", 6);
    \u0275\u0275listener("retry", function ChatHistoryPageComponent_Case_5_Template_app_error_display_retry_0_listener() {
      \u0275\u0275restoreView(_r3);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.chatSvc.loadActiveSessions());
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275property("message", ctx_r1.sessionsError());
  }
}
function ChatHistoryPageComponent_Conditional_6_Template(rf, ctx) {
  if (rf & 1) {
    const _r4 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-session-detail-modal", 7);
    \u0275\u0275listener("close", function ChatHistoryPageComponent_Conditional_6_Template_app_session_detail_modal_close_0_listener() {
      \u0275\u0275restoreView(_r4);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.selectedHistory.set(null));
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275property("data", ctx_r1.selectedHistory());
  }
}
var ChatHistoryPageComponent = class _ChatHistoryPageComponent {
  chatSvc = inject(ChatService);
  selectedHistory = signal(null);
  sessions = () => {
    const s = this.chatSvc.sessionsState();
    return s.stage === "loaded" ? s.data.sessions : [];
  };
  sessionsError = () => {
    const s = this.chatSvc.sessionsState();
    return s.stage === "error" ? s.error : "";
  };
  ngOnInit() {
    this.chatSvc.loadActiveSessions();
  }
  onSelect(sessionId) {
    this.chatSvc.loadHistory(sessionId);
    const check = setInterval(() => {
      const hs = this.chatSvc.historyState();
      if (hs.stage === "loaded") {
        this.selectedHistory.set(hs.data);
        clearInterval(check);
      } else if (hs.stage === "error") {
        clearInterval(check);
      }
    }, 100);
    setTimeout(() => clearInterval(check), 1e4);
  }
  onDelete(sessionId) {
    if (confirm(`Delete session ${sessionId}?`)) {
      this.chatSvc.deleteSession(sessionId);
    }
  }
  static \u0275fac = function ChatHistoryPageComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ChatHistoryPageComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ChatHistoryPageComponent, selectors: [["app-chat-history-page"]], decls: 7, vars: 2, consts: [[1, "page-title"], [3, "message"], [3, "data"], ["message", "No chat sessions found"], [3, "sessions"], [3, "select", "deleteSession", "sessions"], [3, "retry", "message"], [3, "close", "data"]], template: function ChatHistoryPageComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "h1", 0);
      \u0275\u0275text(1, "Chat History");
      \u0275\u0275elementEnd();
      \u0275\u0275template(2, ChatHistoryPageComponent_Case_2_Template, 1, 0, "app-loading-spinner")(3, ChatHistoryPageComponent_Case_3_Template, 1, 0, "app-loading-spinner")(4, ChatHistoryPageComponent_Case_4_Template, 2, 1)(5, ChatHistoryPageComponent_Case_5_Template, 1, 1, "app-error-display", 1)(6, ChatHistoryPageComponent_Conditional_6_Template, 1, 1, "app-session-detail-modal", 2);
    }
    if (rf & 2) {
      let tmp_0_0;
      \u0275\u0275advance(2);
      \u0275\u0275conditional((tmp_0_0 = ctx.chatSvc.sessionsState().stage) === "idle" ? 2 : tmp_0_0 === "loading" ? 3 : tmp_0_0 === "loaded" ? 4 : tmp_0_0 === "error" ? 5 : -1);
      \u0275\u0275advance(4);
      \u0275\u0275conditional(ctx.selectedHistory() ? 6 : -1);
    }
  }, dependencies: [
    SessionListComponent,
    SessionDetailModalComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent
  ], encapsulation: 2 });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ChatHistoryPageComponent, [{
    type: Component,
    args: [{ selector: "app-chat-history-page", standalone: true, imports: [
      SessionListComponent,
      SessionDetailModalComponent,
      LoadingSpinnerComponent,
      ErrorDisplayComponent,
      EmptyStateComponent
    ], template: `<h1 class="page-title">Chat History</h1>\r
\r
@switch (chatSvc.sessionsState().stage) {\r
  @case ('idle') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loading') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loaded') {\r
    @if (sessions().length === 0) {\r
      <app-empty-state message="No chat sessions found" />\r
    } @else {\r
      <app-session-list\r
        [sessions]="sessions()"\r
        (select)="onSelect($event)"\r
        (deleteSession)="onDelete($event)"\r
      />\r
    }\r
  }\r
  @case ('error') {\r
    <app-error-display [message]="sessionsError()" (retry)="chatSvc.loadActiveSessions()" />\r
  }\r
}\r
\r
@if (selectedHistory()) {\r
  <app-session-detail-modal\r
    [data]="selectedHistory()!"\r
    (close)="selectedHistory.set(null)"\r
  />\r
}\r
` }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ChatHistoryPageComponent, { className: "ChatHistoryPageComponent", filePath: "src/app/features/chat-history/containers/chat-history-page.component.ts", lineNumber: 22 });
})();
export {
  ChatHistoryPageComponent
};
//# sourceMappingURL=chunk-ABZ5JVI3.js.map
