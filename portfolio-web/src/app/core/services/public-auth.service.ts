import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { Observable, catchError, map, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthTokens } from '../models/api.models';
import { ApiResponseParser } from './api-response-parser';

/**
 * Obtains and holds a read-only JWT for the seeded `public` user so the site
 * can call the role-protected read endpoints. The token lives in memory only
 * (no persistence needed — it is re-acquired on every app start / SSR render).
 */
@Injectable({ providedIn: 'root' })
export class PublicAuthService {
  private readonly http = inject(HttpClient);
  private readonly parser = inject(ApiResponseParser);

  private readonly _token = signal<string | null>(null);
  readonly token = this._token.asReadonly();

  /** Logs in as the public read-only user. Safe to call more than once. */
  login(): Observable<void> {
    if (this._token()) {
      return of(void 0);
    }

    return this.http
      .post<unknown>(`${environment.apiUrl}/api/auth/login`, environment.publicCredentials)
      .pipe(
        map((raw) => this.parser.parse<AuthTokens>(raw)),
        tap((tokens) => this._token.set(tokens.accessToken)),
        map(() => void 0),
        catchError((err) => {
          // Never block bootstrap: pages degrade to an error state instead.
          console.error('Public authentication failed', err);
          return of(void 0);
        })
      );
  }
}
