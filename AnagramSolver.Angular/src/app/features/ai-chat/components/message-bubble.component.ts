import { Component, input } from '@angular/core';
import { ChatMessage } from '../../../core/models/chat.model';
import { TimeAgoPipe } from '../../../shared/pipes/time-ago.pipe';

@Component({
  selector: 'app-message-bubble',
  standalone: true,
  imports: [TimeAgoPipe],
  templateUrl: './message-bubble.component.html',
  styleUrl: './message-bubble.component.scss',
})
export class MessageBubbleComponent {
  message = input.required<ChatMessage>();
}
