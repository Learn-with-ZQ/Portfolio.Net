import { Injectable } from '@angular/core';
import {
  StatusEnvelope,
  TypedEnvelope,
  isStatusEnvelope,
  isTypedEnvelope
} from '../models/api-response';

/**
 * Reusable, shape-agnostic API response parser.
 *
 * Accepts any of:
 *   1. `{ success, data, message, errors }`   (repo's typed ApiEnvelope)
 *   2. `{ status, message }` where `message`   (alternate; message may be a
 *      is a JSON string OR an object            JSON string that must be parsed)
 *   3. the bare payload itself
 *
 * and always yields the strongly-typed payload `T`, throwing a meaningful
 * error for failure envelopes so callers can surface a single error shape.
 */
@Injectable({ providedIn: 'root' })
export class ApiResponseParser {
  parse<T>(raw: unknown): T {
    if (raw === null || raw === undefined) {
      throw new Error('Empty response from server.');
    }

    if (isTypedEnvelope(raw)) {
      return this.fromTyped<T>(raw);
    }

    if (isStatusEnvelope(raw)) {
      return this.fromStatus<T>(raw);
    }

    // Already the payload.
    return raw as T;
  }

  private fromTyped<T>(env: TypedEnvelope<unknown>): T {
    if (!env.success) {
      throw new Error(env.message ?? env.errors?.join(', ') ?? 'Request failed.');
    }
    if (env.data === undefined || env.data === null) {
      throw new Error(env.message ?? 'Response contained no data.');
    }
    return env.data as T;
  }

  private fromStatus<T>(env: StatusEnvelope): T {
    if (env.status < 200 || env.status >= 300) {
      const msg = typeof env.message === 'string' ? env.message : `Request failed (${env.status}).`;
      throw new Error(msg);
    }
    return this.coerce<T>(env.message);
  }

  /** Turns a `message` that may be a JSON string into a typed object. */
  private coerce<T>(message: unknown): T {
    if (typeof message !== 'string') {
      return message as T;
    }
    const trimmed = message.trim();
    if (!trimmed) {
      return [] as unknown as T;
    }
    try {
      return JSON.parse(trimmed) as T;
    } catch {
      // Not JSON — return the raw string as-is.
      return message as unknown as T;
    }
  }
}
