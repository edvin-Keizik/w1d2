import {
  DictionaryService
} from "./chunk-6PNKKLZZ.js";
import {
  Router
} from "./chunk-EYJBRYUB.js";
import "./chunk-YPJFZ374.js";
import "./chunk-DYQFFD5A.js";
import {
  EmptyStateComponent
} from "./chunk-PN7V6CON.js";
import {
  ErrorDisplayComponent,
  LoadingSpinnerComponent
} from "./chunk-OTOFTWZT.js";
import {
  Component,
  Pipe,
  computed,
  inject,
  input,
  output,
  setClassMetadata,
  signal,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵconditional,
  ɵɵdefineComponent,
  ɵɵdefinePipe,
  ɵɵelement,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵgetCurrentView,
  ɵɵlistener,
  ɵɵnextContext,
  ɵɵpipe,
  ɵɵpipeBind2,
  ɵɵproperty,
  ɵɵrepeater,
  ɵɵrepeaterCreate,
  ɵɵrepeaterTrackByIdentity,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate1,
  ɵɵtextInterpolate2
} from "./chunk-5B3AD7EP.js";

// src/app/shared/pipes/truncate.pipe.ts
var TruncatePipe = class _TruncatePipe {
  transform(value, maxLength = 30) {
    if (!value || value.length <= maxLength)
      return value;
    return value.substring(0, maxLength) + "\u2026";
  }
  static \u0275fac = function TruncatePipe_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _TruncatePipe)();
  };
  static \u0275pipe = /* @__PURE__ */ \u0275\u0275definePipe({ name: "truncate", type: _TruncatePipe, pure: true });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(TruncatePipe, [{
    type: Pipe,
    args: [{ name: "truncate", standalone: true }]
  }], null, null);
})();

