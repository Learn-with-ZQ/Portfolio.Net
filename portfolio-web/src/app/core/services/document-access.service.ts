import { Injectable, inject } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
import { DocumentItem } from '../models/portfolio.models';

/**
 * Resolves a document's public URL from its stored path. Files are served
 * statically by the API (or an absolute URL) — there is no download API.
 */
@Injectable({ providedIn: 'root' })
export class DocumentAccessService {
  private readonly sanitizer = inject(DomSanitizer);

  /** Absolute URL to the file, or null when no path is available. */
  url(doc: DocumentItem): string | null {
    const path = doc.storagePath;
    if (!path) return null;
    return /^https?:\/\//i.test(path)
      ? path
      : `${environment.apiUrl}/${path.replace(/^\//, '')}`;
  }

  /** Sanitized URL for embedding in an inline <iframe> preview. */
  safeUrl(doc: DocumentItem): SafeResourceUrl | null {
    const url = this.url(doc);
    return url ? this.sanitizer.bypassSecurityTrustResourceUrl(url) : null;
  }

  isPreviewable(doc: DocumentItem): boolean {
    return /pdf|png|jpe?g|gif|webp|svg/i.test(doc.fileExtension);
  }
}
