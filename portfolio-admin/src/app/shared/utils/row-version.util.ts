export function normalizeRowVersion(value: unknown): string {
  if (typeof value === 'string') {
    return value;
  }

  if (Array.isArray(value)) {
    return btoa(String.fromCharCode(...value));
  }

  return '';
}
