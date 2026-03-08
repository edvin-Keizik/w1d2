import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-error-display',
  standalone: true,
  templateUrl: './error-display.component.html',
  styleUrl: './error-display.component.scss',
})
export class ErrorDisplayComponent {
  message = input.required<string>();
  retry = output<void>();
}
