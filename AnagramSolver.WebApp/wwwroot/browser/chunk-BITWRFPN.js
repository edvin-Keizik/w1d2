import {
  SettingsService
} from "./chunk-PDFJKIGW.js";
import {
  DomSanitizer
} from "./chunk-YPJFZ374.js";
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
  EmptyStateComponent
} from "./chunk-PN7V6CON.js";
import {
  ErrorDisplayComponent,
  LoadingSpinnerComponent
} from "./chunk-OTOFTWZT.js";
import {
  TimeAgoPipe
} from "./chunk-K5NYSZAZ.js";
import {
  ApiService,
  Component,
  Injectable,
  Pipe,
  inject,
  input,
  output,
  setClassMetadata,
  signal,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵconditional,
  ɵɵdefineComponent,
  ɵɵdefineInjectable,
  ɵɵdefinePipe,
  ɵɵdirectiveInject,
  ɵɵelement,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵgetCurrentView,
  ɵɵlistener,
  ɵɵnextContext,
  ɵɵpipe,
  ɵɵpipeBind1,
  ɵɵpipeBind2,
  ɵɵproperty,
  ɵɵrepeater,
  ɵɵrepeaterCreate,
  ɵɵrepeaterTrackByIdentity,
  ɵɵrepeaterTrackByIndex,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵsanitizeHtml,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate,
  ɵɵtextInterpolate1,
  ɵɵtextInterpolate2
} from "./chunk-5B3AD7EP.js";

