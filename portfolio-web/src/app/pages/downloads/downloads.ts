import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { DocumentItem } from '../../core/models/portfolio.models';
import { DocumentAccessService } from '../../core/services/document-access.service';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { formatBytes } from '../../core/utils/format';

@Component({
  selector: 'app-downloads',
  imports: [RouterLink, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  template: `
    <section class="page">
      <h1 class="page-title">Downloads</h1>
      <p class="page-subtitle">
        Every shareable document in one place.
        <a routerLink="/documents" class="link">Browse the full document center →</a>
      </p>

      @let s = state();
      @if (s.status === 'loading') {
        <div class="state"><mat-spinner diameter="44" /><span>Loading…</span></div>
      } @else if (s.status === 'error') {
        <div class="state"><mat-icon>error_outline</mat-icon><span>{{ s.error }}</span></div>
      } @else if (s.data && s.data.length) {
        <div class="list">
          @for (doc of s.data; track doc.documentId) {
            <div class="row">
              <span class="ic"><mat-icon>description</mat-icon></span>
              <div class="body">
                <span class="title">{{ doc.documentTitle }}</span>
                <span class="sub">{{ doc.documentType }} · {{ formatBytes(doc.fileSizeBytes) }}</span>
              </div>
              @if (doc.isDownloadable && access.url(doc)) {
                <a mat-flat-button class="dl" [href]="access.url(doc)" [attr.download]="doc.fileName"
                   target="_blank" rel="noopener"><mat-icon>download</mat-icon> Download</a>
              } @else if (access.url(doc)) {
                <button mat-stroked-button type="button" (click)="open(doc)">
                  <mat-icon>lock</mat-icon> View only
                </button>
              } @else {
                <span class="na">Unavailable</span>
              }
            </div>
          }
        </div>
      } @else {
        <div class="state"><mat-icon>folder_off</mat-icon><span>No documents available.</span></div>
      }
    </section>
  `,
  styles: `
    .link { color: var(--c-secondary); font-weight: 600; margin-left: 6px; }
    .list { display: grid; gap: 10px; }
    .row {
      display: flex; align-items: center; gap: 14px;
      background: #fff; border: 1px solid var(--line); border-radius: var(--radius); padding: 14px 16px;
    }
    .ic {
      display: inline-grid; place-items: center; width: 40px; height: 40px; flex: none;
      border-radius: 10px; background: rgba(0,120,212,0.12); color: var(--c-secondary);
    }
    .body { flex: 1 1 auto; min-width: 0; display: flex; flex-direction: column; }
    .title { font-weight: 600; }
    .sub { color: var(--ink-soft); font-size: 0.82rem; }
    .dl { background: var(--grad-brand) !important; color: #fff !important; flex: none; }
    .na { color: var(--ink-soft); font-size: 0.85rem; }
  `
})
export class DownloadsPage {
  private readonly api = inject(PortfolioApiService);
  readonly access = inject(DocumentAccessService);
  readonly formatBytes = formatBytes;
  readonly state = toState(this.api.getDocuments());

  open(doc: DocumentItem): void {
    const url = this.access.url(doc);
    if (url) window.open(url, '_blank', 'noopener');
  }
}
