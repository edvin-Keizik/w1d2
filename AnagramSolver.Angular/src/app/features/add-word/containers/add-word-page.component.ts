import { Component, inject, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DictionaryService } from '../../../core/services/dictionary.service';
import { SettingsService } from '../../../core/services/settings.service';
import { NotificationService } from '../../../core/services/notification.service';
import { HasUnsavedChanges } from '../../../core/guards/unsaved-changes.guard';
import { WordFormComponent } from '../components/word-form.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorDisplayComponent } from '../../../shared/components/error-display/error-display.component';

@Component({
  selector: 'app-add-word-page',
  standalone: true,
  imports: [WordFormComponent, LoadingSpinnerComponent, ErrorDisplayComponent],
  templateUrl: './add-word-page.component.html',
  styleUrl: './add-word-page.component.scss',
})
export class AddWordPageComponent implements HasUnsavedChanges {
  protected readonly dictSvc = inject(DictionaryService);
  protected readonly settings = inject(SettingsService);
  private readonly _router = inject(Router);
  private readonly _notify = inject(NotificationService);

  @ViewChild(WordFormComponent) private _wordForm?: WordFormComponent;

  private _lastWord = '';

  readonly errorMsg = () => {
    const s = this.dictSvc.addWordState();
    return s.stage === 'error' ? s.error : '';
  };

  hasUnsavedChanges(): boolean {
    return this._wordForm?.isDirty() ?? false;
  }

  onSubmit(word: string): void {
    this._lastWord = word;
    this.dictSvc.addWord(word);
  }

  onRetry(): void {
    if (this._lastWord) this.dictSvc.addWord(this._lastWord);
  }

  addAnother(): void {
    this.dictSvc.resetAddWord();
    this._wordForm?.reset();
  }

  goToDictionary(): void {
    this.dictSvc.resetAddWord();
    this._router.navigate(['/dictionary']);
  }
}
