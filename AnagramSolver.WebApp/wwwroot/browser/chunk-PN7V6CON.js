import {
  Component,
  input,
  setClassMetadata,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵdefineComponent,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵtext,
  ɵɵtextInterpolate
} from "./chunk-5B3AD7EP.js";

// src/app/shared/components/empty-state/empty-state.component.ts
var EmptyStateComponent = class _EmptyStateComponent {
  message = input("No data available");
  static \u0275fac = function EmptyStateComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _EmptyStateComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _EmptyStateComponent, selectors: [["app-empty-state"]], inputs: { message: [1, "message"] }, decls: 5, vars: 1, consts: [[1, "empty-state"], [1, "empty-icon"]], template: function EmptyStateComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0)(1, "div", 1);
      \u0275\u0275text(2, "\u{1F4ED}");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "p");
      \u0275\u0275text(4);
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance(4);
      \u0275\u0275textInterpolate(ctx.message());
    }
  }, styles: ["\n\n.empty-state[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 2rem;\n  color: #9ca3af;\n}\n.empty-icon[_ngcontent-%COMP%] {\n  font-size: 2.5rem;\n}\n/*# sourceMappingURL=empty-state.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(EmptyStateComponent, [{
    type: Component,
    args: [{ selector: "app-empty-state", standalone: true, template: '<div class="empty-state">\r\n  <div class="empty-icon">\u{1F4ED}</div>\r\n  <p>{{ message() }}</p>\r\n</div>\r\n', styles: ["/* src/app/shared/components/empty-state/empty-state.component.scss */\n.empty-state {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 2rem;\n  color: #9ca3af;\n}\n.empty-icon {\n  font-size: 2.5rem;\n}\n/*# sourceMappingURL=empty-state.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(EmptyStateComponent, { className: "EmptyStateComponent", filePath: "src/app/shared/components/empty-state/empty-state.component.ts", lineNumber: 9 });
})();

export {
  EmptyStateComponent
};
//# sourceMappingURL=chunk-PN7V6CON.js.map
