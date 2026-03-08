import {
  NotificationService
} from "./chunk-4P7ABBOS.js";
import {
  SettingsService
} from "./chunk-PDFJKIGW.js";
import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
  provideRouter,
  withComponentInputBinding
} from "./chunk-EYJBRYUB.js";
import {
  bootstrapApplication
} from "./chunk-YPJFZ374.js";
import "./chunk-DYQFFD5A.js";
import {
  ChatService
} from "./chunk-LU74ZHSU.js";
import {
  Component,
  Injectable,
  __spreadProps,
  __spreadValues,
  catchError,
  computed,
  finalize,
  inject,
  provideHttpClient,
  provideZoneChangeDetection,
  setClassMetadata,
  signal,
  throwError,
  withInterceptors,
  ɵsetClassDebugInfo,
  ɵɵadvance,
  ɵɵclassMap,
  ɵɵclassProp,
  ɵɵconditional,
  ɵɵdefineComponent,
  ɵɵdefineInjectable,
  ɵɵelement,
  ɵɵelementEnd,
  ɵɵelementStart,
  ɵɵgetCurrentView,
  ɵɵlistener,
  ɵɵnextContext,
  ɵɵrepeater,
  ɵɵrepeaterCreate,
  ɵɵresetView,
  ɵɵrestoreView,
  ɵɵtemplate,
  ɵɵtext,
  ɵɵtextInterpolate1
} from "./chunk-5B3AD7EP.js";

// src/app/core/guards/settings-loaded.guard.ts
var settingsLoadedGuard = () => {
  const settings = inject(SettingsService);
  if (settings.loaded()) {
    return true;
  }
  return new Promise((resolve) => {
    settings.load();
    const interval = setInterval(() => {
      if (settings.loaded()) {
        clearInterval(interval);
        resolve(true);
      }
    }, 50);
    setTimeout(() => {
      clearInterval(interval);
      resolve(true);
    }, 5e3);
  });
};

// src/app/core/guards/chat-session.guard.ts
var chatSessionGuard = () => {
  const chat = inject(ChatService);
  const router = inject(Router);
  if (chat.hasSession()) {
    return true;
  }
  chat.startNewSession();
  return true;
};

// src/app/core/guards/unsaved-changes.guard.ts
var unsavedChangesGuard = (component) => {
  if (component.hasUnsavedChanges && component.hasUnsavedChanges()) {
    return confirm("You have unsaved changes. Are you sure you want to leave?");
  }
  return true;
};

// src/app/app.routes.ts
var routes = [
  { path: "", redirectTo: "search", pathMatch: "full" },
  {
    path: "search",
    canActivate: [settingsLoadedGuard],
    loadComponent: () => import("./chunk-BITWRFPN.js").then((m) => m.AnagramSearchPageComponent)
  },
  {
    path: "dictionary",
    canActivate: [settingsLoadedGuard],
    loadComponent: () => import("./chunk-F5KPLGTY.js").then((m) => m.DictionaryPageComponent)
  },
  {
    path: "add-word",
    canDeactivate: [unsavedChangesGuard],
    loadComponent: () => import("./chunk-PLGDXR36.js").then((m) => m.AddWordPageComponent)
  },
  {
    path: "analysis",
    canDeactivate: [unsavedChangesGuard],
    loadComponent: () => import("./chunk-RJ4TN2LI.js").then((m) => m.AnalysisPageComponent)
  },
  {
    path: "chat",
    canActivate: [chatSessionGuard],
    loadComponent: () => import("./chunk-SDGCHNVH.js").then((m) => m.ChatPageComponent)
  },
  {
    path: "chat/history",
    loadComponent: () => import("./chunk-ABZ5JVI3.js").then((m) => m.ChatHistoryPageComponent)
  },
  { path: "**", redirectTo: "search" }
];

// src/app/core/interceptors/api-base-url.interceptor.ts
var apiBaseUrlInterceptor = (req, next) => {
  if (req.url.startsWith("http") || req.url.startsWith("/")) {
    return next(req);
  }
  const apiReq = req.clone({ url: `/api/${req.url}` });
  return next(apiReq);
};

