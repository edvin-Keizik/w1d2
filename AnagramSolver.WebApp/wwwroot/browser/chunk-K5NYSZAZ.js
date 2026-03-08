import {
  Pipe,
  setClassMetadata,
  ɵɵdefinePipe
} from "./chunk-5B3AD7EP.js";

// src/app/shared/pipes/time-ago.pipe.ts
var TimeAgoPipe = class _TimeAgoPipe {
  transform(value) {
    const date = value instanceof Date ? value : new Date(value);
    const now = Date.now();
    const diffMs = now - date.getTime();
    if (diffMs < 0)
      return "just now";
    const seconds = Math.floor(diffMs / 1e3);
    if (seconds < 60)
      return "just now";
    const minutes = Math.floor(seconds / 60);
    if (minutes < 60)
      return `${minutes} min ago`;
    const hours = Math.floor(minutes / 60);
    if (hours < 24)
      return `${hours}h ago`;
    const days = Math.floor(hours / 24);
    if (days < 30)
      return `${days}d ago`;
    const months = Math.floor(days / 30);
    return `${months}mo ago`;
  }
  static \u0275fac = function TimeAgoPipe_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _TimeAgoPipe)();
  };
  static \u0275pipe = /* @__PURE__ */ \u0275\u0275definePipe({ name: "timeAgo", type: _TimeAgoPipe, pure: true });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(TimeAgoPipe, [{
    type: Pipe,
    args: [{ name: "timeAgo", standalone: true }]
  }], null, null);
})();

export {
  TimeAgoPipe
};
//# sourceMappingURL=chunk-K5NYSZAZ.js.map
