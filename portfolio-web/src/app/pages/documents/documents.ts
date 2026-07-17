import { Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { DocumentItem } from '../../core/models/portfolio.models';
import { DocumentAccessService } from '../../core/services/document-access.service';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { formatBytes, monthYear } from '../../core/utils/format';

interface DocGroup {
  type: string;
  items: DocumentItem[];
}

@Component({
  selector: 'app-documents',
  imports: [RouterLink, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './documents.html',
  styleUrl: './documents.scss'
})
export class DocumentsPage {
  private readonly api = inject(PortfolioApiService);
  readonly access = inject(DocumentAccessService);
  readonly formatBytes = formatBytes;
  readonly monthYear = monthYear;

  readonly state = toState(this.api.getDocuments());
  readonly previewId = signal<number | null>(null);

  readonly groups = computed<DocGroup[]>(() => {
    const st = this.state();
    if (st.status !== 'success' || !st.data) return [];
    const map = new Map<string, DocumentItem[]>();
    for (const d of st.data) {
      const key = d.documentType || 'Other';
      const bucket = map.get(key);
      if (bucket) bucket.push(d);
      else map.set(key, [d]);
    }
    return [...map.entries()].map(([type, items]) => ({ type, items }));
  });

  togglePreview(doc: DocumentItem): void {
    this.previewId.update((id) => (id === doc.documentId ? null : doc.documentId));
  }

  open(doc: DocumentItem): void {
    const url = this.access.url(doc);
    if (url) window.open(url, '_blank', 'noopener');
  }
}
