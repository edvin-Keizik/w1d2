import { Injectable, inject, signal } from '@angular/core';
import { ApiService } from './api.service';
import { UxState } from '../models/ux-state.model';
import { PaginatedResult } from '../models/paginated.model';

@Injectable({ providedIn: 'root' })
export class DictionaryService {
  private readonly _api = inject(ApiService);

  readonly state = signal<UxState<PaginatedResult<string>>>({ stage: 'idle' });
  readonly addWordState = signal<UxState<string>>({ stage: 'idle' });

  loadPage(page: number, pageSize = 90): void {
    this.state.set({ stage: 'loading' });
    this._api
      .get<PaginatedResult<string>>('words/paginated', { page, pageSize })
      .subscribe({
        next: (data) => this.state.set({ stage: 'loaded', data }),
        error: (err) =>
          this.state.set({ stage: 'error', error: err.message ?? 'Failed to load dictionary' }),
      });
  }

  addWord(word: string): void {
    this.addWordState.set({ stage: 'loading' });
    this._api.post<string, string>('words', word).subscribe({
      next: (msg) => {
        this.addWordState.set({ stage: 'loaded', data: msg });
      },
      error: (err) =>
        this.addWordState.set({ stage: 'error', error: err.message ?? 'Failed to add word' }),
    });
  }

  resetAddWord(): void {
    this.addWordState.set({ stage: 'idle' });
  }
}
