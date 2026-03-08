import {
  Injectable,
  setClassMetadata,
  signal,
  ɵɵdefineInjectable
} from "./chunk-5B3AD7EP.js";

// src/app/core/services/notification.service.ts
var NotificationService = class _NotificationService {
  _nextId = 0;
  toasts = signal([]);
  show(message, type = "info") {
    const id = this._nextId++;
    this.toasts.update((t) => [...t, { message, type, id }]);
    setTimeout(() => this.dismiss(id), 5e3);
  }
  dismiss(id) {
    this.toasts.update((t) => t.filter((toast) => toast.id !== id));
  }
  static \u0275fac = function NotificationService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _NotificationService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _NotificationService, factory: _NotificationService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NotificationService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

export {
  NotificationService
};
//# sourceMappingURL=chunk-4P7ABBOS.js.map
