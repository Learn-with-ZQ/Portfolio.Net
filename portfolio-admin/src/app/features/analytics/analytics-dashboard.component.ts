import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { AnalyticsEvent, AnalyticsSummary, CountItem } from '../../core/models/portfolio.models';
import { AnalyticsApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

interface StatCard {
  label: string;
  value: number;
  icon: string;
}

@Component({
  selector: 'app-analytics-dashboard',
  imports: [
    DatePipe,
    PageHeaderComponent,
    MatIconModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule
  ],
  template: `
    <app-page-header title="Analytics" subtitle="Visitors, downloads and engagement" />

    @if (loading()) {
      <div class="center"><mat-spinner diameter="40" /></div>
    } @else if (summary(); as s) {
      <div class="stats">
        @for (card of cards(s); track card.label) {
          <div class="stat">
            <mat-icon>{{ card.icon }}</mat-icon>
            <span class="num">{{ card.value }}</span>
            <span class="label">{{ card.label }}</span>
          </div>
        }
      </div>

      <div class="breakdowns">
        <div class="panel">
          <h3><mat-icon>public</mat-icon> Top countries</h3>
          @for (row of s.byCountry; track row.label) {
            <div class="bar-row">
              <span class="bar-label">{{ row.label }}</span>
              <span class="bar"><span class="fill" [style.width.%]="pct(row, s.byCountry)"></span></span>
              <span class="bar-count">{{ row.count }}</span>
            </div>
          } @empty {
            <p class="empty">No data yet.</p>
          }
        </div>

        <div class="panel">
          <h3><mat-icon>language</mat-icon> Browsers</h3>
          @for (row of s.byBrowser; track row.label) {
            <div class="bar-row">
              <span class="bar-label">{{ row.label }}</span>
              <span class="bar"><span class="fill" [style.width.%]="pct(row, s.byBrowser)"></span></span>
              <span class="bar-count">{{ row.count }}</span>
            </div>
          } @empty {
            <p class="empty">No data yet.</p>
          }
        </div>

        <div class="panel">
          <h3><mat-icon>devices</mat-icon> Devices</h3>
          @for (row of s.byDevice; track row.label) {
            <div class="bar-row">
              <span class="bar-label">{{ row.label }}</span>
              <span class="bar"><span class="fill" [style.width.%]="pct(row, s.byDevice)"></span></span>
              <span class="bar-count">{{ row.count }}</span>
            </div>
          } @empty {
            <p class="empty">No data yet.</p>
          }
        </div>
      </div>

      <h3 class="log-title">Recent events</h3>
      <table mat-table [dataSource]="events()" class="mat-elevation-z1">
        <ng-container matColumnDef="eventType">
          <th mat-header-cell *matHeaderCellDef>Event</th>
          <td mat-cell *matCellDef="let e"><span class="type">{{ e.eventType }}</span></td>
        </ng-container>
        <ng-container matColumnDef="path">
          <th mat-header-cell *matHeaderCellDef>Path</th>
          <td mat-cell *matCellDef="let e">{{ e.path || '—' }}</td>
        </ng-container>
        <ng-container matColumnDef="where">
          <th mat-header-cell *matHeaderCellDef>Location</th>
          <td mat-cell *matCellDef="let e">{{ e.city ? e.city + ', ' : '' }}{{ e.country || '—' }}</td>
        </ng-container>
        <ng-container matColumnDef="client">
          <th mat-header-cell *matHeaderCellDef>Client</th>
          <td mat-cell *matCellDef="let e">{{ e.browser || '—' }} / {{ e.device || '—' }}</td>
        </ng-container>
        <ng-container matColumnDef="createdAt">
          <th mat-header-cell *matHeaderCellDef>When</th>
          <td mat-cell *matCellDef="let e">{{ e.createdAt | date: 'MMM d, HH:mm' }}</td>
        </ng-container>
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
      </table>

      @if (!events().length) {
        <p class="empty">No events recorded yet.</p>
      }

      <mat-paginator
        [length]="totalRecords()"
        [pageIndex]="pageNumber() - 1"
        [pageSize]="pageSize()"
        [pageSizeOptions]="[10, 25, 50]"
        (page)="onPage($event)"
      />
    }
  `,
  styles: `
    .center { display: grid; place-items: center; padding: 48px; }
    .empty { color: rgba(0,0,0,0.55); padding: 12px 0; }
    .stats {
      display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
      gap: 12px; margin-bottom: 20px;
    }
    .stat {
      background: #fff; border: 1px solid rgba(0,0,0,0.08); border-radius: 12px;
      padding: 16px; display: flex; flex-direction: column; gap: 2px;
    }
    .stat mat-icon { color: #512bd4; }
    .num { font-size: 1.7rem; font-weight: 800; }
    .label { color: rgba(0,0,0,0.55); font-size: 0.82rem; }
    .breakdowns {
      display: grid; grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
      gap: 12px; margin-bottom: 24px;
    }
    .panel { background: #fff; border: 1px solid rgba(0,0,0,0.08); border-radius: 12px; padding: 16px; }
    .panel h3 { display: flex; align-items: center; gap: 8px; margin: 0 0 12px; font-size: 1rem; }
    .panel h3 mat-icon { color: #0078d4; }
    .bar-row { display: grid; grid-template-columns: 90px 1fr 36px; align-items: center; gap: 8px; margin-bottom: 6px; }
    .bar-label { font-size: 0.82rem; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .bar { height: 8px; background: rgba(0,0,0,0.06); border-radius: 999px; overflow: hidden; }
    .fill { display: block; height: 100%; background: linear-gradient(90deg, #512bd4, #0078d4); border-radius: 999px; }
    .bar-count { font-size: 0.8rem; font-weight: 600; text-align: right; }
    .log-title { margin: 0 0 12px; }
    table { width: 100%; }
    .type {
      background: rgba(0,120,212,0.1); color: #0078d4; font-size: 0.75rem; font-weight: 700;
      padding: 3px 10px; border-radius: 999px;
    }
  `
})
export class AnalyticsDashboardComponent {
  private readonly api = inject(AnalyticsApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly summary = signal<AnalyticsSummary | null>(null);
  readonly events = signal<AnalyticsEvent[]>([]);
  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalRecords = signal(0);

  readonly displayedColumns = ['eventType', 'path', 'where', 'client', 'createdAt'];

  constructor() {
    this.load();
  }

  cards(s: AnalyticsSummary): StatCard[] {
    return [
      { label: 'Total events', value: s.totalEvents, icon: 'insights' },
      { label: 'Unique visitors', value: s.uniqueVisitors, icon: 'group' },
      { label: 'Page views', value: s.pageViews, icon: 'visibility' },
      { label: 'Resume downloads', value: s.resumeDownloads, icon: 'download' },
      { label: 'Certificate downloads', value: s.certificateDownloads, icon: 'verified' },
      { label: 'Project views', value: s.projectViews, icon: 'folder' },
      { label: 'Blog views', value: s.blogViews, icon: 'article' },
      { label: 'Contact requests', value: s.contactRequests, icon: 'mail' }
    ];
  }

  pct(row: CountItem, rows: CountItem[]): number {
    const max = Math.max(1, ...rows.map((r) => r.count));
    return Math.max(4, Math.round((row.count / max) * 100));
  }

  load(): void {
    this.loading.set(true);
    this.api.getSummary(this.portfolio.profileId()).subscribe({
      next: (s) => {
        this.summary.set(s);
        this.loading.set(false);
        this.loadEvents();
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load analytics', 'Close', { duration: 5000 });
        this.loading.set(false);
      }
    });
  }

  loadEvents(): void {
    this.api
      .getPaged({
        portfolioProfileId: this.portfolio.profileId(),
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize()
      })
      .subscribe({
        next: (r) => {
          this.events.set(r.items);
          this.totalRecords.set(r.totalRecords);
        },
        error: (err: Error) =>
          this.snackBar.open(err.message ?? 'Failed to load events', 'Close', { duration: 5000 })
      });
  }

  onPage(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.loadEvents();
  }
}
