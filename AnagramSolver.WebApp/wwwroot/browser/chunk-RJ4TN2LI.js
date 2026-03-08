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
  ApiService,
  Component,
  Injectable,
  Pipe,
  ViewChild,
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
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate,
  ɵɵtextInterpolate1,
  ɵɵviewQuery
} from "./chunk-5B3AD7EP.js";

// src/app/core/services/analysis.service.ts
var AnalysisService = class _AnalysisService {
  _api = inject(ApiService);
  state = signal({ stage: "idle" });
  analyze(text) {
    this.state.set({ stage: "loading" });
    this._api.post("v1/analysis/frequency", { text }).subscribe({
      next: (data) => this.state.set({ stage: "loaded", data }),
      error: (err) => this.state.set({ stage: "error", error: err.message ?? "Analysis failed" })
    });
  }
  reset() {
    this.state.set({ stage: "idle" });
  }
  static \u0275fac = function AnalysisService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AnalysisService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _AnalysisService, factory: _AnalysisService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AnalysisService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

// src/app/shared/pipes/word-count.pipe.ts
var WordCountPipe = class _WordCountPipe {
  transform(value) {
    if (!value || !value.trim())
      return 0;
    return value.trim().split(/\s+/).length;
  }
  static \u0275fac = function WordCountPipe_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _WordCountPipe)();
  };
  static \u0275pipe = /* @__PURE__ */ \u0275\u0275definePipe({ name: "wordCount", type: _WordCountPipe, pure: true });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(WordCountPipe, [{
    type: Pipe,
    args: [{ name: "wordCount", standalone: true }]
  }], null, null);
})();

