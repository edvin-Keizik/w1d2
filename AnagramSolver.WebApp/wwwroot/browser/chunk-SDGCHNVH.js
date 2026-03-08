import {
  DefaultValueAccessor,
  FormControl,
  FormControlDirective,
  NgControlStatus,
  NgControlStatusGroup,
  ReactiveFormsModule,
  Validators,
  ɵNgNoValidate
} from "./chunk-YO6CZKCV.js";
import "./chunk-DYQFFD5A.js";
import {
  ChatService
} from "./chunk-LU74ZHSU.js";
import {
  TimeAgoPipe
} from "./chunk-K5NYSZAZ.js";
import {
  Component,
  ViewChild,
  inject,
  input,
  output,
  setClassMetadata,
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
  ɵɵloadQuery,
  ɵɵnextContext,
  ɵɵpipe,
  ɵɵpipeBind1,
  ɵɵproperty,
  ɵɵqueryRefresh,
  ɵɵrepeater,
  ɵɵrepeaterCreate,
  ɵɵrepeaterTrackByIndex,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate,
  ɵɵviewQuery
} from "./chunk-5B3AD7EP.js";

// src/app/features/ai-chat/components/message-bubble.component.ts
var MessageBubbleComponent = class _MessageBubbleComponent {
  message = input.required();
  static \u0275fac = function MessageBubbleComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _MessageBubbleComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _MessageBubbleComponent, selectors: [["app-message-bubble"]], inputs: { message: [1, "message"] }, decls: 6, vars: 8, consts: [[1, "bubble"], [1, "bubble-content"], [1, "bubble-meta"]], template: function MessageBubbleComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0)(1, "div", 1);
      \u0275\u0275text(2);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "div", 2);
      \u0275\u0275text(4);
      \u0275\u0275pipe(5, "timeAgo");
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275classProp("user", ctx.message().role === "user")("assistant", ctx.message().role !== "user");
      \u0275\u0275advance(2);
      \u0275\u0275textInterpolate(ctx.message().content);
      \u0275\u0275advance(2);
      \u0275\u0275textInterpolate(\u0275\u0275pipeBind1(5, 6, ctx.message().timestamp * 1e3));
    }
  }, dependencies: [TimeAgoPipe], styles: ["\n\n.bubble[_ngcontent-%COMP%] {\n  max-width: 75%;\n  padding: 0.75rem 1rem;\n  border-radius: 12px;\n  line-height: 1.4;\n}\n.user[_ngcontent-%COMP%] {\n  align-self: flex-end;\n  background: #3b82f6;\n  color: white;\n  border-bottom-right-radius: 4px;\n}\n.assistant[_ngcontent-%COMP%] {\n  align-self: flex-start;\n  background: white;\n  border: 1px solid #e5e7eb;\n  border-bottom-left-radius: 4px;\n}\n.bubble-meta[_ngcontent-%COMP%] {\n  font-size: 0.65rem;\n  margin-top: 0.25rem;\n  opacity: 0.6;\n}\n/*# sourceMappingURL=message-bubble.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(MessageBubbleComponent, [{
    type: Component,
    args: [{ selector: "app-message-bubble", standalone: true, imports: [TimeAgoPipe], template: `<div class="bubble" [class.user]="message().role === 'user'" [class.assistant]="message().role !== 'user'">\r
  <div class="bubble-content">{{ message().content }}</div>\r
  <div class="bubble-meta">{{ message().timestamp * 1000 | timeAgo }}</div>\r
</div>\r
`, styles: ["/* src/app/features/ai-chat/components/message-bubble.component.scss */\n.bubble {\n  max-width: 75%;\n  padding: 0.75rem 1rem;\n  border-radius: 12px;\n  line-height: 1.4;\n}\n.user {\n  align-self: flex-end;\n  background: #3b82f6;\n  color: white;\n  border-bottom-right-radius: 4px;\n}\n.assistant {\n  align-self: flex-start;\n  background: white;\n  border: 1px solid #e5e7eb;\n  border-bottom-left-radius: 4px;\n}\n.bubble-meta {\n  font-size: 0.65rem;\n  margin-top: 0.25rem;\n  opacity: 0.6;\n}\n/*# sourceMappingURL=message-bubble.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(MessageBubbleComponent, { className: "MessageBubbleComponent", filePath: "src/app/features/ai-chat/components/message-bubble.component.ts", lineNumber: 12 });
})();

// src/app/features/ai-chat/components/chat-window.component.ts
function ChatWindowComponent_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 1);
    \u0275\u0275text(1, "Start a conversation with AI assistant");
    \u0275\u0275elementEnd();
  }
}
function ChatWindowComponent_For_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-message-bubble", 2);
  }
  if (rf & 2) {
    const msg_r1 = ctx.$implicit;
    \u0275\u0275property("message", msg_r1);
  }
}
var ChatWindowComponent = class _ChatWindowComponent {
  messages = input.required();
  static \u0275fac = function ChatWindowComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ChatWindowComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ChatWindowComponent, selectors: [["app-chat-window"]], inputs: { messages: [1, "messages"] }, decls: 4, vars: 1, consts: [[1, "chat-window"], [1, "chat-empty"], [3, "message"]], template: function ChatWindowComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0);
      \u0275\u0275template(1, ChatWindowComponent_Conditional_1_Template, 2, 0, "div", 1);
      \u0275\u0275repeaterCreate(2, ChatWindowComponent_For_3_Template, 1, 1, "app-message-bubble", 2, \u0275\u0275repeaterTrackByIndex);
      \u0275\u0275elementEnd();
    }
    if (rf & 2) {
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.messages().length === 0 ? 1 : -1);
      \u0275\u0275advance();
      \u0275\u0275repeater(ctx.messages());
    }
  }, dependencies: [MessageBubbleComponent], styles: ["\n\n.chat-window[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  gap: 0.75rem;\n  padding: 1rem;\n  max-height: 500px;\n  overflow-y: auto;\n  background: #f9fafb;\n  border-radius: 8px;\n  border: 1px solid #e5e7eb;\n}\n.chat-empty[_ngcontent-%COMP%] {\n  text-align: center;\n  color: #9ca3af;\n  padding: 3rem;\n}\n/*# sourceMappingURL=chat-window.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ChatWindowComponent, [{
    type: Component,
    args: [{ selector: "app-chat-window", standalone: true, imports: [MessageBubbleComponent], template: '<div class="chat-window">\r\n  @if (messages().length === 0) {\r\n    <div class="chat-empty">Start a conversation with AI assistant</div>\r\n  }\r\n  @for (msg of messages(); track $index) {\r\n    <app-message-bubble [message]="msg" />\r\n  }\r\n</div>\r\n', styles: ["/* src/app/features/ai-chat/components/chat-window.component.scss */\n.chat-window {\n  display: flex;\n  flex-direction: column;\n  gap: 0.75rem;\n  padding: 1rem;\n  max-height: 500px;\n  overflow-y: auto;\n  background: #f9fafb;\n  border-radius: 8px;\n  border: 1px solid #e5e7eb;\n}\n.chat-empty {\n  text-align: center;\n  color: #9ca3af;\n  padding: 3rem;\n}\n/*# sourceMappingURL=chat-window.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ChatWindowComponent, { className: "ChatWindowComponent", filePath: "src/app/features/ai-chat/components/chat-window.component.ts", lineNumber: 12 });
})();

// src/app/features/ai-chat/components/chat-input.component.ts
function ChatInputComponent_Conditional_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " ... ");
  }
}
function ChatInputComponent_Conditional_4_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Send ");
  }
}
var ChatInputComponent = class _ChatInputComponent {
  isSending = input(false);
  sendMessage = output();
  messageControl = new FormControl("", {
    nonNullable: true,
    validators: [Validators.required]
  });
  onSend() {
    if (this.messageControl.valid) {
      this.sendMessage.emit(this.messageControl.value.trim());
      this.messageControl.reset();
    }
  }
  static \u0275fac = function ChatInputComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ChatInputComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ChatInputComponent, selectors: [["app-chat-input"]], inputs: { isSending: [1, "isSending"] }, outputs: { sendMessage: "sendMessage" }, decls: 5, vars: 3, consts: [[1, "chat-input", 3, "ngSubmit"], ["type", "text", "placeholder", "Type a message...", 3, "keydown.enter", "formControl"], ["type", "submit", 1, "btn", "btn-primary", 3, "disabled"]], template: function ChatInputComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "form", 0);
      \u0275\u0275listener("ngSubmit", function ChatInputComponent_Template_form_ngSubmit_0_listener() {
        return ctx.onSend();
      });
      \u0275\u0275elementStart(1, "input", 1);
      \u0275\u0275listener("keydown.enter", function ChatInputComponent_Template_input_keydown_enter_1_listener($event) {
        ctx.onSend();
        return $event.preventDefault();
      });
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(2, "button", 2);
      \u0275\u0275template(3, ChatInputComponent_Conditional_3_Template, 1, 0)(4, ChatInputComponent_Conditional_4_Template, 1, 0);
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance();
      \u0275\u0275property("formControl", ctx.messageControl);
      \u0275\u0275advance();
      \u0275\u0275property("disabled", ctx.messageControl.invalid || ctx.isSending());
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.isSending() ? 3 : 4);
    }
  }, dependencies: [ReactiveFormsModule, \u0275NgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, FormControlDirective], styles: ["\n\n.chat-input[_ngcontent-%COMP%] {\n  display: flex;\n  gap: 0.5rem;\n  padding: 0.75rem;\n  background: white;\n  border-top: 1px solid #e5e7eb;\n  border-radius: 0 0 8px 8px;\n}\n.chat-input[_ngcontent-%COMP%]   input[_ngcontent-%COMP%] {\n  flex: 1;\n  padding: 0.5rem 0.75rem;\n  border: 1px solid #d1d5db;\n  border-radius: 6px;\n  font-size: 0.95rem;\n}\n.chat-input[_ngcontent-%COMP%]   input[_ngcontent-%COMP%]:focus {\n  outline: none;\n  border-color: #3b82f6;\n}\n/*# sourceMappingURL=chat-input.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ChatInputComponent, [{
    type: Component,
    args: [{ selector: "app-chat-input", standalone: true, imports: [ReactiveFormsModule], template: '<form class="chat-input" (ngSubmit)="onSend()">\r\n  <input\r\n    type="text"\r\n    [formControl]="messageControl"\r\n    placeholder="Type a message..."\r\n    (keydown.enter)="onSend(); $event.preventDefault()"\r\n  />\r\n  <button\r\n    type="submit"\r\n    class="btn btn-primary"\r\n    [disabled]="messageControl.invalid || isSending()"\r\n  >\r\n    @if (isSending()) {\r\n      ...\r\n    } @else {\r\n      Send\r\n    }\r\n  </button>\r\n</form>\r\n', styles: ["/* src/app/features/ai-chat/components/chat-input.component.scss */\n.chat-input {\n  display: flex;\n  gap: 0.5rem;\n  padding: 0.75rem;\n  background: white;\n  border-top: 1px solid #e5e7eb;\n  border-radius: 0 0 8px 8px;\n}\n.chat-input input {\n  flex: 1;\n  padding: 0.5rem 0.75rem;\n  border: 1px solid #d1d5db;\n  border-radius: 6px;\n  font-size: 0.95rem;\n}\n.chat-input input:focus {\n  outline: none;\n  border-color: #3b82f6;\n}\n/*# sourceMappingURL=chat-input.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ChatInputComponent, { className: "ChatInputComponent", filePath: "src/app/features/ai-chat/components/chat-input.component.ts", lineNumber: 11 });
})();

// src/app/features/ai-chat/containers/chat-page.component.ts
var _c0 = ["chatContainer"];
function ChatPageComponent_Conditional_9_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 7);
    \u0275\u0275text(1);
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275advance();
    \u0275\u0275textInterpolate(ctx_r1.chatErrorMsg());
  }
}
var ChatPageComponent = class _ChatPageComponent {
  chatSvc = inject(ChatService);
  _chatContainer;
  chatErrorMsg = () => {
    const s = this.chatSvc.sendState();
    return s.stage === "error" ? s.error : "";
  };
  ngAfterViewChecked() {
    this._scrollToBottom();
  }
  _scrollToBottom() {
    const el = this._chatContainer?.nativeElement;
    if (el) {
      el.scrollTop = el.scrollHeight;
    }
  }
  static \u0275fac = function ChatPageComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ChatPageComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ChatPageComponent, selectors: [["app-chat-page"]], viewQuery: function ChatPageComponent_Query(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275viewQuery(_c0, 5);
    }
    if (rf & 2) {
      let _t;
      \u0275\u0275queryRefresh(_t = \u0275\u0275loadQuery()) && (ctx._chatContainer = _t.first);
    }
  }, decls: 10, vars: 3, consts: [["chatContainer", ""], [1, "chat-header"], [1, "page-title"], [1, "btn", "btn-secondary", 3, "click"], [1, "chat-container"], [3, "messages"], [3, "sendMessage", "isSending"], [1, "chat-error"]], template: function ChatPageComponent_Template(rf, ctx) {
    if (rf & 1) {
      const _r1 = \u0275\u0275getCurrentView();
      \u0275\u0275elementStart(0, "div", 1)(1, "h1", 2);
      \u0275\u0275text(2, "AI Chat");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "button", 3);
      \u0275\u0275listener("click", function ChatPageComponent_Template_button_click_3_listener() {
        \u0275\u0275restoreView(_r1);
        return \u0275\u0275resetView(ctx.chatSvc.startNewSession());
      });
      \u0275\u0275text(4, "New Session");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(5, "div", 4, 0);
      \u0275\u0275element(7, "app-chat-window", 5);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(8, "app-chat-input", 6);
      \u0275\u0275listener("sendMessage", function ChatPageComponent_Template_app_chat_input_sendMessage_8_listener($event) {
        \u0275\u0275restoreView(_r1);
        return \u0275\u0275resetView(ctx.chatSvc.send($event));
      });
      \u0275\u0275elementEnd();
      \u0275\u0275template(9, ChatPageComponent_Conditional_9_Template, 2, 1, "div", 7);
    }
    if (rf & 2) {
      \u0275\u0275advance(7);
      \u0275\u0275property("messages", ctx.chatSvc.messages());
      \u0275\u0275advance();
      \u0275\u0275property("isSending", ctx.chatSvc.sendState().stage === "loading");
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.chatSvc.sendState().stage === "error" ? 9 : -1);
    }
  }, dependencies: [ChatWindowComponent, ChatInputComponent], styles: ["\n\n.chat-header[_ngcontent-%COMP%] {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.chat-container[_ngcontent-%COMP%] {\n  margin-bottom: 0;\n}\n.chat-error[_ngcontent-%COMP%] {\n  margin-top: 0.5rem;\n  padding: 0.5rem;\n  background: #fef2f2;\n  color: #dc2626;\n  border-radius: 6px;\n  font-size: 0.875rem;\n}\n/*# sourceMappingURL=chat-page.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ChatPageComponent, [{
    type: Component,
    args: [{ selector: "app-chat-page", standalone: true, imports: [ChatWindowComponent, ChatInputComponent], template: `<div class="chat-header">\r
  <h1 class="page-title">AI Chat</h1>\r
  <button class="btn btn-secondary" (click)="chatSvc.startNewSession()">New Session</button>\r
</div>\r
\r
<div class="chat-container" #chatContainer>\r
  <app-chat-window [messages]="chatSvc.messages()" />\r
</div>\r
\r
<app-chat-input\r
  [isSending]="chatSvc.sendState().stage === 'loading'"\r
  (sendMessage)="chatSvc.send($event)"\r
/>\r
\r
@if (chatSvc.sendState().stage === 'error') {\r
  <div class="chat-error">{{ chatErrorMsg() }}</div>\r
}\r
`, styles: ["/* src/app/features/ai-chat/containers/chat-page.component.scss */\n.chat-header {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n}\n.chat-container {\n  margin-bottom: 0;\n}\n.chat-error {\n  margin-top: 0.5rem;\n  padding: 0.5rem;\n  background: #fef2f2;\n  color: #dc2626;\n  border-radius: 6px;\n  font-size: 0.875rem;\n}\n/*# sourceMappingURL=chat-page.component.css.map */\n"] }]
  }], null, { _chatContainer: [{
    type: ViewChild,
    args: ["chatContainer"]
  }] });
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ChatPageComponent, { className: "ChatPageComponent", filePath: "src/app/features/ai-chat/containers/chat-page.component.ts", lineNumber: 13 });
})();
export {
  ChatPageComponent
};
//# sourceMappingURL=chunk-SDGCHNVH.js.map
