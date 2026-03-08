import { Component, input, output } from '@angular/core';
import { TruncatePipe } from '../../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-word-grid',
  standalone: true,
  imports: [TruncatePipe],
  templateUrl: './word-grid.component.html',
  styleUrl: './word-grid.component.scss',
})
export class WordGridComponent {
  words = input.required<string[]>();
  wordClick = output<string>();
}
