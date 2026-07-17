import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { PublicAuthService } from '../services/public-auth.service';

/** Attaches the public read-only bearer token to API requests. */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(PublicAuthService).token();

  if (token && req.url.startsWith(environment.apiUrl)) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  return next(req);
};
