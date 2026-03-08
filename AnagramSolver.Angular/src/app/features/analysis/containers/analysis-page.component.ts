import { Component, inject, ViewChild } from '@angular/core';
import { AnalysisService } from '../../../core/services/analysis.service';
import { HasUnsavedChanges } from '../../../core/guards/unsaved-changes.guard';
import { AnalysisFormComponent } from '../components/analysis-form.component';
import { AnalysisResultsComponent } from '../components/analysis-results.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorDisplayComponent } from '../../../shared/components/error-display/error-display.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';
import { FrequencyAnalysisResult } from '../../../core/models/analysis.model';

@Component({
  selector: 'app-analysis-page',
  standalone: true,
  imports: [
    AnalysisFormComponent,
    AnalysisResultsComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent,
  ],
  templateUrl: './analysis-page.component.html',
})
export class AnalysisPageComponent implements HasUnsavedChanges {
  protected readonly analysisSvc = inject(AnalysisService);

  @ViewChild(AnalysisFormComponent) private _form?: AnalysisFormComponent;

  private _lastText = '';

  readonly loadedData = (): FrequencyAnalysisResult => {
    const s = this.analysisSvc.state();
    return s.stage === 'loaded'
      ? s.data
      : { topWords: [], totalWordCount: 0, uniqueWordCount: 0, longestWord: '', analyzedAt: '' };
  };

  readonly errorMsg = () => {
    const s = this.analysisSvc.state();
    return s.stage === 'error' ? s.error : '';
  };

  hasUnsavedChanges(): boolean {
    return this._form?.isDirty() ?? false;
  }

  onAnalyze(text: string): void {
    this._lastText = text;
    this.analysisSvc.analyze(text);
  }

  onRetry(): void {
    if (this._lastText) this.analysisSvc.analyze(this._lastText);
  }
}