// src/app/features/dictionary/components/word-grid.component.ts
function WordGridComponent_For_2_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "div", 2);
    \u0275\u0275listener("click", function WordGridComponent_For_2_Template_div_click_0_listener() {
      const word_r2 = \u0275\u0275restoreView(_r1).$implicit;
      const ctx_r2 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r2.wordClick.emit(word_r2));
    });
    \u0275\u0275text(1);
    \u0275\u0275pipe(2, "truncate");
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const word_r2 = ctx.$implicit;
    \u0275\u0275advance();
    \u0275\u0275textInterpolate1(" ", \u0275\u0275pipeBind2(2, 1, word_r2, 25), " ");
  }
}
var WordGridComponent = class _WordGridComponent {
  words = input.required();
  wordClick = output();
  static \u0275fac = function WordGridComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _WordGridComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _WordGridComponent, selectors: [["app-word-grid"]], inputs: { words: [1, "words"] }, outputs: { wordClick: "wordClick" }, decls: 3, vars: 0, consts: [[1, "word-grid"], [1, "word-card"], [1, "word-card", 3, "click"]], template: function WordGridComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0);
      \u0275\u0275repeaterCreate(1, WordGridComponent_For_2_Template, 3, 4, "div", 1, \u0275\u0275repeaterTrackByIdentity);
      \u0275\u0275elementEnd();
    }
    if (rf & 2) {
      \u0275\u0275advance();
      \u0275\u0275repeater(ctx.words());
    }
  }, dependencies: [TruncatePipe], styles: ["\n\n.word-grid[_ngcontent-%COMP%] {\n  display: grid;\n  grid-template-columns: repeat(3, 1fr);\n  gap: 0.5rem;\n}\n.word-card[_ngcontent-%COMP%] {\n  padding: 0.5rem 0.75rem;\n  background: white;\n  border: 1px solid #e5e7eb;\n  border-radius: 6px;\n  cursor: pointer;\n  text-align: center;\n  transition: background 0.15s, border-color 0.15s;\n}\n.word-card[_ngcontent-%COMP%]:hover {\n  background: #eff6ff;\n  border-color: #3b82f6;\n}\n@media (max-width: 640px) {\n  .word-grid[_ngcontent-%COMP%] {\n    grid-template-columns: repeat(2, 1fr);\n  }\n}\n/*# sourceMappingURL=word-grid.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(WordGridComponent, [{
    type: Component,
    args: [{ selector: "app-word-grid", standalone: true, imports: [TruncatePipe], template: '<div class="word-grid">\r\n  @for (word of words(); track word) {\r\n    <div class="word-card" (click)="wordClick.emit(word)">\r\n      {{ word | truncate: 25 }}\r\n    </div>\r\n  }\r\n</div>\r\n', styles: ["/* src/app/features/dictionary/components/word-grid.component.scss */\n.word-grid {\n  display: grid;\n  grid-template-columns: repeat(3, 1fr);\n  gap: 0.5rem;\n}\n.word-card {\n  padding: 0.5rem 0.75rem;\n  background: white;\n  border: 1px solid #e5e7eb;\n  border-radius: 6px;\n  cursor: pointer;\n  text-align: center;\n  transition: background 0.15s, border-color 0.15s;\n}\n.word-card:hover {\n  background: #eff6ff;\n  border-color: #3b82f6;\n}\n@media (max-width: 640px) {\n  .word-grid {\n    grid-template-columns: repeat(2, 1fr);\n  }\n}\n/*# sourceMappingURL=word-grid.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(WordGridComponent, { className: "WordGridComponent", filePath: "src/app/features/dictionary/components/word-grid.component.ts", lineNumber: 11 });
})();

// src/app/shared/components/pagination/pagination.component.ts
var PaginationComponent = class _PaginationComponent {
  currentPage = input.required();
  totalCount = input.required();
  pageSize = input(90);
  pageChange = output();
  totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()));
  static \u0275fac = function PaginationComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _PaginationComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _PaginationComponent, selectors: [["app-pagination"]], inputs: { currentPage: [1, "currentPage"], totalCount: [1, "totalCount"], pageSize: [1, "pageSize"] }, outputs: { pageChange: "pageChange" }, decls: 7, vars: 4, consts: [[1, "pagination"], [1, "page-btn", 3, "click", "disabled"], [1, "page-info"]], template: function PaginationComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "nav", 0)(1, "button", 1);
      \u0275\u0275listener("click", function PaginationComponent_Template_button_click_1_listener() {
        return ctx.pageChange.emit(ctx.currentPage() - 1);
      });
      \u0275\u0275text(2, " \xAB Previous ");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "span", 2);
      \u0275\u0275text(4);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(5, "button", 1);
      \u0275\u0275listener("click", function PaginationComponent_Template_button_click_5_listener() {
        return ctx.pageChange.emit(ctx.currentPage() + 1);
      });
      \u0275\u0275text(6, " Next \xBB ");
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance();
      \u0275\u0275property("disabled", ctx.currentPage() <= 1);
      \u0275\u0275advance(3);
      \u0275\u0275textInterpolate2("Page ", ctx.currentPage(), " of ", ctx.totalPages(), "");
      \u0275\u0275advance();
      \u0275\u0275property("disabled", ctx.currentPage() >= ctx.totalPages());
    }
  }, styles: ["\n\n.pagination[_ngcontent-%COMP%] {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  gap: 1rem;\n  padding: 1rem 0;\n}\n.page-btn[_ngcontent-%COMP%] {\n  padding: 0.5rem 1rem;\n  background: #3b82f6;\n  color: white;\n  border: none;\n  border-radius: 4px;\n  cursor: pointer;\n}\n.page-btn[_ngcontent-%COMP%]:hover:not(:disabled) {\n  background: #2563eb;\n}\n.page-btn[_ngcontent-%COMP%]:disabled {\n  background: #d1d5db;\n  cursor: not-allowed;\n}\n.page-info[_ngcontent-%COMP%] {\n  color: #4b5563;\n}\n/*# sourceMappingURL=pagination.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(PaginationComponent, [{
    type: Component,
    args: [{ selector: "app-pagination", standalone: true, template: '<nav class="pagination">\r\n  <button\r\n    class="page-btn"\r\n    [disabled]="currentPage() <= 1"\r\n    (click)="pageChange.emit(currentPage() - 1)"\r\n  >\r\n    &laquo; Previous\r\n  </button>\r\n  <span class="page-info">Page {{ currentPage() }} of {{ totalPages() }}</span>\r\n  <button\r\n    class="page-btn"\r\n    [disabled]="currentPage() >= totalPages()"\r\n    (click)="pageChange.emit(currentPage() + 1)"\r\n  >\r\n    Next &raquo;\r\n  </button>\r\n</nav>\r\n', styles: ["/* src/app/shared/components/pagination/pagination.component.scss */\n.pagination {\n  display: flex;\n  align-items: center;\n  justify-content: center;\n  gap: 1rem;\n  padding: 1rem 0;\n}\n.page-btn {\n  padding: 0.5rem 1rem;\n  background: #3b82f6;\n  color: white;\n  border: none;\n  border-radius: 4px;\n  cursor: pointer;\n}\n.page-btn:hover:not(:disabled) {\n  background: #2563eb;\n}\n.page-btn:disabled {\n  background: #d1d5db;\n  cursor: not-allowed;\n}\n.page-info {\n  color: #4b5563;\n}\n/*# sourceMappingURL=pagination.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(PaginationComponent, { className: "PaginationComponent", filePath: "src/app/shared/components/pagination/pagination.component.ts", lineNumber: 9 });
})();

// src/app/features/dictionary/containers/dictionary-page.component.ts
function DictionaryPageComponent_Case_2_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function DictionaryPageComponent_Case_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function DictionaryPageComponent_Case_4_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-empty-state", 2);
  }
}
function DictionaryPageComponent_Case_4_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-word-grid", 3);
    \u0275\u0275listener("wordClick", function DictionaryPageComponent_Case_4_Conditional_1_Template_app_word_grid_wordClick_0_listener($event) {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext(2);
      return \u0275\u0275resetView(ctx_r1.onWordClick($event));
    });
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(1, "app-pagination", 4);
    \u0275\u0275listener("pageChange", function DictionaryPageComponent_Case_4_Conditional_1_Template_app_pagination_pageChange_1_listener($event) {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext(2);
      return \u0275\u0275resetView(ctx_r1.onPageChange($event));
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext(2);
    \u0275\u0275property("words", ctx_r1.loadedData().words);
    \u0275\u0275advance();
    \u0275\u0275property("currentPage", ctx_r1.currentPage())("totalCount", ctx_r1.loadedData().totalCount)("pageSize", 90);
  }
}
function DictionaryPageComponent_Case_4_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275template(0, DictionaryPageComponent_Case_4_Conditional_0_Template, 1, 0, "app-empty-state", 2)(1, DictionaryPageComponent_Case_4_Conditional_1_Template, 2, 4);
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275conditional(ctx_r1.loadedData().words.length === 0 ? 0 : 1);
  }
}
function DictionaryPageComponent_Case_5_Template(rf, ctx) {
  if (rf & 1) {
    const _r3 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-error-display", 5);
    \u0275\u0275listener("retry", function DictionaryPageComponent_Case_5_Template_app_error_display_retry_0_listener() {
      \u0275\u0275restoreView(_r3);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.loadPage());
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275property("message", ctx_r1.errorMsg());
  }
}
var DictionaryPageComponent = class _DictionaryPageComponent {
  dictSvc = inject(DictionaryService);
  _router = inject(Router);
  currentPage = signal(1);
  loadedData = () => {
    const s = this.dictSvc.state();
    return s.stage === "loaded" ? s.data : { words: [], totalCount: 0, page: 1, pageSize: 90 };
  };
  errorMsg = () => {
    const s = this.dictSvc.state();
    return s.stage === "error" ? s.error : "";
  };
  ngOnInit() {
    this.loadPage();
  }
  loadPage() {
    this.dictSvc.loadPage(this.currentPage());
  }
  onPageChange(page) {
    this.currentPage.set(page);
    this.dictSvc.loadPage(page);
  }
  onWordClick(word) {
    this._router.navigate(["/search"], { queryParams: { word } });
  }
  static \u0275fac = function DictionaryPageComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _DictionaryPageComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _DictionaryPageComponent, selectors: [["app-dictionary-page"]], decls: 6, vars: 1, consts: [[1, "page-title"], [3, "message"], ["message", "Dictionary is empty"], [3, "wordClick", "words"], [3, "pageChange", "currentPage", "totalCount", "pageSize"], [3, "retry", "message"]], template: function DictionaryPageComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "h1", 0);
      \u0275\u0275text(1, "Dictionary");
      \u0275\u0275elementEnd();
      \u0275\u0275template(2, DictionaryPageComponent_Case_2_Template, 1, 0, "app-loading-spinner")(3, DictionaryPageComponent_Case_3_Template, 1, 0, "app-loading-spinner")(4, DictionaryPageComponent_Case_4_Template, 2, 1)(5, DictionaryPageComponent_Case_5_Template, 1, 1, "app-error-display", 1);
    }
    if (rf & 2) {
      let tmp_0_0;
      \u0275\u0275advance(2);
      \u0275\u0275conditional((tmp_0_0 = ctx.dictSvc.state().stage) === "idle" ? 2 : tmp_0_0 === "loading" ? 3 : tmp_0_0 === "loaded" ? 4 : tmp_0_0 === "error" ? 5 : -1);
    }
  }, dependencies: [
    WordGridComponent,
    PaginationComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent
  ], encapsulation: 2 });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(DictionaryPageComponent, [{
    type: Component,
    args: [{ selector: "app-dictionary-page", standalone: true, imports: [
      WordGridComponent,
      PaginationComponent,
      LoadingSpinnerComponent,
      ErrorDisplayComponent,
      EmptyStateComponent
    ], template: `<h1 class="page-title">Dictionary</h1>\r
\r
@switch (dictSvc.state().stage) {\r
  @case ('idle') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loading') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loaded') {\r
    @if (loadedData().words.length === 0) {\r
      <app-empty-state message="Dictionary is empty" />\r
    } @else {\r
      <app-word-grid\r
        [words]="loadedData().words"\r
        (wordClick)="onWordClick($event)"\r
      />\r
      <app-pagination\r
        [currentPage]="currentPage()"\r
        [totalCount]="loadedData().totalCount"\r
        [pageSize]="90"\r
        (pageChange)="onPageChange($event)"\r
      />\r
    }\r
  }\r
  @case ('error') {\r
    <app-error-display [message]="errorMsg()" (retry)="loadPage()" />\r
  }\r
}\r
` }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(DictionaryPageComponent, { className: "DictionaryPageComponent", filePath: "src/app/features/dictionary/containers/dictionary-page.component.ts", lineNumber: 22 });
})();
export {
  DictionaryPageComponent
};
//# sourceMappingURL=chunk-F5KPLGTY.js.map
