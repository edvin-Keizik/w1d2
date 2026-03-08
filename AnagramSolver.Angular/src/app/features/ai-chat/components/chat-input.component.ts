import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-chat-input',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './chat-input.component.html',
  styleUrl: './chat-input.component.scss',
})
export class ChatInputComponent {
  isSending = input(false);
  sendMessage = output<string>();

  messageControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required],
  });

  onSend(): void {
    if (this.messageControl.valid) {
      this.sendMessage.emit(this.messageControl.value.trim());
      this.messageControl.reset();
    }
  }
}
