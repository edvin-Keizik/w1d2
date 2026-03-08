import { Injectable, inject, signal } from '@angular/core';
import { ApiService } from './api.service';
import { AppSettings } from '../models/settings.model';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  private readonly _api = inject(ApiService);

  readonly settings = signal<AppSettings | null>(null);
  readonly loaded = signal(false);

  load(): void {
    this._api.get<AppSettings>('settings').subscribe({
      next: (s) => {
        this.settings.set(s);
        this.loaded.set(true);
      },
      error: () => {
        this.settings.set({ minWordLength: 3, maxAnagramsToShow: 3 });
        this.loaded.set(true);
      },
    });
  }
}
