import { Component, inject, ElementRef, ViewChild, AfterViewChecked } from '@angular/core';
import { ChatService } from '../../../core/services/chat.service';
import { ChatWindowComponent } from '../components/chat-window.component';
import { ChatInputComponent } from '../components/chat-input.component';

@Component({
  selector: 'app-chat-page',
  standalone: true,
  imports: [ChatWindowComponent, ChatInputComponent],
  templateUrl: './chat-page.component.html',
  styleUrl: './chat-page.component.scss',
})
export class ChatPageComponent implements AfterViewChecked {
  protected readonly chatSvc = inject(ChatService);

  @ViewChild('chatContainer') private _chatContainer?: ElementRef<HTMLDivElement>;

  readonly chatErrorMsg = () => {
    const s = this.chatSvc.sendState();
    return s.stage === 'error' ? s.error : '';
  };

  ngAfterViewChecked(): void {
    this._scrollToBottom();
  }

  private _scrollToBottom(): void {
    const el = this._chatContainer?.nativeElement;
    if (el) {
      el.scrollTop = el.scrollHeight;
    }
  }
}
