import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly _http = inject(HttpClient);

  get<T>(path: string, params?: Record<string, string | number>): Observable<T> {
    let httpParams = new HttpParams();
    if (params) {
      for (const [key, value] of Object.entries(params)) {
        httpParams = httpParams.set(key, String(value));
      }
    }
    return this._http.get<T>(path, { params: httpParams });
  }

  post<T, B>(path: string, body: B): Observable<T> {
    return this._http.post<T>(path, body);
  }

  put<T, B>(path: string, body: B): Observable<T> {
    return this._http.put<T>(path, body);
  }

  delete<T>(path: string): Observable<T> {
    return this._http.delete<T>(path);
  }
}
