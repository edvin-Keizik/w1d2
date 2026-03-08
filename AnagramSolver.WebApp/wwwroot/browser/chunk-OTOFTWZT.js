import {
  Component,
  input,
  output,
  setClassMetadata,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵdefineComponent,
  ɵɵelement,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵlistener,
  ɵɵtext,
  ɵɵtextInterpolate
} from "./chunk-5B3AD7EP.js";

// src/app/shared/components/loading-spinner/loading-spinner.component.ts
var LoadingSpinnerComponent = class _LoadingSpinnerComponent {
  static \u0275fac = function LoadingSpinnerComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _LoadingSpinnerComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _LoadingSpinnerComponent, selectors: [["app-loading-spinner"]], decls: 4, vars: 0, consts: [[1, "spinner-container"], [1, "spinner"]], template: function LoadingSpinnerComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0);
      \u0275\u0275element(1, "div", 1);
      \u0275\u0275elementStart(2, "p");
      \u0275\u0275text(3, "Loading...");
      \u0275\u0275elementEnd()();
    }
  }, styles: ["\n\n.spinner-container[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 2rem;\n}\n.spinner[_ngcontent-%COMP%] {\n  width: 40px;\n  height: 40px;\n  border: 4px solid #e0e0e0;\n  border-top-color: #3b82f6;\n  border-radius: 50%;\n  animation: _ngcontent-%COMP%_spin 0.8s linear infinite;\n}\n@keyframes _ngcontent-%COMP%_spin {\n  to {\n    transform: rotate(360deg);\n  }\n}\np[_ngcontent-%COMP%] {\n  margin-top: 0.5rem;\n  color: #6b7280;\n}\n/*# sourceMappingURL=loading-spinner.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(LoadingSpinnerComponent, [{
    type: Component,
    args: [{ selector: "app-loading-spinner", standalone: true, template: '<div class="spinner-container">\r\n  <div class="spinner"></div>\r\n  <p>Loading...</p>\r\n</div>\r\n', styles: ["/* src/app/shared/components/loading-spinner/loading-spinner.component.scss */\n.spinner-container {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 2rem;\n}\n.spinner {\n  width: 40px;\n  height: 40px;\n  border: 4px solid #e0e0e0;\n  border-top-color: #3b82f6;\n  border-radius: 50%;\n  animation: spin 0.8s linear infinite;\n}\n@keyframes spin {\n  to {\n    transform: rotate(360deg);\n  }\n}\np {\n  margin-top: 0.5rem;\n  color: #6b7280;\n}\n/*# sourceMappingURL=loading-spinner.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(LoadingSpinnerComponent, { className: "LoadingSpinnerComponent", filePath: "src/app/shared/components/loading-spinner/loading-spinner.component.ts", lineNumber: 9 });
})();

// src/app/shared/components/error-display/error-display.component.ts
var ErrorDisplayComponent = class _ErrorDisplayComponent {
  message = input.required();
  retry = output();
  static \u0275fac = function ErrorDisplayComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ErrorDisplayComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ErrorDisplayComponent, selectors: [["app-error-display"]], inputs: { message: [1, "message"] }, outputs: { retry: "retry" }, decls: 7, vars: 1, consts: [[1, "error-container"], [1, "error-icon"], [1, "error-message"], [1, "retry-btn", 3, "click"]], template: function ErrorDisplayComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0)(1, "div", 1);
      \u0275\u0275text(2, "\u26A0\uFE0F");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "p", 2);
      \u0275\u0275text(4);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(5, "button", 3);
      \u0275\u0275listener("click", function ErrorDisplayComponent_Template_button_click_5_listener() {
        return ctx.retry.emit();
      });
      \u0275\u0275text(6, "Try Again");
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance(4);
      \u0275\u0275textInterpolate(ctx.message());
    }
  }, styles: ["\n\n.error-container[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 2rem;\n  background: #fef2f2;\n  border: 1px solid #fecaca;\n  border-radius: 8px;\n}\n.error-icon[_ngcontent-%COMP%] {\n  font-size: 2rem;\n}\n.error-message[_ngcontent-%COMP%] {\n  color: #dc2626;\n  margin: 0.5rem 0;\n}\n.retry-btn[_ngcontent-%COMP%] {\n  padding: 0.5rem 1rem;\n  background: #dc2626;\n  color: white;\n  border: none;\n  border-radius: 4px;\n  cursor: pointer;\n}\n.retry-btn[_ngcontent-%COMP%]:hover {\n  background: #b91c1c;\n}\n/*# sourceMappingURL=error-display.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ErrorDisplayComponent, [{
    type: Component,
    args: [{ selector: "app-error-display", standalone: true, template: '<div class="error-container">\r\n  <div class="error-icon">\u26A0\uFE0F</div>\r\n  <p class="error-message">{{ message() }}</p>\r\n  <button class="retry-btn" (click)="retry.emit()">Try Again</button>\r\n</div>\r\n', styles: ["/* src/app/shared/components/error-display/error-display.component.scss */\n.error-container {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 2rem;\n  background: #fef2f2;\n  border: 1px solid #fecaca;\n  border-radius: 8px;\n}\n.error-icon {\n  font-size: 2rem;\n}\n.error-message {\n  color: #dc2626;\n  margin: 0.5rem 0;\n}\n.retry-btn {\n  padding: 0.5rem 1rem;\n  background: #dc2626;\n  color: white;\n  border: none;\n  border-radius: 4px;\n  cursor: pointer;\n}\n.retry-btn:hover {\n  background: #b91c1c;\n}\n/*# sourceMappingURL=error-display.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ErrorDisplayComponent, { className: "ErrorDisplayComponent", filePath: "src/app/shared/components/error-display/error-display.component.ts", lineNumber: 9 });
})();

export {
  LoadingSpinnerComponent,
  ErrorDisplayComponent
};
//# sourceMappingURL=chunk-OTOFTWZT.js.map
