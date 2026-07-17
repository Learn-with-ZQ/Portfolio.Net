import { isPlatformBrowser } from '@angular/common';
import { Component, PLATFORM_ID, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';
import { SITE } from '../../core/content/site-content';
import { DocumentItem, ResumeData, SkillFull } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange, monthYear } from '../../core/utils/format';
import {
  ResumeContact,
  buildMarkdown,
  buildPlainText,
  downloadText
} from '../../core/utils/resume-export';

@Component({
  selector: 'app-resume',
  imports: [RouterLink, MatButtonModule, MatIconModule, MatMenuModule, MatProgressSpinnerModule],
  templateUrl: './resume.html',
  styleUrl: './resume.scss'
})
export class ResumePage {
  private readonly api = inject(PortfolioApiService);
  private readonly snack = inject(MatSnackBar);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  readonly site = SITE;
  readonly dateRange = dateRange;
  readonly monthYear = monthYear;

  readonly resume = toState(this.api.getResume());
  readonly documents = toState(this.api.getDocuments());
  readonly busyId = signal<number | null>(null);

  /** Comma-joined skill keywords for a category (sorted). */
  skillItems(skill: SkillFull): string {
    return [...skill.details]
      .sort((a, b) => a.sortOrder - b.sortOrder)
      .map((d) => d.skillDetailName)
      .join(', ');
  }

  print(): void {
    if (this.isBrowser) {
      window.print();
    }
  }

  exportTxt(): void {
    const data = this.readyData();
    if (data) downloadText('resume.txt', buildPlainText(data, this.contact()), 'text/plain');
  }

  exportMarkdown(): void {
    const data = this.readyData();
    if (data) downloadText('resume.md', buildMarkdown(data, this.contact()), 'text/markdown');
  }

  formatBytes(bytes?: number): string {
    if (!bytes) return '—';
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(0)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }

  download(doc: DocumentItem): void {
    this.busyId.set(doc.documentId);
    this.api.getDocument(doc.documentId).subscribe({
      next: (detail) => {
        this.busyId.set(null);
        if (!this.isBrowser) return;
        const path = detail.storagePath;
        const url = /^https?:\/\//i.test(path)
          ? path
          : `${environment.apiUrl}/${path.replace(/^\//, '')}`;
        window.open(url, '_blank', 'noopener');
      },
      error: (err: Error) => {
        this.busyId.set(null);
        this.snack.open(err.message ?? 'Could not open document', 'Close', { duration: 4000 });
      }
    });
  }

  private readyData(): ResumeData | undefined {
    const state = this.resume();
    if (state.status !== 'success' || !state.data) {
      this.snack.open('Resume is still loading…', 'Close', { duration: 2500 });
      return undefined;
    }
    return state.data;
  }

  private contact(): ResumeContact {
    return {
      owner: this.site.owner,
      role: this.site.role,
      email: this.site.email,
      location: this.site.location,
      summary: this.site.about.intro,
      links: this.site.socials.map((s) => ({ label: s.label, url: s.url }))
    };
  }
}