// src/app/core/services/loading.service.ts
var LoadingService = class _LoadingService {
  activeRequests = signal(0);
  isLoading = computed(() => this.activeRequests() > 0);
  start() {
    this.activeRequests.update((c) => c + 1);
  }
  stop() {
    this.activeRequests.update((c) => Math.max(0, c - 1));
  }
  static \u0275fac = function LoadingService_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _LoadingService)();
  };
  static \u0275prov = /* @__PURE__ */ \u0275\u0275defineInjectable({ token: _LoadingService, factory: _LoadingService.\u0275fac, providedIn: "root" });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(LoadingService, [{
    type: Injectable,
    args: [{ providedIn: "root" }]
  }], null, null);
})();

// src/app/core/interceptors/loading.interceptor.ts
var loadingInterceptor = (req, next) => {
  const loading = inject(LoadingService);
  loading.start();
  return next(req).pipe(finalize(() => loading.stop()));
};

// src/app/core/interceptors/error.interceptor.ts
var errorInterceptor = (req, next) => {
  const notifications = inject(NotificationService);
  return next(req).pipe(catchError((error) => {
    let message = "An unexpected error occurred";
    if (error.status === 0) {
      message = "Unable to connect to the server";
    } else if (error.status === 400) {
      message = error.error?.message ?? "Bad request";
    } else if (error.status === 404) {
      message = error.error?.message ?? "Resource not found";
    } else if (error.status === 408) {
      message = "Request timed out";
    } else if (error.status >= 500) {
      message = "Server error \u2014 please try again later";
    }
    notifications.show(message, "error");
    return throwError(() => __spreadProps(__spreadValues({}, error), { message }));
  }));
};

// src/app/app.config.ts
var appConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptors([apiBaseUrlInterceptor, loadingInterceptor, errorInterceptor]))
  ]
};

