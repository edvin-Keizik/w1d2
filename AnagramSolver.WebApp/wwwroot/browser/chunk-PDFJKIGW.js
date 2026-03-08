import {
  ApiService,
  Injectable,
  inject,
  setClassMetadata,
  signal,
  ɵɵdefineInjectable
} from "./chunk-5B3AD7EP.js";

// src/app/core/services/settings.service.ts
var SettingsService = class _SettingsService {
  _api = inject(ApiService);
  settings = signal(null);
  loaded = signal(false);
  load() {
    this._api.get("settings").subscribe({
      next: (s) => {
        this.settings.set(s);
        this.loaded.set(true);
      },
      error: () => {
        this.settings.set({ minWordLength: 3, maxAnagramsToShow: 3 });
        this.loaded.set(true);
      }
    });
  }
  static \u0275fac = function SettingsService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _SettingsService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _SettingsService, factory: _SettingsService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(SettingsService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

export {
  SettingsService
};
//# sourceMappingURL=chunk-PDFJKIGW.js.map
