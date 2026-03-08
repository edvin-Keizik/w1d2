import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ChatService } from '../services/chat.service';

export const chatSessionGuard: CanActivateFn = () => {
  const chat = inject(ChatService);
  const router = inject(Router);

  if (chat.hasSession()) {
    return true;
  }

  // No existing session — allow access but start a new session context
  chat.startNewSession();
  return true;
};