// src/app/core/services/anagram.service.ts
var HISTORY_KEY = "anagram_search_history";
var AnagramService = class _AnagramService {
  _api = inject(ApiService);
  state = signal({ stage: "idle" });
  searchHistory = signal(this._loadHistory());
  search(word) {
    this.state.set({ stage: "loading" });
    this._api.get(`anagrams/${encodeURIComponent(word)}`).subscribe({
      next: (results) => {
        this.state.set({ stage: "loaded", data: results });
        this._addToHistory(word, results.length);
      },
      error: (err) => {
        this.state.set({ stage: "error", error: err.message ?? "Search failed" });
      }
    });
  }
  reset() {
    this.state.set({ stage: "idle" });
  }
  clearHistory() {
    localStorage.removeItem(HISTORY_KEY);
    this.searchHistory.set([]);
  }
  _addToHistory(word, resultCount) {
    const item = {
      word,
      searchedAt: (/* @__PURE__ */ new Date()).toISOString(),
      resultCount
    };
    const history = [item, ...this.searchHistory()].slice(0, 50);
    this.searchHistory.set(history);
    localStorage.setItem(HISTORY_KEY, JSON.stringify(history));
  }
  _loadHistory() {
    const raw = localStorage.getItem(HISTORY_KEY);
    if (!raw)
      return [];
    try {
      return JSON.parse(raw);
    } catch {
      return [];
    }
  }
  static \u0275fac = function AnagramService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AnagramService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _AnagramService, factory: _AnagramService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AnagramService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

// src/app/features/anagram-search/components/search-form.component.ts
function SearchFormComponent_Conditional_5_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 5);
    \u0275\u0275text(1, "Word is required");
    \u0275\u0275elementEnd();
  }
}
function SearchFormComponent_Conditional_5_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 5);
    \u0275\u0275text(1);
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext(2);
    \u0275\u0275advance();
    \u0275\u0275textInterpolate1("Minimum ", ctx_r0.minLength(), " characters required");
  }
}
function SearchFormComponent_Conditional_5_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275template(0, SearchFormComponent_Conditional_5_Conditional_0_Template, 2, 0, "div", 5)(1, SearchFormComponent_Conditional_5_Conditional_1_Template, 2, 1, "div", 5);
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext();
    \u0275\u0275conditional(ctx_r0.wordControl.hasError("required") ? 0 : -1);
    \u0275\u0275advance();
    \u0275\u0275conditional(ctx_r0.wordControl.hasError("minlength") ? 1 : -1);
  }
}
function SearchFormComponent_Conditional_7_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Searching... ");
  }
}
function SearchFormComponent_Conditional_8_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Search Anagrams ");
  }
}
var SearchFormComponent = class _SearchFormComponent {
  minLength = input(3);
  isLoading = input(false);
  search = output();
  wordControl = new FormControl("", {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(3)]
  });
  constructor() {
  }
  ngOnInit() {
    const ml = this.minLength();
    if (ml !== 3) {
      this.wordControl.setValidators([Validators.required, Validators.minLength(ml)]);
      this.wordControl.updateValueAndValidity();
    }
  }
  onSubmit() {
    if (this.wordControl.valid) {
      this.search.emit(this.wordControl.value.trim());
    }
  }
  static \u0275fac = function SearchFormComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _SearchFormComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _SearchFormComponent, selectors: [["app-search-form"]], inputs: { minLength: [1, "minLength"], isLoading: [1, "isLoading"] }, outputs: { search: "search" }, decls: 9, vars: 4, consts: [[1, "card", "search-form", 3, "ngSubmit"], [1, "form-group"], ["for", "searchWord"], ["id", "searchWord", "type", "text", "placeholder", "Type a word...", "autocomplete", "off", 3, "formControl"], ["type", "submit", 1, "btn", "btn-primary", 3, "disabled"], [1, "error-msg"]], template: function SearchFormComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "form", 0);
      \u0275\u0275listener("ngSubmit", function SearchFormComponent_Template_form_ngSubmit_0_listener() {
        return ctx.onSubmit();
      });
      \u0275\u0275elementStart(1, "div", 1)(2, "label", 2);
      \u0275\u0275text(3, "Enter a word to find anagrams");
      \u0275\u0275elementEnd();
      \u0275\u0275element(4, "input", 3);
      \u0275\u0275template(5, SearchFormComponent_Conditional_5_Template, 2, 2);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(6, "button", 4);
      \u0275\u0275template(7, SearchFormComponent_Conditional_7_Template, 1, 0)(8, SearchFormComponent_Conditional_8_Template, 1, 0);
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance(4);
      \u0275\u0275property("formControl", ctx.wordControl);
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.wordControl.invalid && ctx.wordControl.touched ? 5 : -1);
      \u0275\u0275advance();
      \u0275\u0275property("disabled", ctx.wordControl.invalid || ctx.isLoading());
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.isLoading() ? 7 : 8);
    }
  }, dependencies: [ReactiveFormsModule, \u0275NgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, FormControlDirective], styles: ["\n\n.search-form[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n/*# sourceMappingURL=search-form.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(SearchFormComponent, [{
    type: Component,
    args: [{ selector: "app-search-form", standalone: true, imports: [ReactiveFormsModule], template: `<form class="card search-form" (ngSubmit)="onSubmit()">\r
  <div class="form-group">\r
    <label for="searchWord">Enter a word to find anagrams</label>\r
    <input\r
      id="searchWord"\r
      type="text"\r
      [formControl]="wordControl"\r
      placeholder="Type a word..."\r
      autocomplete="off"\r
    />\r
    @if (wordControl.invalid && wordControl.touched) {\r
      @if (wordControl.hasError('required')) {\r
        <div class="error-msg">Word is required</div>\r
      }\r
      @if (wordControl.hasError('minlength')) {\r
        <div class="error-msg">Minimum {{ minLength() }} characters required</div>\r
      }\r
    }\r
  </div>\r
  <button\r
    type="submit"\r
    class="btn btn-primary"\r
    [disabled]="wordControl.invalid || isLoading()"\r
  >\r
    @if (isLoading()) {\r
      Searching...\r
    } @else {\r
      Search Anagrams\r
    }\r
  </button>\r
</form>\r
`, styles: ["/* src/app/features/anagram-search/components/search-form.component.scss */\n.search-form {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n/*# sourceMappingURL=search-form.component.css.map */\n"] }]
  }], () => [], null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(SearchFormComponent, { className: "SearchFormComponent", filePath: "src/app/features/anagram-search/components/search-form.component.ts", lineNumber: 11 });
})();

