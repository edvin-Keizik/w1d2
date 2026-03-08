import { Component, input } from '@angular/core';
import { ChatMessage } from '../../../core/models/chat.model';
import { MessageBubbleComponent } from './message-bubble.component';

@Component({
  selector: 'app-chat-window',
  standalone: true,
  imports: [MessageBubbleComponent],
  templateUrl: './chat-window.component.html',
  styleUrl: './chat-window.component.scss',
})
export class ChatWindowComponent {
  messages = input.required<ChatMessage[]>();
}
