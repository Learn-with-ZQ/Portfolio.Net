/**
 * The platform's API has been seen returning more than one envelope shape.
 * These types describe every shape the parser tolerates.
 */

/** Repo's Portfolio.Api shape: strongly typed. */
export interface TypedEnvelope<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}

/**
 * Legacy / alternate shape: an HTTP-style status plus a `message` that is
 * often a JSON *string* (e.g. `{"status":200,"message":"[{...}]"}`).
 */
export interface StatusEnvelope {
  status: number;
  message: unknown;
}

export function isTypedEnvelope(v: unknown): v is TypedEnvelope<unknown> {
  return typeof v === 'object' && v !== null && typeof (v as TypedEnvelope<unknown>).success === 'boolean';
}

export function isStatusEnvelope(v: unknown): v is StatusEnvelope {
  return (
    typeof v === 'object' &&
    v !== null &&
    typeof (v as StatusEnvelope).status === 'number' &&
    'message' in (v as StatusEnvelope)
  );
}
