import { Signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Observable, catchError, map, of, startWith } from 'rxjs';

export interface AsyncState<T> {
  status: 'loading' | 'success' | 'error';
  data?: T;
  error?: string;
}

/**
 * Converts a one-shot HTTP observable into a signal exposing loading / success
 * / error status. Works during SSR — the request resolves server-side and the
 * result is transferred to the client via hydration, avoiding a refetch.
 */
export function toState<T>(source$: Observable<T>): Signal<AsyncState<T>> {
  return toSignal(
    source$.pipe(
      map((data): AsyncState<T> => ({ status: 'success', data })),
      catchError((err): Observable<AsyncState<T>> =>
        of({ status: 'error', error: err?.message ?? 'Failed to load data' })
      ),
      startWith<AsyncState<T>>({ status: 'loading' })
    ),
    { initialValue: { status: 'loading' } }
  );
}
