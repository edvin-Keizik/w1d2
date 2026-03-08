import {
  ApiService,
  Injectable,
  inject,
  setClassMetadata,
  signal,
  ɵɵdefineInjectable
} from "./chunk-5B3AD7EP.js";

// src/app/core/services/dictionary.service.ts
var DictionaryService = class _DictionaryService {
  _api = inject(ApiService);
  state = signal({ stage: "idle" });
  addWordState = signal({ stage: "idle" });
  loadPage(page, pageSize = 90) {
    this.state.set({ stage: "loading" });
    this._api.get("words/paginated", { page, pageSize }).subscribe({
      next: (data) => this.state.set({ stage: "loaded", data }),
      error: (err) => this.state.set({ stage: "error", error: err.message ?? "Failed to load dictionary" })
    });
  }
  addWord(word) {
    this.addWordState.set({ stage: "loading" });
    this._api.post("words", word).subscribe({
      next: (msg) => {
        this.addWordState.set({ stage: "loaded", data: msg });
      },
      error: (err) => this.addWordState.set({ stage: "error", error: err.message ?? "Failed to add word" })
    });
  }
  resetAddWord() {
    this.addWordState.set({ stage: "idle" });
  }
  static \u0275fac = function DictionaryService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _DictionaryService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _DictionaryService, factory: _DictionaryService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(DictionaryService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

export {
  DictionaryService
};
//# sourceMappingURL=chunk-6PNKKLZZ.js.map
