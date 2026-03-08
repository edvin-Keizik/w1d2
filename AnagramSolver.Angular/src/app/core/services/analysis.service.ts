import { Injectable, inject, signal } from '@angular/core';
import { ApiService } from './api.service';
import { UxState } from '../models/ux-state.model';
import { FrequencyAnalysisResult } from '../models/analysis.model';

@Injectable({ providedIn: 'root' })
export class AnalysisService {
  private readonly _api = inject(ApiService);

  readonly state = signal<UxState<FrequencyAnalysisResult>>({ stage: 'idle' });

  analyze(text: string): void {
    this.state.set({ stage: 'loading' });
    this._api
      .post<FrequencyAnalysisResult, { text: string }>('v1/analysis/frequency', { text })
      .subscribe({
        next: (data) => this.state.set({ stage: 'loaded', data }),
        error: (err) =>
          this.state.set({ stage: 'error', error: err.message ?? 'Analysis failed' }),
      });
  }

  reset(): void {
    this.state.set({ stage: 'idle' });
  }
}
