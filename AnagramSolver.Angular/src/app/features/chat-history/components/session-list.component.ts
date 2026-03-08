import { Component, input, output } from '@angular/core';

@Component({
  selector: 'app-session-list',
  standalone: true,
  templateUrl: './session-list.component.html',
  styleUrl: './session-list.component.scss',
})
export class SessionListComponent {
  sessions = input.required<string[]>();
  select = output<string>();
  deleteSession = output<string>();
}
