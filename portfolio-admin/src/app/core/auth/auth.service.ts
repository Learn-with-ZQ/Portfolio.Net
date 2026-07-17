import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, map, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiEnvelope, AuthTokens, LoginRequest } from '../models/api.models';

const ACCESS_TOKEN_KEY = 'portfolio_access_token';
const REFRESH_TOKEN_KEY = 'portfolio_refresh_token';
const ROLES_KEY = 'portfolio_roles';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly _accessToken = signal<string | null>(this.read(ACCESS_TOKEN_KEY));
  private readonly _refreshToken = signal<string | null>(this.read(REFRESH_TOKEN_KEY));
  private readonly _roles = signal<string[]>(this.readJson(ROLES_KEY));

  readonly isAuthenticated = computed(() => !!this._accessToken());
  readonly isAdmin = computed(() => this._roles().includes('Admin'));
  readonly roles = this._roles.asReadonly();

  get accessToken(): string | null {
    return this._accessToken();
  }

  get refreshToken(): string | null {
    return this._refreshToken();
  }

  login(request: LoginRequest): Observable<AuthTokens> {
    return this.http
      .post<ApiEnvelope<AuthTokens>>(`${environment.apiUrl}/api/auth/login`, request)
      .pipe(
        map((env) => {
          if (!env.success || !env.data) {
            throw new Error(env.message ?? 'Login failed');
          }
          return env.data;
        }),
        tap((tokens) => this.storeTokens(tokens))
      );
  }

  refresh(): Observable<AuthTokens> {
    const refreshToken = this._refreshToken();
    if (!refreshToken) {
      throw new Error('No refresh token');
    }

    return this.http
      .post<ApiEnvelope<AuthTokens>>(`${environment.apiUrl}/api/auth/refresh`, { refreshToken })
      .pipe(
        map((env) => {
          if (!env.success || !env.data) {
            throw new Error(env.message ?? 'Refresh failed');
          }
          return env.data;
        }),
        tap((tokens) => this.storeTokens(tokens))
      );
  }

  logout(): Observable<void> {
    const refreshToken = this._refreshToken();
    const request$ = refreshToken
      ? this.http.post<void>(`${environment.apiUrl}/api/auth/logout`, { refreshToken })
      : of(void 0);

    return request$.pipe(tap(() => this.clearSession()));
  }

  clearSession(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(ROLES_KEY);
    this._accessToken.set(null);
    this._refreshToken.set(null);
    this._roles.set([]);
    void this.router.navigate(['/login']);
  }

  private storeTokens(tokens: AuthTokens): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, tokens.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, tokens.refreshToken);
    localStorage.setItem(ROLES_KEY, JSON.stringify(tokens.roles ?? []));
    this._accessToken.set(tokens.accessToken);
    this._refreshToken.set(tokens.refreshToken);
    this._roles.set(tokens.roles ?? []);
  }

  private read(key: string): string | null {
    return localStorage.getItem(key);
  }

  private readJson(key: string): string[] {
    const raw = localStorage.getItem(key);
    if (!raw) return [];
    try {
      return JSON.parse(raw) as string[];
    } catch {
      return [];
    }
  }
}
