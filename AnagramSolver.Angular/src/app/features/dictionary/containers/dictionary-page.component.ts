import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { DictionaryService } from '../../../core/services/dictionary.service';
import { WordGridComponent } from '../components/word-grid.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorDisplayComponent } from '../../../shared/components/error-display/error-display.component';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state.component';

@Component({
  selector: 'app-dictionary-page',
  standalone: true,
  imports: [
    WordGridComponent,
    PaginationComponent,
    LoadingSpinnerComponent,
    ErrorDisplayComponent,
    EmptyStateComponent,
  ],
  templateUrl: './dictionary-page.component.html',
})
export class DictionaryPageComponent implements OnInit {
  protected readonly dictSvc = inject(DictionaryService);
  private readonly _router = inject(Router);

  readonly currentPage = signal(1);

  readonly loadedData = () => {
    const s = this.dictSvc.state();
    return s.stage === 'loaded' ? s.data : { words: [], totalCount: 0, page: 1, pageSize: 90 };
  };

  readonly errorMsg = () => {
    const s = this.dictSvc.state();
    return s.stage === 'error' ? s.error : '';
  };

  ngOnInit(): void {
    this.loadPage();
  }

  loadPage(): void {
    this.dictSvc.loadPage(this.currentPage());
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.dictSvc.loadPage(page);
  }

  onWordClick(word: string): void {
    this._router.navigate(['/search'], { queryParams: { word } });
  }
}
