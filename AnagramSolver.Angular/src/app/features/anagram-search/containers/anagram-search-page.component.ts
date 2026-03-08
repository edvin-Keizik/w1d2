import { Component, inject, signal } from '@angular/core';
import { AnagramService } from '../../../core/services/anagram.service';
import { SettingsService } from '../../../core/services/settings.service';
import { SearchFormComponent } from '../components/search-form.component';
import { SearchResultsComponent } from '../components/search-results.component';
import { SearchHistoryComponent } from '../components/search-history.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorDisplayComponent } from '../../../shared/components/error-display/error-display.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-anagram-search-page',
  standalone: true,
  imports: [
    SearchFormComponent,
    SearchResultsComponent,
    SearchHistoryComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent,
  ],
  templateUrl: './anagram-search-page.component.html',
})
export class AnagramSearchPageComponent {
  protected readonly anagramSvc = inject(AnagramService);
  protected readonly settings = inject(SettingsService);

  readonly lastSearch = signal('');

  readonly loadedData = () => {
    const s = this.anagramSvc.state();
    return s.stage === 'loaded' ? s.data : [];
  };

  readonly errorMsg = () => {
    const s = this.anagramSvc.state();
    return s.stage === 'error' ? s.error : '';
  };

  onSearch(word: string): void {
    this.lastSearch.set(word);
    this.anagramSvc.search(word);
  }

  onRetry(): void {
    const word = this.lastSearch();
    if (word) this.anagramSvc.search(word);
  }
}