// src/app/shared/pipes/highlight.pipe.ts
var HighlightPipe = class _HighlightPipe {
  _sanitizer;
  constructor(_sanitizer) {
    this._sanitizer = _sanitizer;
  }
  transform(text, search) {
    if (!search || !text)
      return text;
    const escaped = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
    const regex = new RegExp(`(${escaped})`, "gi");
    const highlighted = text.replace(regex, "<mark>$1</mark>");
    return this._sanitizer.bypassSecurityTrustHtml(highlighted);
  }
  static \u0275fac = function HighlightPipe_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _HighlightPipe)(\u0275\u0275directiveInject(DomSanitizer, 16));
  };
  static \u0275pipe = /* @__PURE__ */ \u0275\u0275definePipe({ name: "highlight", type: _HighlightPipe, pure: true });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(HighlightPipe, [{
    type: Pipe,
    args: [{ name: "highlight", standalone: true }]
  }], () => [{ type: DomSanitizer }], null);
})();

// src/app/features/anagram-search/components/search-results.component.ts
function SearchResultsComponent_For_5_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "li", 2);
    \u0275\u0275pipe(1, "highlight");
  }
  if (rf & 2) {
    const word_r1 = ctx.$implicit;
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275property("innerHTML", \u0275\u0275pipeBind2(1, 1, word_r1, ctx_r1.searchTerm()), \u0275\u0275sanitizeHtml);
  }
}
var SearchResultsComponent = class _SearchResultsComponent {
  results = input.required();
  searchTerm = input("");
  static \u0275fac = function SearchResultsComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _SearchResultsComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _SearchResultsComponent, selectors: [["app-search-results"]], inputs: { results: [1, "results"], searchTerm: [1, "searchTerm"] }, decls: 6, vars: 1, consts: [[1, "results", "card"], [1, "result-list"], [3, "innerHTML"]], template: function SearchResultsComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0)(1, "h3");
      \u0275\u0275text(2);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "ul", 1);
      \u0275\u0275repeaterCreate(4, SearchResultsComponent_For_5_Template, 2, 4, "li", 2, \u0275\u0275repeaterTrackByIdentity);
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance(2);
      \u0275\u0275textInterpolate1("Results (", ctx.results().length, ")");
      \u0275\u0275advance(2);
      \u0275\u0275repeater(ctx.results());
    }
  }, dependencies: [HighlightPipe], styles: ["\n\n.results[_ngcontent-%COMP%]   h3[_ngcontent-%COMP%] {\n  margin-bottom: 0.75rem;\n}\n.result-list[_ngcontent-%COMP%] {\n  list-style: none;\n  display: flex;\n  flex-wrap: wrap;\n  gap: 0.5rem;\n}\n.result-list[_ngcontent-%COMP%]   li[_ngcontent-%COMP%] {\n  padding: 0.375rem 0.75rem;\n  background: #eff6ff;\n  border: 1px solid #bfdbfe;\n  border-radius: 6px;\n  font-size: 0.9rem;\n}\n/*# sourceMappingURL=search-results.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(SearchResultsComponent, [{
    type: Component,
    args: [{ selector: "app-search-results", standalone: true, imports: [HighlightPipe], template: '<div class="results card">\r\n  <h3>Results ({{ results().length }})</h3>\r\n  <ul class="result-list">\r\n    @for (word of results(); track word) {\r\n      <li [innerHTML]="word | highlight: searchTerm()"></li>\r\n    }\r\n  </ul>\r\n</div>\r\n', styles: ["/* src/app/features/anagram-search/components/search-results.component.scss */\n.results h3 {\n  margin-bottom: 0.75rem;\n}\n.result-list {\n  list-style: none;\n  display: flex;\n  flex-wrap: wrap;\n  gap: 0.5rem;\n}\n.result-list li {\n  padding: 0.375rem 0.75rem;\n  background: #eff6ff;\n  border: 1px solid #bfdbfe;\n  border-radius: 6px;\n  font-size: 0.9rem;\n}\n/*# sourceMappingURL=search-results.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(SearchResultsComponent, { className: "SearchResultsComponent", filePath: "src/app/features/anagram-search/components/search-results.component.ts", lineNumber: 11 });
})();

// src/app/features/anagram-search/components/search-history.component.ts
function SearchHistoryComponent_Conditional_0_For_8_Template(rf, ctx) {
  if (rf & 1) {
    const _r3 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "li", 4);
    \u0275\u0275listener("click", function SearchHistoryComponent_Conditional_0_For_8_Template_li_click_0_listener() {
      const item_r4 = \u0275\u0275restoreView(_r3).$implicit;
      const ctx_r1 = \u0275\u0275nextContext(2);
      return \u0275\u0275resetView(ctx_r1.selectWord.emit(item_r4.word));
    });
    \u0275\u0275elementStart(1, "span", 5);
    \u0275\u0275text(2);
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(3, "span", 6);
    \u0275\u0275text(4);
    \u0275\u0275pipe(5, "timeAgo");
    \u0275\u0275elementEnd()();
  }
  if (rf & 2) {
    const item_r4 = ctx.$implicit;
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(item_r4.word);
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate2("", item_r4.resultCount, " results \xB7 ", \u0275\u0275pipeBind1(5, 3, item_r4.searchedAt), "");
  }
}
function SearchHistoryComponent_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "div", 0)(1, "div", 1)(2, "h3");
    \u0275\u0275text(3, "Search History");
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(4, "button", 2);
    \u0275\u0275listener("click", function SearchHistoryComponent_Conditional_0_Template_button_click_4_listener() {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.clearHistory.emit());
    });
    \u0275\u0275text(5, "Clear");
    \u0275\u0275elementEnd()();
    \u0275\u0275elementStart(6, "ul", 3);
    \u0275\u0275repeaterCreate(7, SearchHistoryComponent_Conditional_0_For_8_Template, 6, 5, "li", null, \u0275\u0275repeaterTrackByIndex);
    \u0275\u0275elementEnd()();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275advance(7);
    \u0275\u0275repeater(ctx_r1.history());
  }
}
var SearchHistoryComponent = class _SearchHistoryComponent {
  history = input.required();
  selectWord = output();
  clearHistory = output();
  static \u0275fac = function SearchHistoryComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _SearchHistoryComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _SearchHistoryComponent, selectors: [["app-search-history"]], inputs: { history: [1, "history"] }, outputs: { selectWord: "selectWord", clearHistory: "clearHistory" }, decls: 1, vars: 1, consts: [[1, "card", "history"], [1, "history-header"], [1, "btn", "btn-secondary", "btn-sm", 3, "click"], [1, "history-list"], [3, "click"], [1, "word"], [1, "meta"]], template: function SearchHistoryComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275template(0, SearchHistoryComponent_Conditional_0_Template, 9, 0, "div", 0);
    }
    if (rf & 2) {
      \u0275\u0275conditional(ctx.history().length > 0 ? 0 : -1);
    }
  }, dependencies: [TimeAgoPipe], styles: ["\n\n.history-header[_ngcontent-%COMP%] {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  margin-bottom: 0.75rem;\n}\n.btn-sm[_ngcontent-%COMP%] {\n  padding: 0.25rem 0.5rem;\n  font-size: 0.75rem;\n}\n.history-list[_ngcontent-%COMP%] {\n  list-style: none;\n}\n.history-list[_ngcontent-%COMP%]   li[_ngcontent-%COMP%] {\n  display: flex;\n  justify-content: space-between;\n  padding: 0.5rem 0;\n  border-bottom: 1px solid #f3f4f6;\n  cursor: pointer;\n}\n.history-list[_ngcontent-%COMP%]   li[_ngcontent-%COMP%]:hover {\n  background: #f9fafb;\n}\n.word[_ngcontent-%COMP%] {\n  font-weight: 600;\n}\n.meta[_ngcontent-%COMP%] {\n  color: #9ca3af;\n  font-size: 0.8rem;\n}\n/*# sourceMappingURL=search-history.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(SearchHistoryComponent, [{
    type: Component,
    args: [{ selector: "app-search-history", standalone: true, imports: [TimeAgoPipe], template: '@if (history().length > 0) {\r\n  <div class="card history">\r\n    <div class="history-header">\r\n      <h3>Search History</h3>\r\n      <button class="btn btn-secondary btn-sm" (click)="clearHistory.emit()">Clear</button>\r\n    </div>\r\n    <ul class="history-list">\r\n      @for (item of history(); track $index) {\r\n        <li (click)="selectWord.emit(item.word)">\r\n          <span class="word">{{ item.word }}</span>\r\n          <span class="meta">{{ item.resultCount }} results &middot; {{ item.searchedAt | timeAgo }}</span>\r\n        </li>\r\n      }\r\n    </ul>\r\n  </div>\r\n}\r\n', styles: ["/* src/app/features/anagram-search/components/search-history.component.scss */\n.history-header {\n  display: flex;\n  justify-content: space-between;\n  align-items: center;\n  margin-bottom: 0.75rem;\n}\n.btn-sm {\n  padding: 0.25rem 0.5rem;\n  font-size: 0.75rem;\n}\n.history-list {\n  list-style: none;\n}\n.history-list li {\n  display: flex;\n  justify-content: space-between;\n  padding: 0.5rem 0;\n  border-bottom: 1px solid #f3f4f6;\n  cursor: pointer;\n}\n.history-list li:hover {\n  background: #f9fafb;\n}\n.word {\n  font-weight: 600;\n}\n.meta {\n  color: #9ca3af;\n  font-size: 0.8rem;\n}\n/*# sourceMappingURL=search-history.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(SearchHistoryComponent, { className: "SearchHistoryComponent", filePath: "src/app/features/anagram-search/components/search-history.component.ts", lineNumber: 12 });
})();

// src/app/features/anagram-search/containers/anagram-search-page.component.ts
function AnagramSearchPageComponent_Case_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-empty-state", 2);
  }
}
function AnagramSearchPageComponent_Case_4_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function AnagramSearchPageComponent_Case_5_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-empty-state", 5);
  }
}
function AnagramSearchPageComponent_Case_5_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-search-results", 6);
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext(2);
    \u0275\u0275property("results", ctx_r0.loadedData())("searchTerm", ctx_r0.lastSearch());
  }
}
function AnagramSearchPageComponent_Case_5_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275template(0, AnagramSearchPageComponent_Case_5_Conditional_0_Template, 1, 0, "app-empty-state", 5)(1, AnagramSearchPageComponent_Case_5_Conditional_1_Template, 1, 2, "app-search-results", 6);
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext();
    \u0275\u0275conditional(ctx_r0.loadedData().length === 0 ? 0 : 1);
  }
}
function AnagramSearchPageComponent_Case_6_Template(rf, ctx) {
  if (rf & 1) {
    const _r2 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-error-display", 7);
    \u0275\u0275listener("retry", function AnagramSearchPageComponent_Case_6_Template_app_error_display_retry_0_listener() {
      \u0275\u0275restoreView(_r2);
      const ctx_r0 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r0.onRetry());
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext();
    \u0275\u0275property("message", ctx_r0.errorMsg());
  }
}
var AnagramSearchPageComponent = class _AnagramSearchPageComponent {
  anagramSvc = inject(AnagramService);
  settings = inject(SettingsService);
  lastSearch = signal("");
  loadedData = () => {
    const s = this.anagramSvc.state();
    return s.stage === "loaded" ? s.data : [];
  };
  errorMsg = () => {
    const s = this.anagramSvc.state();
    return s.stage === "error" ? s.error : "";
  };
  onSearch(word) {
    this.lastSearch.set(word);
    this.anagramSvc.search(word);
  }
  onRetry() {
    const word = this.lastSearch();
    if (word)
      this.anagramSvc.search(word);
  }
  static \u0275fac = function AnagramSearchPageComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AnagramSearchPageComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _AnagramSearchPageComponent, selectors: [["app-anagram-search-page"]], decls: 8, vars: 4, consts: [[1, "page-title"], [3, "search", "minLength", "isLoading"], ["message", "Enter a word above to search for anagrams"], [3, "message"], [3, "selectWord", "clearHistory", "history"], ["message", "No anagrams found for this word"], [3, "results", "searchTerm"], [3, "retry", "message"]], template: function AnagramSearchPageComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "h1", 0);
      \u0275\u0275text(1, "Anagram Search");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(2, "app-search-form", 1);
      \u0275\u0275listener("search", function AnagramSearchPageComponent_Template_app_search_form_search_2_listener($event) {
        return ctx.onSearch($event);
      });
      \u0275\u0275elementEnd();
      \u0275\u0275template(3, AnagramSearchPageComponent_Case_3_Template, 1, 0, "app-empty-state", 2)(4, AnagramSearchPageComponent_Case_4_Template, 1, 0, "app-loading-spinner")(5, AnagramSearchPageComponent_Case_5_Template, 2, 1)(6, AnagramSearchPageComponent_Case_6_Template, 1, 1, "app-error-display", 3);
      \u0275\u0275elementStart(7, "app-search-history", 4);
      \u0275\u0275listener("selectWord", function AnagramSearchPageComponent_Template_app_search_history_selectWord_7_listener($event) {
        return ctx.onSearch($event);
      })("clearHistory", function AnagramSearchPageComponent_Template_app_search_history_clearHistory_7_listener() {
        return ctx.anagramSvc.clearHistory();
      });
      \u0275\u0275elementEnd();
    }
    if (rf & 2) {
      let tmp_0_0;
      let tmp_2_0;
      \u0275\u0275advance(2);
      \u0275\u0275property("minLength", (tmp_0_0 = (tmp_0_0 = ctx.settings.settings()) == null ? null : tmp_0_0.minWordLength) !== null && tmp_0_0 !== void 0 ? tmp_0_0 : 3)("isLoading", ctx.anagramSvc.state().stage === "loading");
      \u0275\u0275advance();
      \u0275\u0275conditional((tmp_2_0 = ctx.anagramSvc.state().stage) === "idle" ? 3 : tmp_2_0 === "loading" ? 4 : tmp_2_0 === "loaded" ? 5 : tmp_2_0 === "error" ? 6 : -1);
      \u0275\u0275advance(4);
      \u0275\u0275property("history", ctx.anagramSvc.searchHistory());
    }
  }, dependencies: [
    SearchFormComponent,
    SearchResultsComponent,
    SearchHistoryComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent
  ], encapsulation: 2 });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AnagramSearchPageComponent, [{
    type: Component,
    args: [{ selector: "app-anagram-search-page", standalone: true, imports: [
      SearchFormComponent,
      SearchResultsComponent,
      SearchHistoryComponent,
      LoadingSpinnerComponent,
      ErrorDisplayComponent,
      EmptyStateComponent
    ], template: `<h1 class="page-title">Anagram Search</h1>\r
\r
<app-search-form\r
  [minLength]="settings.settings()?.minWordLength ?? 3"\r
  [isLoading]="anagramSvc.state().stage === 'loading'"\r
  (search)="onSearch($event)"\r
/>\r
\r
@switch (anagramSvc.state().stage) {\r
  @case ('idle') {\r
    <app-empty-state message="Enter a word above to search for anagrams" />\r
  }\r
  @case ('loading') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loaded') {\r
    @if (loadedData().length === 0) {\r
      <app-empty-state message="No anagrams found for this word" />\r
    } @else {\r
      <app-search-results [results]="loadedData()" [searchTerm]="lastSearch()" />\r
    }\r
  }\r
  @case ('error') {\r
    <app-error-display [message]="errorMsg()" (retry)="onRetry()" />\r
  }\r
}\r
\r
<app-search-history\r
  [history]="anagramSvc.searchHistory()"\r
  (selectWord)="onSearch($event)"\r
  (clearHistory)="anagramSvc.clearHistory()"\r
/>\r
` }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(AnagramSearchPageComponent, { className: "AnagramSearchPageComponent", filePath: "src/app/features/anagram-search/containers/anagram-search-page.component.ts", lineNumber: 24 });
})();
export {
  AnagramSearchPageComponent
};
//# sourceMappingURL=chunk-BITWRFPN.js.map
