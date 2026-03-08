import { Component, input, output } from '@angular/core';
import { ChatHistory } from '../../../core/models/chat.model';
import { TimeAgoPipe } from '../../../shared/pipes/time-ago.pipe';

@Component({
  selector: 'app-session-detail-modal',
  standalone: true,
  imports: [TimeAgoPipe],
  templateUrl: './session-detail-modal.component.html',
  styleUrl: './session-detail-modal.component.scss',
})
export class SessionDetailModalComponent {
  data = input.required<ChatHistory>();
  close = output<void>();
}
