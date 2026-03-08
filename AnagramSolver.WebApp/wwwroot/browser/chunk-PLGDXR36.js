import {
  NotificationService
} from "./chunk-4P7ABBOS.js";
import {
  SettingsService
} from "./chunk-PDFJKIGW.js";
import {
  DictionaryService
} from "./chunk-6PNKKLZZ.js";
import {
  Router
} from "./chunk-EYJBRYUB.js";
import "./chunk-YPJFZ374.js";
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
  ErrorDisplayComponent,
  LoadingSpinnerComponent
} from "./chunk-OTOFTWZT.js";
import {
  Component,
  ViewChild,
  inject,
  input,
  output,
  setClassMetadata,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵconditional,
  ɵɵdefineComponent,
  ɵɵelement,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵgetCurrentView,
  ɵɵlistener,
  ɵɵloadQuery,
  ɵɵnextContext,
  ɵɵproperty,
  ɵɵqueryRefresh,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate1,
  ɵɵviewQuery
} from "./chunk-5B3AD7EP.js";

// src/app/features/add-word/components/word-form.component.ts
function WordFormComponent_Conditional_5_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 5);
    \u0275\u0275text(1, "Word is required");
    \u0275\u0275elementEnd();
  }
}
function WordFormComponent_Conditional_5_Conditional_1_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 5);
    \u0275\u0275text(1);
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext(2);
    \u0275\u0275advance();
    \u0275\u0275textInterpolate1("Minimum ", ctx_r0.minLength(), " characters");
  }
}
function WordFormComponent_Conditional_5_Conditional_2_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275elementStart(0, "div", 5);
    \u0275\u0275text(1, "Only letters are allowed");
    \u0275\u0275elementEnd();
  }
}
function WordFormComponent_Conditional_5_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275template(0, WordFormComponent_Conditional_5_Conditional_0_Template, 2, 0, "div", 5)(1, WordFormComponent_Conditional_5_Conditional_1_Template, 2, 1, "div", 5)(2, WordFormComponent_Conditional_5_Conditional_2_Template, 2, 0, "div", 5);
  }
  if (rf & 2) {
    const ctx_r0 = \u0275\u0275nextContext();
    \u0275\u0275conditional(ctx_r0.wordControl.hasError("required") ? 0 : -1);
    \u0275\u0275advance();
    \u0275\u0275conditional(ctx_r0.wordControl.hasError("minlength") ? 1 : -1);
    \u0275\u0275advance();
    \u0275\u0275conditional(ctx_r0.wordControl.hasError("pattern") ? 2 : -1);
  }
}
function WordFormComponent_Conditional_7_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Adding... ");
  }
}
function WordFormComponent_Conditional_8_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275text(0, " Add Word ");
  }
}
var WordFormComponent = class _WordFormComponent {
  minLength = input(3);
  isLoading = input(false);
  submitWord = output();
  wordControl = new FormControl("", {
    nonNullable: true,
    validators: [
      Validators.required,
      Validators.minLength(3),
      Validators.pattern(/^[\p{L}]+$/u)
    ]
  });
  isDirty() {
    return this.wordControl.dirty;
  }
  onSubmit() {
    if (this.wordControl.valid) {
      this.submitWord.emit(this.wordControl.value.trim());
    }
  }
  reset() {
    this.wordControl.reset();
  }
  static \u0275fac = function WordFormComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _WordFormComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _WordFormComponent, selectors: [["app-word-form"]], inputs: { minLength: [1, "minLength"], isLoading: [1, "isLoading"] }, outputs: { submitWord: "submitWord" }, decls: 9, vars: 4, consts: [[1, "card", "word-form", 3, "ngSubmit"], [1, "form-group"], ["for", "newWord"], ["id", "newWord", "type", "text", "placeholder", "Enter a new word...", "autocomplete", "off", 3, "formControl"], ["type", "submit", 1, "btn", "btn-primary", 3, "disabled"], [1, "error-msg"]], template: function WordFormComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "form", 0);
      \u0275\u0275listener("ngSubmit", function WordFormComponent_Template_form_ngSubmit_0_listener() {
        return ctx.onSubmit();
      });
      \u0275\u0275elementStart(1, "div", 1)(2, "label", 2);
      \u0275\u0275text(3, "New Word");
      \u0275\u0275elementEnd();
      \u0275\u0275element(4, "input", 3);
      \u0275\u0275template(5, WordFormComponent_Conditional_5_Template, 3, 3);
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(6, "button", 4);
      \u0275\u0275template(7, WordFormComponent_Conditional_7_Template, 1, 0)(8, WordFormComponent_Conditional_8_Template, 1, 0);
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
  }, dependencies: [ReactiveFormsModule, \u0275NgNoValidate, DefaultValueAccessor, NgControlStatus, NgControlStatusGroup, FormControlDirective], styles: ["\n\n.word-form[_ngcontent-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n/*# sourceMappingURL=word-form.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(WordFormComponent, [{
    type: Component,
    args: [{ selector: "app-word-form", standalone: true, imports: [ReactiveFormsModule], template: `<form class="card word-form" (ngSubmit)="onSubmit()">\r
  <div class="form-group">\r
    <label for="newWord">New Word</label>\r
    <input\r
      id="newWord"\r
      type="text"\r
      [formControl]="wordControl"\r
      placeholder="Enter a new word..."\r
      autocomplete="off"\r
    />\r
    @if (wordControl.invalid && wordControl.touched) {\r
      @if (wordControl.hasError('required')) {\r
        <div class="error-msg">Word is required</div>\r
      }\r
      @if (wordControl.hasError('minlength')) {\r
        <div class="error-msg">Minimum {{ minLength() }} characters</div>\r
      }\r
      @if (wordControl.hasError('pattern')) {\r
        <div class="error-msg">Only letters are allowed</div>\r
      }\r
    }\r
  </div>\r
  <button\r
    type="submit"\r
    class="btn btn-primary"\r
    [disabled]="wordControl.invalid || isLoading()"\r
  >\r
    @if (isLoading()) {\r
      Adding...\r
    } @else {\r
      Add Word\r
    }\r
  </button>\r
</form>\r
`, styles: ["/* src/app/features/add-word/components/word-form.component.scss */\n.word-form {\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n/*# sourceMappingURL=word-form.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(WordFormComponent, { className: "WordFormComponent", filePath: "src/app/features/add-word/components/word-form.component.ts", lineNumber: 11 });
})();

// src/app/features/add-word/containers/add-word-page.component.ts
function AddWordPageComponent_Case_3_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "app-loading-spinner");
  }
}
function AddWordPageComponent_Case_4_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "div", 2)(1, "p");
    \u0275\u0275text(2, "Word added successfully!");
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(3, "button", 4);
    \u0275\u0275listener("click", function AddWordPageComponent_Case_4_Template_button_click_3_listener() {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.addAnother());
    });
    \u0275\u0275text(4, "Add Another");
    \u0275\u0275elementEnd();
    \u0275\u0275elementStart(5, "button", 5);
    \u0275\u0275listener("click", function AddWordPageComponent_Case_4_Template_button_click_5_listener() {
      \u0275\u0275restoreView(_r1);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.goToDictionary());
    });
    \u0275\u0275text(6, "Go to Dictionary");
    \u0275\u0275elementEnd()();
  }
}
function AddWordPageComponent_Case_5_Template(rf, ctx) {
  if (rf & 1) {
    const _r3 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "app-error-display", 6);
    \u0275\u0275listener("retry", function AddWordPageComponent_Case_5_Template_app_error_display_retry_0_listener() {
      \u0275\u0275restoreView(_r3);
      const ctx_r1 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r1.onRetry());
    });
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const ctx_r1 = \u0275\u0275nextContext();
    \u0275\u0275property("message", ctx_r1.errorMsg());
  }
}
var AddWordPageComponent = class _AddWordPageComponent {
  dictSvc = inject(DictionaryService);
  settings = inject(SettingsService);
  _router = inject(Router);
  _notify = inject(NotificationService);
  _wordForm;
  _lastWord = "";
  errorMsg = () => {
    const s = this.dictSvc.addWordState();
    return s.stage === "error" ? s.error : "";
  };
  hasUnsavedChanges() {
    return this._wordForm?.isDirty() ?? false;
  }
  onSubmit(word) {
    this._lastWord = word;
    this.dictSvc.addWord(word);
  }
  onRetry() {
    if (this._lastWord)
      this.dictSvc.addWord(this._lastWord);
  }
  addAnother() {
    this.dictSvc.resetAddWord();
    this._wordForm?.reset();
  }
  goToDictionary() {
    this.dictSvc.resetAddWord();
    this._router.navigate(["/dictionary"]);
  }
  static \u0275fac = function AddWordPageComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AddWordPageComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _AddWordPageComponent, selectors: [["app-add-word-page"]], viewQuery: function AddWordPageComponent_Query(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275viewQuery(WordFormComponent, 5);
    }
    if (rf & 2) {
      let _t;
      \u0275\u0275queryRefresh(_t = \u0275\u0275loadQuery()) && (ctx._wordForm = _t.first);
    }
  }, decls: 6, vars: 3, consts: [[1, "page-title"], [3, "submitWord", "minLength", "isLoading"], [1, "card", "success-msg"], [3, "message"], [1, "btn", "btn-secondary", 3, "click"], [1, "btn", "btn-primary", 3, "click"], [3, "retry", "message"]], template: function AddWordPageComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "h1", 0);
      \u0275\u0275text(1, "Add Word to Dictionary");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(2, "app-word-form", 1);
      \u0275\u0275listener("submitWord", function AddWordPageComponent_Template_app_word_form_submitWord_2_listener($event) {
        return ctx.onSubmit($event);
      });
      \u0275\u0275elementEnd();
      \u0275\u0275template(3, AddWordPageComponent_Case_3_Template, 1, 0, "app-loading-spinner")(4, AddWordPageComponent_Case_4_Template, 7, 0, "div", 2)(5, AddWordPageComponent_Case_5_Template, 1, 1, "app-error-display", 3);
    }
    if (rf & 2) {
      let tmp_0_0;
      let tmp_2_0;
      \u0275\u0275advance(2);
      \u0275\u0275property("minLength", (tmp_0_0 = (tmp_0_0 = ctx.settings.settings()) == null ? null : tmp_0_0.minWordLength) !== null && tmp_0_0 !== void 0 ? tmp_0_0 : 3)("isLoading", ctx.dictSvc.addWordState().stage === "loading");
      \u0275\u0275advance();
      \u0275\u0275conditional((tmp_2_0 = ctx.dictSvc.addWordState().stage) === "loading" ? 3 : tmp_2_0 === "loaded" ? 4 : tmp_2_0 === "error" ? 5 : -1);
    }
  }, dependencies: [WordFormComponent, LoadingSpinnerComponent, ErrorDisplayComponent], styles: ["\n\n.success-msg[_ngcontent-%COMP%] {\n  margin-top: 1rem;\n  display: flex;\n  align-items: center;\n  gap: 1rem;\n  background: #f0fdf4;\n  border: 1px solid #bbf7d0;\n  color: #166534;\n}\n/*# sourceMappingURL=add-word-page.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AddWordPageComponent, [{
    type: Component,
    args: [{ selector: "app-add-word-page", standalone: true, imports: [WordFormComponent, LoadingSpinnerComponent, ErrorDisplayComponent], template: `<h1 class="page-title">Add Word to Dictionary</h1>\r
\r
<app-word-form\r
  [minLength]="settings.settings()?.minWordLength ?? 3"\r
  [isLoading]="dictSvc.addWordState().stage === 'loading'"\r
  (submitWord)="onSubmit($event)"\r
/>\r
\r
@switch (dictSvc.addWordState().stage) {\r
  @case ('loading') {\r
    <app-loading-spinner />\r
  }\r
  @case ('loaded') {\r
    <div class="card success-msg">\r
      <p>Word added successfully!</p>\r
      <button class="btn btn-secondary" (click)="addAnother()">Add Another</button>\r
      <button class="btn btn-primary" (click)="goToDictionary()">Go to Dictionary</button>\r
    </div>\r
  }\r
  @case ('error') {\r
    <app-error-display [message]="errorMsg()" (retry)="onRetry()" />\r
  }\r
}\r
`, styles: ["/* src/app/features/add-word/containers/add-word-page.component.scss */\n.success-msg {\n  margin-top: 1rem;\n  display: flex;\n  align-items: center;\n  gap: 1rem;\n  background: #f0fdf4;\n  border: 1px solid #bbf7d0;\n  color: #166534;\n}\n/*# sourceMappingURL=add-word-page.component.css.map */\n"] }]
  }], null, { _wordForm: [{
    type: ViewChild,
    args: [WordFormComponent]
  }] });
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(AddWordPageComponent, { className: "AddWordPageComponent", filePath: "src/app/features/add-word/containers/add-word-page.component.ts", lineNumber: 18 });
})();
export {
  AddWordPageComponent
};
//# sourceMappingURL=chunk-PLGDXR36.js.map
