import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notifications = inject(NotificationService);

  return next(req).pipe(
    catchError((error) => {
      let message = 'An unexpected error occurred';

      if (error.status === 0) {
        message = 'Unable to connect to the server';
      } else if (error.status === 400) {
        message = error.error?.message ?? 'Bad request';
      } else if (error.status === 404) {
        message = error.error?.message ?? 'Resource not found';
      } else if (error.status === 408) {
        message = 'Request timed out';
      } else if (error.status >= 500) {
        message = 'Server error — please try again later';
      }

      notifications.show(message, 'error');

      return throwError(() => ({ ...error, message }));
    })
  );
};
