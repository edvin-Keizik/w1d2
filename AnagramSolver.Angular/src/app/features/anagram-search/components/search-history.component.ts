import { Component, input, output } from '@angular/core';
import { SearchHistoryItem } from '../../../core/models/anagram.model';
import { TimeAgoPipe } from '../../../shared/pipes/time-ago.pipe';

@Component({
  selector: 'app-search-history',
  standalone: true,
  imports: [TimeAgoPipe],
  templateUrl: './search-history.component.html',
  styleUrl: './search-history.component.scss',
})
export class SearchHistoryComponent {
  history = input.required<SearchHistoryItem[]>();
  selectWord = output<string>();
  clearHistory = output<void>();
}
