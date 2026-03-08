import { Injectable, inject, signal } from '@angular/core';
import { ApiService } from './api.service';
import { UxState } from '../models/ux-state.model';
import { SearchHistoryItem } from '../models/anagram.model';

const HISTORY_KEY = 'anagram_search_history';

@Injectable({ providedIn: 'root' })
export class AnagramService {
  private readonly _api = inject(ApiService);

  readonly state = signal<UxState<string[]>>({ stage: 'idle' });
  readonly searchHistory = signal<SearchHistoryItem[]>(this._loadHistory());

  search(word: string): void {
    this.state.set({ stage: 'loading' });
    this._api.get<string[]>(`anagrams/${encodeURIComponent(word)}`).subscribe({
      next: (results) => {
        this.state.set({ stage: 'loaded', data: results });
        this._addToHistory(word, results.length);
      },
      error: (err) => {
        this.state.set({ stage: 'error', error: err.message ?? 'Search failed' });
      },
    });
  }

  reset(): void {
    this.state.set({ stage: 'idle' });
  }

  clearHistory(): void {
    localStorage.removeItem(HISTORY_KEY);
    this.searchHistory.set([]);
  }

  private _addToHistory(word: string, resultCount: number): void {
    const item: SearchHistoryItem = {
      word,
      searchedAt: new Date().toISOString(),
      resultCount,
    };
    const history = [item, ...this.searchHistory()].slice(0, 50);
    this.searchHistory.set(history);
    localStorage.setItem(HISTORY_KEY, JSON.stringify(history));
  }

  private _loadHistory(): SearchHistoryItem[] {
    const raw = localStorage.getItem(HISTORY_KEY);
    if (!raw) return [];
    try {
      return JSON.parse(raw) as SearchHistoryItem[];
    } catch {
      return [];
    }
  }
}