// src/app/layout/navbar.component.ts
var NavbarComponent = class _NavbarComponent {
  menuOpen = false;
  static \u0275fac = function NavbarComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _NavbarComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _NavbarComponent, selectors: [["app-navbar"]], decls: 24, vars: 2, consts: [[1, "navbar"], ["routerLink", "/search", 1, "navbar-brand"], [1, "menu-toggle", 3, "click"], [1, "nav-links"], ["routerLink", "/search", "routerLinkActive", "active", 3, "click"], ["routerLink", "/dictionary", "routerLinkActive", "active", 3, "click"], ["routerLink", "/add-word", "routerLinkActive", "active", 3, "click"], ["routerLink", "/analysis", "routerLinkActive", "active", 3, "click"], ["routerLink", "/chat", "routerLinkActive", "active", 3, "click"], ["routerLink", "/chat/history", "routerLinkActive", "active", 3, "click"]], template: function NavbarComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "nav", 0)(1, "a", 1);
      \u0275\u0275text(2, "AnagramSolver");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(3, "button", 2);
      \u0275\u0275listener("click", function NavbarComponent_Template_button_click_3_listener() {
        return ctx.menuOpen = !ctx.menuOpen;
      });
      \u0275\u0275text(4, "\u2630");
      \u0275\u0275elementEnd();
      \u0275\u0275elementStart(5, "ul", 3)(6, "li")(7, "a", 4);
      \u0275\u0275listener("click", function NavbarComponent_Template_a_click_7_listener() {
        return ctx.menuOpen = false;
      });
      \u0275\u0275text(8, "Search");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(9, "li")(10, "a", 5);
      \u0275\u0275listener("click", function NavbarComponent_Template_a_click_10_listener() {
        return ctx.menuOpen = false;
      });
      \u0275\u0275text(11, "Dictionary");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(12, "li")(13, "a", 6);
      \u0275\u0275listener("click", function NavbarComponent_Template_a_click_13_listener() {
        return ctx.menuOpen = false;
      });
      \u0275\u0275text(14, "Add Word");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(15, "li")(16, "a", 7);
      \u0275\u0275listener("click", function NavbarComponent_Template_a_click_16_listener() {
        return ctx.menuOpen = false;
      });
      \u0275\u0275text(17, "Analysis");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(18, "li")(19, "a", 8);
      \u0275\u0275listener("click", function NavbarComponent_Template_a_click_19_listener() {
        return ctx.menuOpen = false;
      });
      \u0275\u0275text(20, "AI Chat");
      \u0275\u0275elementEnd()();
      \u0275\u0275elementStart(21, "li")(22, "a", 9);
      \u0275\u0275listener("click", function NavbarComponent_Template_a_click_22_listener() {
        return ctx.menuOpen = false;
      });
      \u0275\u0275text(23, "History");
      \u0275\u0275elementEnd()()()();
    }
    if (rf & 2) {
      \u0275\u0275advance(5);
      \u0275\u0275classProp("open", ctx.menuOpen);
    }
  }, dependencies: [RouterLink, RouterLinkActive], styles: ["\n\n.navbar[_ngcontent-%COMP%] {\n  display: flex;\n  align-items: center;\n  padding: 0 1.5rem;\n  background: #1e293b;\n  height: 56px;\n  color: white;\n}\n.navbar-brand[_ngcontent-%COMP%] {\n  font-size: 1.25rem;\n  font-weight: 700;\n  color: white;\n  text-decoration: none;\n  margin-right: 2rem;\n}\n.menu-toggle[_ngcontent-%COMP%] {\n  display: none;\n  background: none;\n  border: none;\n  color: white;\n  font-size: 1.5rem;\n  cursor: pointer;\n  margin-left: auto;\n}\n.nav-links[_ngcontent-%COMP%] {\n  display: flex;\n  list-style: none;\n  gap: 0.25rem;\n  margin: 0;\n  padding: 0;\n}\n.nav-links[_ngcontent-%COMP%]   a[_ngcontent-%COMP%] {\n  color: #cbd5e1;\n  text-decoration: none;\n  padding: 0.5rem 0.75rem;\n  border-radius: 4px;\n  transition: background 0.2s;\n}\n.nav-links[_ngcontent-%COMP%]   a[_ngcontent-%COMP%]:hover {\n  background: #334155;\n  color: white;\n}\n.nav-links[_ngcontent-%COMP%]   a.active[_ngcontent-%COMP%] {\n  background: #3b82f6;\n  color: white;\n}\n@media (max-width: 768px) {\n  .menu-toggle[_ngcontent-%COMP%] {\n    display: block;\n  }\n  .nav-links[_ngcontent-%COMP%] {\n    display: none;\n    flex-direction: column;\n    position: absolute;\n    top: 56px;\n    left: 0;\n    right: 0;\n    background: #1e293b;\n    padding: 0.5rem;\n    z-index: 100;\n  }\n  .nav-links.open[_ngcontent-%COMP%] {\n    display: flex;\n  }\n}\n/*# sourceMappingURL=navbar.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(NavbarComponent, [{
    type: Component,
    args: [{ selector: "app-navbar", standalone: true, imports: [RouterLink, RouterLinkActive], template: '<nav class="navbar">\r\n  <a class="navbar-brand" routerLink="/search">AnagramSolver</a>\r\n  <button class="menu-toggle" (click)="menuOpen = !menuOpen">\u2630</button>\r\n  <ul class="nav-links" [class.open]="menuOpen">\r\n    <li><a routerLink="/search" routerLinkActive="active" (click)="menuOpen = false">Search</a></li>\r\n    <li><a routerLink="/dictionary" routerLinkActive="active" (click)="menuOpen = false">Dictionary</a></li>\r\n    <li><a routerLink="/add-word" routerLinkActive="active" (click)="menuOpen = false">Add Word</a></li>\r\n    <li><a routerLink="/analysis" routerLinkActive="active" (click)="menuOpen = false">Analysis</a></li>\r\n    <li><a routerLink="/chat" routerLinkActive="active" (click)="menuOpen = false">AI Chat</a></li>\r\n    <li><a routerLink="/chat/history" routerLinkActive="active" (click)="menuOpen = false">History</a></li>\r\n  </ul>\r\n</nav>\r\n', styles: ["/* src/app/layout/navbar.component.scss */\n.navbar {\n  display: flex;\n  align-items: center;\n  padding: 0 1.5rem;\n  background: #1e293b;\n  height: 56px;\n  color: white;\n}\n.navbar-brand {\n  font-size: 1.25rem;\n  font-weight: 700;\n  color: white;\n  text-decoration: none;\n  margin-right: 2rem;\n}\n.menu-toggle {\n  display: none;\n  background: none;\n  border: none;\n  color: white;\n  font-size: 1.5rem;\n  cursor: pointer;\n  margin-left: auto;\n}\n.nav-links {\n  display: flex;\n  list-style: none;\n  gap: 0.25rem;\n  margin: 0;\n  padding: 0;\n}\n.nav-links a {\n  color: #cbd5e1;\n  text-decoration: none;\n  padding: 0.5rem 0.75rem;\n  border-radius: 4px;\n  transition: background 0.2s;\n}\n.nav-links a:hover {\n  background: #334155;\n  color: white;\n}\n.nav-links a.active {\n  background: #3b82f6;\n  color: white;\n}\n@media (max-width: 768px) {\n  .menu-toggle {\n    display: block;\n  }\n  .nav-links {\n    display: none;\n    flex-direction: column;\n    position: absolute;\n    top: 56px;\n    left: 0;\n    right: 0;\n    background: #1e293b;\n    padding: 0.5rem;\n    z-index: 100;\n  }\n  .nav-links.open {\n    display: flex;\n  }\n}\n/*# sourceMappingURL=navbar.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(NavbarComponent, { className: "NavbarComponent", filePath: "src/app/layout/navbar.component.ts", lineNumber: 11 });
})();

// src/app/layout/footer.component.ts
var FooterComponent = class _FooterComponent {
  year = (/* @__PURE__ */ new Date()).getFullYear();
  static \u0275fac = function FooterComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _FooterComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _FooterComponent, selectors: [["app-footer"]], decls: 3, vars: 1, consts: [[1, "footer"]], template: function FooterComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "footer", 0)(1, "p");
      \u0275\u0275text(2);
      \u0275\u0275elementEnd()();
    }
    if (rf & 2) {
      \u0275\u0275advance(2);
      \u0275\u0275textInterpolate1("\xA9 ", ctx.year, " AnagramSolver");
    }
  }, styles: ["\n\n.footer[_ngcontent-%COMP%] {\n  text-align: center;\n  padding: 1rem;\n  color: #9ca3af;\n  border-top: 1px solid #e5e7eb;\n  font-size: 0.875rem;\n}\n/*# sourceMappingURL=footer.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(FooterComponent, [{
    type: Component,
    args: [{ selector: "app-footer", standalone: true, template: '<footer class="footer">\r\n  <p>&copy; {{ year }} AnagramSolver</p>\r\n</footer>\r\n', styles: ["/* src/app/layout/footer.component.scss */\n.footer {\n  text-align: center;\n  padding: 1rem;\n  color: #9ca3af;\n  border-top: 1px solid #e5e7eb;\n  font-size: 0.875rem;\n}\n/*# sourceMappingURL=footer.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(FooterComponent, { className: "FooterComponent", filePath: "src/app/layout/footer.component.ts", lineNumber: 9 });
})();

// src/app/shared/components/toast/toast.component.ts
var _forTrack0 = ($index, $item) => $item.id;
function ToastComponent_For_2_Template(rf, ctx) {
  if (rf & 1) {
    const _r1 = \u0275\u0275getCurrentView();
    \u0275\u0275elementStart(0, "div", 2);
    \u0275\u0275listener("click", function ToastComponent_For_2_Template_div_click_0_listener() {
      const toast_r2 = \u0275\u0275restoreView(_r1).$implicit;
      const ctx_r2 = \u0275\u0275nextContext();
      return \u0275\u0275resetView(ctx_r2.notifications.dismiss(toast_r2.id));
    });
    \u0275\u0275text(1);
    \u0275\u0275elementEnd();
  }
  if (rf & 2) {
    const toast_r2 = ctx.$implicit;
    \u0275\u0275classMap("toast--" + toast_r2.type);
    \u0275\u0275advance();
    \u0275\u0275textInterpolate1(" ", toast_r2.message, " ");
  }
}
var ToastComponent = class _ToastComponent {
  notifications = inject(NotificationService);
  static \u0275fac = function ToastComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ToastComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ToastComponent, selectors: [["app-toast"]], decls: 3, vars: 0, consts: [[1, "toast-container"], [1, "toast", 3, "class"], [1, "toast", 3, "click"]], template: function ToastComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275elementStart(0, "div", 0);
      \u0275\u0275repeaterCreate(1, ToastComponent_For_2_Template, 2, 3, "div", 1, _forTrack0);
      \u0275\u0275elementEnd();
    }
    if (rf & 2) {
      \u0275\u0275advance();
      \u0275\u0275repeater(ctx.notifications.toasts());
    }
  }, styles: ["\n\n.toast-container[_ngcontent-%COMP%] {\n  position: fixed;\n  top: 1rem;\n  right: 1rem;\n  z-index: 1000;\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n.toast[_ngcontent-%COMP%] {\n  padding: 0.75rem 1.25rem;\n  border-radius: 6px;\n  color: white;\n  cursor: pointer;\n  min-width: 250px;\n  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);\n  animation: _ngcontent-%COMP%_slideIn 0.3s ease-out;\n}\n.toast--success[_ngcontent-%COMP%] {\n  background: #16a34a;\n}\n.toast--error[_ngcontent-%COMP%] {\n  background: #dc2626;\n}\n.toast--info[_ngcontent-%COMP%] {\n  background: #2563eb;\n}\n@keyframes _ngcontent-%COMP%_slideIn {\n  from {\n    transform: translateX(100%);\n    opacity: 0;\n  }\n  to {\n    transform: translateX(0);\n    opacity: 1;\n  }\n}\n/*# sourceMappingURL=toast.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ToastComponent, [{
    type: Component,
    args: [{ selector: "app-toast", standalone: true, template: `<div class="toast-container">\r
  @for (toast of notifications.toasts(); track toast.id) {\r
    <div class="toast" [class]="'toast--' + toast.type" (click)="notifications.dismiss(toast.id)">\r
      {{ toast.message }}\r
    </div>\r
  }\r
</div>\r
`, styles: ["/* src/app/shared/components/toast/toast.component.scss */\n.toast-container {\n  position: fixed;\n  top: 1rem;\n  right: 1rem;\n  z-index: 1000;\n  display: flex;\n  flex-direction: column;\n  gap: 0.5rem;\n}\n.toast {\n  padding: 0.75rem 1.25rem;\n  border-radius: 6px;\n  color: white;\n  cursor: pointer;\n  min-width: 250px;\n  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);\n  animation: slideIn 0.3s ease-out;\n}\n.toast--success {\n  background: #16a34a;\n}\n.toast--error {\n  background: #dc2626;\n}\n.toast--info {\n  background: #2563eb;\n}\n@keyframes slideIn {\n  from {\n    transform: translateX(100%);\n    opacity: 0;\n  }\n  to {\n    transform: translateX(0);\n    opacity: 1;\n  }\n}\n/*# sourceMappingURL=toast.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ToastComponent, { className: "ToastComponent", filePath: "src/app/shared/components/toast/toast.component.ts", lineNumber: 10 });
})();

// src/app/layout/shell.component.ts
function ShellComponent_Conditional_0_Template(rf, ctx) {
  if (rf & 1) {
    \u0275\u0275element(0, "div", 0);
  }
}
var ShellComponent = class _ShellComponent {
  loading = inject(LoadingService);
  static \u0275fac = function ShellComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _ShellComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _ShellComponent, selectors: [["app-shell"]], decls: 6, vars: 1, consts: [[1, "global-loading-bar"], [1, "main-content"]], template: function ShellComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275template(0, ShellComponent_Conditional_0_Template, 1, 0, "div", 0);
      \u0275\u0275element(1, "app-navbar");
      \u0275\u0275elementStart(2, "main", 1);
      \u0275\u0275element(3, "router-outlet");
      \u0275\u0275elementEnd();
      \u0275\u0275element(4, "app-footer")(5, "app-toast");
    }
    if (rf & 2) {
      \u0275\u0275conditional(ctx.loading.isLoading() ? 0 : -1);
    }
  }, dependencies: [RouterOutlet, NavbarComponent, FooterComponent, ToastComponent], styles: ["\n\n[_nghost-%COMP%] {\n  display: flex;\n  flex-direction: column;\n  min-height: 100vh;\n}\n.main-content[_ngcontent-%COMP%] {\n  flex: 1;\n  padding: 1.5rem;\n  max-width: 1200px;\n  margin: 0 auto;\n  width: 100%;\n  box-sizing: border-box;\n}\n.global-loading-bar[_ngcontent-%COMP%] {\n  position: fixed;\n  top: 0;\n  left: 0;\n  right: 0;\n  height: 3px;\n  background:\n    linear-gradient(\n      90deg,\n      #3b82f6,\n      #8b5cf6,\n      #3b82f6);\n  background-size: 200% 100%;\n  animation: _ngcontent-%COMP%_shimmer 1.5s infinite;\n  z-index: 9999;\n}\n@keyframes _ngcontent-%COMP%_shimmer {\n  0% {\n    background-position: -200% 0;\n  }\n  100% {\n    background-position: 200% 0;\n  }\n}\n/*# sourceMappingURL=shell.component.css.map */"] });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(ShellComponent, [{
    type: Component,
    args: [{ selector: "app-shell", standalone: true, imports: [RouterOutlet, NavbarComponent, FooterComponent, ToastComponent], template: '@if (loading.isLoading()) {\r\n  <div class="global-loading-bar"></div>\r\n}\r\n<app-navbar />\r\n<main class="main-content">\r\n  <router-outlet />\r\n</main>\r\n<app-footer />\r\n<app-toast />\r\n', styles: ["/* src/app/layout/shell.component.scss */\n:host {\n  display: flex;\n  flex-direction: column;\n  min-height: 100vh;\n}\n.main-content {\n  flex: 1;\n  padding: 1.5rem;\n  max-width: 1200px;\n  margin: 0 auto;\n  width: 100%;\n  box-sizing: border-box;\n}\n.global-loading-bar {\n  position: fixed;\n  top: 0;\n  left: 0;\n  right: 0;\n  height: 3px;\n  background:\n    linear-gradient(\n      90deg,\n      #3b82f6,\n      #8b5cf6,\n      #3b82f6);\n  background-size: 200% 100%;\n  animation: shimmer 1.5s infinite;\n  z-index: 9999;\n}\n@keyframes shimmer {\n  0% {\n    background-position: -200% 0;\n  }\n  100% {\n    background-position: 200% 0;\n  }\n}\n/*# sourceMappingURL=shell.component.css.map */\n"] }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(ShellComponent, { className: "ShellComponent", filePath: "src/app/layout/shell.component.ts", lineNumber: 15 });
})();

// src/app/app.component.ts
var AppComponent = class _AppComponent {
  static \u0275fac = function AppComponent_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _AppComponent)();
  };
  static \u0275cmp = /* @__PURE__ */ \u0275\u0275defineComponent({ type: _AppComponent, selectors: [["app-root"]], decls: 1, vars: 0, template: function AppComponent_Template(rf, ctx) {
    if (rf & 1) {
      \u0275\u0275element(0, "app-shell");
    }
  }, dependencies: [ShellComponent], encapsulation: 2 });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(AppComponent, [{
    type: Component,
    args: [{ selector: "app-root", standalone: true, imports: [ShellComponent], template: "<app-shell />\r\n" }]
  }], null, null);
})();
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && \u0275setClassDebugInfo(AppComponent, { className: "AppComponent", filePath: "src/app/app.component.ts", lineNumber: 10 });
})();

// src/main.ts
bootstrapApplication(AppComponent, appConfig).catch((err) => console.error(err));
//# sourceMappingURL=main.js.map