// src/app/features/analysis/components/analysis-form.component.ts
function AnalysisFormComponent_Conditional_11_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 6);
    \u0275\u0275text(1, "Text is required");
    \u0275\u0275elementEnd();
  }
}
function AnalysisFormComponent_Conditional_11_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 6);
    \u0275\u0275text(1, "Maximum 100,000 characters allowed");
    \u0275\u0275elementEnd();
  }
}
function AnalysisFormComponent_Conditional_11_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275template(0, AnalysisFormComponent_Conditional_11_Conditional_0_Template, 2, 0, "div", 6)(1, AnalysisFormComponent_Conditional_11_Conditional_1_Template, 2, 0, "div", 6);
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext();
    \u0275\u0275conditional(ctx_r0.textControl.hasError("required") ? 0 : -1);
    \u0275\u0275advance();
    \u0275\u0275conditional(ctx_r0.textControl.hasError("maxlength") ? 1 : -1);
  }
}
function AnalysisFormComponent_Conditional_13_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Analyzing... ");
  }
}
function AnalysisFormComponent_Conditional_14_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Analyze Frequency ");
  }
}
var AnalysisFormComponent = class _AnalysisFormComponent {
  isLoading = input(false);
  analyze = output();
  textControl = new FormControl("", {
    nonNullable: true,
    validators: [Validators.required, Validators.maxLength(1e5)]
  });
  isDirty() {
    return this.textControl.dirty;
  }
  onSubmit() {
    if (this.textControl.valid) {
      this.analyze.emit(this.textControl.value);
    }
  }
  static \u0275fac = function AnalysisFormComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AnalysisFormComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _AnalysisFormComponent, selectors: [["app-analysis-form"]], inputs: { isLoading: [1, "isLoading"] }, outputs: { analyze: "analyze" }, decls: 15, vars: 8, consts: [[1, "card", "analysis-form", 3, "ngSubmit"], [1, "form-group"], ["for", "analysisText"], ["id", "analysisText", "rows", "6", "placeholder", "Paste or type text here for frequency analysis...", 3, "formControl"], [1, "form-meta"], ["type", "submit", 1, "btn", "btn-primary", 3, "disabled"], [1, "error-msg"]], template: function AnalysisFormComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "form", 0);
      \u0275\u0275listener("ngSubmit", function AnalysisFormComponent_Template_form_ngSubmit_0_listener() {
        return ctx.onSubmit();
      });
      \u0275\u0275elementStart(1, "div", 1)(2, "label", 2);
      \u0275\u0275text(3, "Text to Analyze");
      \u0275\u0275elementEnd();
      \u0275\u0275element(4, "textarea", 3);
      \u0275\u0275elementStart(5, "div", 4)(6, "span");
      \u0275\u0275text(7);
      \u0275\u0275pipe(8, "wordCount");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(9, "span");
      \u0275\u0275text(10);
      \u0275\u0275elementEnd()();
      \u0275\u0275template(11, AnalysisFormComponent_Conditional_11_Template, 2, 2);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(12, "button", 5);
      \u0275\u0275template(13, AnalysisFormComponent_Conditional_13_Template, 1, 0)(14, AnalysisFormComponent_Conditional_14_Template, 1, 0);
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance(4);
      \u0275\u0275property("formControl", ctx.textControl);
      \u0275\u0275advance(3);
      \u0275\u0275textInterpolate1("", \u0275\u0275pipeBind1(8, 6, ctx.textControl.value), " words");
      \u0275\u0275advance(3);
      \u0275\u0275textInterpolate1("", ctx.textControl.value.length, " / 100,000 characters");
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.textControl.invalid && ctx.textControl.touched ? 11 : -1);
      \u0275\u0275advance();
      \u0275\u0275property("disabled", ctx.textControl.invalid || ctx.isLoading());
      \u0275\u0275advance();
      \u0275\u0275conditional(ctx.isLoading() ? 13 : 14);
    }
  }, dependencies: [ReactiveFormsModule, \u0275NgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, FormControlDirective, WordCountPipe], styles: ["\n\n.analysis-form[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n.form-meta[_ngcontent-%COMP%] {\n  display: flex;\n  justify-content: space-between;\n  font-size: 0.75rem;\n  color: #9ca3af;\n  margin-top: 0.25rem;\n}\n/*# sourceMappingURL=analysis-form.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AnalysisFormComponent, [{
    type: Component,
    args: [{ selector: "app-analysis-form", standalone: true, imports: [ReactiveFormsModule, WordCountPipe], template: `<form class="card analysis-form" (ngSubmit)="onSubmit()">\r
  <div class="form-group">\r
    <label for="analysisText">Text to Analyze</label>\r
    <textarea\r
      id="analysisText"\r
      [formControl]="textControl"\r
      rows="6"\r
      placeholder="Paste or type text here for frequency analysis..."\r
    ></textarea>\r
    <div class="form-meta">\r
      <span>{{ textControl.value | wordCount }} words</span>\r
      <span>{{ textControl.value.length }} / 100,000 characters</span>\r
    </div>\r
    @if (textControl.invalid && textControl.touched) {\r
      @if (textControl.hasError('required')) {\r
        <div class="error-msg">Text is required</div>\r
      }\r
      @if (textControl.hasError('maxlength')) {\r
        <div class="error-msg">Maximum 100,000 characters allowed</div>\r
      }\r
    }\r
  </div>\r
  <button\r
    type="submit"\r
    class="btn btn-primary"\r
    [disabled]="textControl.invalid || isLoading()"\r
  >\r
    @if (isLoading()) {\r
      Analyzing...\r
    } @else {\r
      Analyze Frequency\r
    }\r
  </button>\r
</form>\r
`, styles: ["/* src/app/features/analysis/components/analysis-form.component.scss */\n.analysis-form {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n.form-meta {\n  display: flex;\n  justify-content: space-between;\n  font-size: 0.75rem;\n  color: #9ca3af;\n  margin-top: 0.25rem;\n}\n/*# sourceMappingURL=analysis-form.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(AnalysisFormComponent, { className: "AnalysisFormComponent", filePath: "src/app/features/analysis/components/analysis-form.component.ts", lineNumber: 12 });
})();

// src/app/features/analysis/components/analysis-results.component.ts
var _forTrack0 = ($index, $item) => $item.word;
function AnalysisResultsComponent_For_32_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "tr")(1, "td");
    \u0275\u0275text(2);
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(3, "td");
    \u0275\u0275text(4);
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(5, "td");
    \u0275\u0275text(6);
    \u0275\u0275elementEnd()();
  }
  if (rf & 2) {
    const item_r1 = ctx.$implicit;
    const \u0275$index_52_r2 = ctx.$index;
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(\u0275$index_52_r2 + 1);
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(item_r1.word);
    \u0275\u0275advance(2);
    \u0275\u0275textInterpolate(item_r1.frequency);
  }
}
var AnalysisResultsComponent = class _AnalysisResultsComponent {
  data = input.required();
  static \u0275fac = function AnalysisResultsComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AnalysisResultsComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _AnalysisResultsComponent, selectors: [["app-analysis-results"]], inputs: { data: [1, "data"] }, decls: 33, vars: 3, consts: [[1, "card", "results"], [1, "stats-grid"], [1, "stat"], [1, "stat-value"], [1, "stat-label"], [1, "freq-table"]], template: function AnalysisResultsComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0)(1, "h3");
      \u0275\u0275text(2, "Analysis Results");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "div", 1)(4, "div", 2)(5, "span", 3);
      \u0275\u0275text(6);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(7, "span", 4);
      \u0275\u0275text(8, "Total Words");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(9, "div", 2)(10, "span", 3);
      \u0275\u0275text(11);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(12, "span", 4);
      \u0275\u0275text(13, "Unique Words");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(14, "div", 2)(15, "span", 3);
      \u0275\u0275text(16);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(17, "span", 4);
      \u0275\u0275text(18, "Longest Word");
      \u0275\u0275elementEnd()()();
      \u0275\u0275elementStart(19, "h4");
      \u0275\u0275text(20, "Top Words");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(21, "table", 5)(22, "thead")(23, "tr")(24, "th");
      \u0275\u0275text(25, "#");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(26, "th");
      \u0275\u0275text(27, "Word");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(28, "th");
      \u0275\u0275text(29, "Frequency");
      \u0275\u0275elementEnd()()();
      \u0275\u0275elementStart(30, "tbody");
      \u0275\u0275repeaterCreate(31, AnalysisResultsComponent_For_32_Template, 7, 3, "tr", null, _forTrack0);
      \u0275\u0275elementEnd()()();
    }
    if (rf & 2) {
      \u0275\u0275advance(6);
      \u0275\u0275textInterpolate(ctx.data().totalWordCount);
      \u0275\u0275advance(5);
      \u0275\u0275textInterpolate(ctx.data().uniqueWordCount);
      \u0275\u0275advance(5);
      \u0275\u0275textInterpolate(ctx.data().longestWord);
      \u0275\u0275advance(15);
      \u0275\u0275repeater(ctx.data().topWords);
    }
  }, styles: ["\n\n.stats-grid[_ngcontent-%COMP%] {\n  display: grid;\n  grid-template-columns: repeat(3, 1fr);\n  gap: 1rem;\n  margin: 1rem 0;\n}\n.stat[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 0.75rem;\n  background: #f9fafb;\n  border-radius: 6px;\n}\n.stat-value[_ngcontent-%COMP%] {\n  font-size: 1.5rem;\n  font-weight: 700;\n  color: #3b82f6;\n}\n.stat-label[_ngcontent-%COMP%] {\n  font-size: 0.75rem;\n  color: #6b7280;\n}\n.freq-table[_ngcontent-%COMP%] {\n  width: 100%;\n  border-collapse: collapse;\n  margin-top: 0.5rem;\n}\n.freq-table[_ngcontent-%COMP%]   th[_ngcontent-%COMP%], \n.freq-table[_ngcontent-%COMP%]   td[_ngcontent-%COMP%] {\n  padding: 0.5rem;\n  text-align: left;\n  border-bottom: 1px solid #e5e7eb;\n}\n.freq-table[_ngcontent-%COMP%]   th[_ngcontent-%COMP%] {\n  font-weight: 600;\n  color: #374151;\n}\n.freq-table[_ngcontent-%COMP%]   tr[_ngcontent-%COMP%]:hover   td[_ngcontent-%COMP%] {\n  background: #f9fafb;\n}\n/*# sourceMappingURL=analysis-results.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AnalysisResultsComponent, [{
    type: Component,
    args: [{ selector: "app-analysis-results", standalone: true, template: '<div class="card results">\r\n  <h3>Analysis Results</h3>\r\n\r\n  <div class="stats-grid">\r\n    <div class="stat">\r\n      <span class="stat-value">{{ data().totalWordCount }}</span>\r\n      <span class="stat-label">Total Words</span>\r\n    </div>\r\n    <div class="stat">\r\n      <span class="stat-value">{{ data().uniqueWordCount }}</span>\r\n      <span class="stat-label">Unique Words</span>\r\n    </div>\r\n    <div class="stat">\r\n      <span class="stat-value">{{ data().longestWord }}</span>\r\n      <span class="stat-label">Longest Word</span>\r\n    </div>\r\n  </div>\r\n\r\n  <h4>Top Words</h4>\r\n  <table class="freq-table">\r\n    <thead>\r\n      <tr>\r\n        <th>#</th>\r\n        <th>Word</th>\r\n        <th>Frequency</th>\r\n      </tr>\r\n    </thead>\r\n    <tbody>\r\n      @for (item of data().topWords; track item.word; let i = $index) {\r\n        <tr>\r\n          <td>{{ i + 1 }}</td>\r\n          <td>{{ item.word }}</td>\r\n          <td>{{ item.frequency }}</td>\r\n        </tr>\r\n      }\r\n    </tbody>\r\n  </table>\r\n</div>\r\n', styles: ["/* src/app/features/analysis/components/analysis-results.component.scss */\n.stats-grid {\n  display: grid;\n  grid-template-columns: repeat(3, 1fr);\n  gap: 1rem;\n  margin: 1rem 0;\n}\n.stat {\n  display: flex;\n  flex-direction: column;\n  align-items: center;\n  padding: 0.75rem;\n  background: #f9fafb;\n  border-radius: 6px;\n}\n.stat-value {\n  font-size: 1.5rem;\n  font-weight: 700;\n  color: #3b82f6;\n}\n.stat-label {\n  font-size: 0.75rem;\n  color: #6b7280;\n}\n.freq-table {\n  width: 100%;\n  border-collapse: collapse;\n  margin-top: 0.5rem;\n}\n.freq-table th,\n.freq-table td {\n  padding: 0.5rem;\n  text-align: left;\n  border-bottom: 1px solid #e5e7eb;\n}\n.freq-table th {\n  font-weight: 600;\n  color: #374151;\n}\n.freq-table tr:hover td {\n  background: #f9fafb;\n}\n/*# sourceMappingURL=analysis-results.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(AnalysisResultsComponent, { className: "AnalysisResultsComponent", filePath: "src/app/features/analysis/components/analysis-results.component.ts", lineNumber: 10 });
})();

// src/app/features/analysis/containers/analysis-page.component.ts
function AnalysisPageComponent_Case_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-empty-state", 2);
  }
}
function AnalysisPageComponent_Case_4_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function AnalysisPageComponent_Case_5_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-analysis-results", 3);
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext();
    \u0275\u0275property("data", ctx_r0.loadedData());
  }
}
function AnalysisPageComponent_Case_6_Template(rf, ctx) {
  if (rf & 1) {
    const _r2 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-error-display", 5);
    \u0275\u0275listener("retry", function AnalysisPageComponent_Case_6_Template_app_error_display_retry_0_listener() {
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
var AnalysisPageComponent = class _AnalysisPageComponent {
  analysisSvc = inject(AnalysisService);
  _form;
  _lastText = "";
  loadedData = () => {
    const s = this.analysisSvc.state();
    return s.stage === "loaded" ? s.data : { topWords: [], totalWordCount: 0, uniqueWordCount: 0, longestWord: "", analyzedAt: "" };
  };
  errorMsg = () => {
    const s = this.analysisSvc.state();
    return s.stage === "error" ? s.error : "";
  };
  hasUnsavedChanges() {
    return this._form?.isDirty() ?? false;
  }
  onAnalyze(text) {
    this._lastText = text;
    this.analysisSvc.analyze(text);
  }
  onRetry() {
    if (this._lastText)
      this.analysisSvc.analyze(this._lastText);
  }
  static \u0275fac = function AnalysisPageComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AnalysisPageComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _AnalysisPageComponent, selectors: [["app-analysis-page"]], viewQuery: function AnalysisPageComponent_Query(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275viewQuery(AnalysisFormComponent, 5);
    }
    if (rf & 2) {
      let _t;
      \u0275\u0275queryRefresh(_t = \u0275\u0275loadQuery()) && (ctx._form = _t.first);
    }
  }, decls: 7, vars: 2, consts: [[1, "page-title"], [3, "analyze", "isLoading"], ["message", "Enter text above to analyze word frequency"], [3, "data"], [3, "message"], [3, "retry", "message"]], template: function AnalysisPageComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "h1", 0);
      \u0275\u0275text(1, "Frequency Analysis");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(2, "app-analysis-form", 1);
      \u0275\u0275listener("analyze", function AnalysisPageComponent_Template_app_analysis_form_analyze_2_listener($event) {
        return ctx.onAnalyze($event);
      });
      \u0275\u0275elementEnd();
      \u0275\u0275template(3, AnalysisPageComponent_Case_3_Template, 1, 0, "app-empty-state", 2)(4, AnalysisPageComponent_Case_4_Template, 1, 0, "app-loading-spinner")(5, AnalysisPageComponent_Case_5_Template, 1, 1, "app-analysis-results", 3)(6, AnalysisPageComponent_Case_6_Template, 1, 1, "app-error-display", 4);
    }
    if (rf & 2) {
      let tmp_1_0;
      \u0275\u0275advance(2);
      \u0275\u0275property("isLoading", ctx.analysisSvc.state().stage === "loading");
      \u0275\u0275advance();
      \u0275\u0275conditional((tmp_1_0 = ctx.analysisSvc.state().stage) === "idle" ? 3 : tmp_1_0 === "loading" ? 4 : tmp_1_0 === "loaded" ? 5 : tmp_1_0 === "error" ? 6 : -1);
    }
  }, dependencies: [
    AnalysisFormComponent,
    AnalysisResultsComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent
  ], encapsulation: 2 });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AnalysisPageComponent, [{
    type: Component,
    args: [{ selector: "app-analysis-page", standalone: true, imports: [
      AnalysisFormComponent,
      AnalysisResultsComponent,
      LoadingSpinnerComponent,
      ErrorDisplayComponent,
      EmptyStateComponent
    ], template: `<h1 class="page-title">Frequency Analysis</h1>\r
\r
<app-analysis-form\r
  [isLoading]="analysisSvc.state().stage === 'loading'"\r
  (analyze)="onAnalyze($event)"\r
/>\r
\r
@switch (analysisSvc.state().stage) {\r
  @case ('idle') {\r
    <app-empty-state message="Enter text above to analyze word frequency" />\r
  }\r
  @case ('loading') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loaded') {\r
    <app-analysis-results [data]="loadedData()" />\r
  }\r
  @case ('error') {\r
    <app-error-display [message]="errorMsg()" (retry)="onRetry()" />\r
  }\r
}\r
` }]
  }], null, { _form: [{
    type: ViewChild,
    args: [AnalysisFormComponent]
  }] });
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(AnalysisPageComponent, { className: "AnalysisPageComponent", filePath: "src/app/features/analysis/containers/analysis-page.component.ts", lineNumber: 23 });
})();
export {
  AnalysisPageComponent
};
//# sourceMappingURL=chunk-RJ4TN2LI.js.map
