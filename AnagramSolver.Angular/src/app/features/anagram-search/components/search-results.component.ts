import { Component, input } from '@angular/core';
import { HighlightPipe } from '../../../shared/pipes/highlight.pipe';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [HighlightPipe],
  templateUrl: './search-results.component.html',
  styleUrl: './search-results.component.scss',
})
export class SearchResultsComponent {
  results = input.required<string[]>();
  searchTerm = input('');
}
