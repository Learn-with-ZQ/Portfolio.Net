/** Formatting helpers for API date strings (ISO `yyyy-MM-dd`). */

const MONTHS = [
  'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
  'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
];

/** `2023-06-01` -> `Jun 2023`. Returns '' for empty input. */
export function monthYear(date?: string | null): string {
  if (!date) return '';
  const d = new Date(date);
  if (Number.isNaN(d.getTime())) return '';
  return `${MONTHS[d.getMonth()]} ${d.getFullYear()}`;
}

/** Renders a start/end span, using `Present` when the end date is missing. */
export function dateRange(start?: string | null, end?: string | null): string {
  const from = monthYear(start);
  const to = end ? monthYear(end) : 'Present';
  return from ? `${from} — ${to}` : to;
}

/** Splits the API's comma-separated technology string into trimmed tags. */
export function splitTags(value?: string | null): string[] {
  if (!value) return [];
  return value
    .split(',')
    .map((t) => t.trim())
    .filter(Boolean);
}

/** Bytes → human size, e.g. `2.4 MB`. */
export function formatBytes(bytes?: number | null): string {
  if (!bytes) return '—';
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(0)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

/** Human duration between two dates, e.g. `2 yrs 3 mos`. Open-ended = to now. */
export function durationBetween(start?: string | null, end?: string | null): string {
  if (!start) return '';
  const s = new Date(start);
  const e = end ? new Date(end) : new Date();
  if (Number.isNaN(s.getTime())) return '';
  let months = (e.getFullYear() - s.getFullYear()) * 12 + (e.getMonth() - s.getMonth());
  if (months < 0) months = 0;
  const years = Math.floor(months / 12);
  const rem = months % 12;
  const parts: string[] = [];
  if (years) parts.push(`${years} yr${years > 1 ? 's' : ''}`);
  if (rem) parts.push(`${rem} mo${rem > 1 ? 's' : ''}`);
  return parts.join(' ') || '<1 mo';
}
