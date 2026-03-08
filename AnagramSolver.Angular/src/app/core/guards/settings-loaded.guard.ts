import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { SettingsService } from '../services/settings.service';

export const settingsLoadedGuard: CanActivateFn = () => {
  const settings = inject(SettingsService);

  if (settings.loaded()) {
    return true;
  }

  return new Promise<boolean>((resolve) => {
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
    }, 5000);
  });
};
