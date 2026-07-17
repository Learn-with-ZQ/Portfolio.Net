import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Testimonial } from '../../core/models/portfolio.models';
import { TestimonialsApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-testimonials-list',
  imports: [
    DatePipe,
    PageHeaderComponent,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatProgressSpinnerModule
  ],
  template: `
    <app-page-header title="Testimonials" subtitle="Review and publish visitor submissions" />

    <div class="chips">
      @for (f of filters; track f.value) {
        <button class="chip" [class.active]="status() === f.value" (click)="setStatus(f.value)">
          {{ f.label }}
        </button>
      }
    </div>

    @if (loading()) {
      <div class="center"><mat-spinner diameter="40" /></div>
    } @else if (!items().length) {
      <p class="empty">No testimonials for this filter.</p>
    } @else {
      <div class="cards">
        @for (t of items(); track t.testimonialId) {
          <div class="card" [class.pending]="t.status === 1">
            <div class="head">
              <div>
                <strong>{{ t.authorName }}</strong>
                <span class="muted">
                  {{ t.authorTitle }}@if (t.authorTitle && t.authorCompany) {, }{{ t.authorCompany }}
                  @if (t.relationship) { · {{ t.relationship }} }
                </span>
              </div>
              <span class="badge s{{ t.status }}">{{ t.statusName }}</span>
            </div>
            @if (t.rating) {
              <div class="stars">
                @for (i of stars(t.rating); track i) { <mat-icon>star</mat-icon> }
              </div>
            }
            <p class="content">{{ t.content }}</p>
            <div class="foot">
              <span class="muted">{{ t.createdAt | date: 'mediumDate' }}</span>
              <span class="actions">
                @if (t.status !== 2) {
                  <button mat-stroked-button class="ok" (click)="approve(t)">
                    <mat-icon>check</mat-icon> Approve
                  </button>
                }
                @if (t.status !== 3) {
                  <button mat-stroked-button class="warn" (click)="reject(t)">
                    <mat-icon>block</mat-icon> Reject
                  </button>
                }
                <button mat-icon-button (click)="deleteItem(t)" aria-label="Delete">
                  <mat-icon>delete</mat-icon>
                </button>
              </span>
            </div>
          </div>
        }
      </div>

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
    .chips { display: flex; gap: 8px; margin-bottom: 16px; }
    .chip {
      border: 1px solid rgba(0,0,0,0.15); background: #fff; padding: 6px 16px;
      border-radius: 999px; cursor: pointer; font-weight: 500;
    }
    .chip.active { background: #512bd4; color: #fff; border-color: transparent; }
    .center { display: grid; place-items: center; padding: 48px; }
    .empty { color: rgba(0,0,0,0.55); padding: 24px 0; }
    .cards { display: grid; gap: 12px; }
    .card {
      background: #fff; border: 1px solid rgba(0,0,0,0.08); border-radius: 12px; padding: 16px 18px;
    }
    .card.pending { border-left: 4px solid #f59e0b; }
    .head { display: flex; justify-content: space-between; align-items: flex-start; gap: 12px; }
    .head strong { display: block; }
    .muted { color: rgba(0,0,0,0.55); font-size: 0.85rem; }
    .badge { font-size: 0.72rem; font-weight: 700; padding: 3px 10px; border-radius: 999px; }
    .badge.s1 { background: rgba(245,158,11,0.15); color: #b45309; }
    .badge.s2 { background: rgba(22,163,74,0.15); color: #15803d; }
    .badge.s3 { background: rgba(220,38,38,0.12); color: #b91c1c; }
    .stars { color: #f5a623; margin-top: 6px; }
    .stars mat-icon { font-size: 16px; height: 16px; width: 16px; }
    .content { margin: 8px 0; line-height: 1.55; }
    .foot { display: flex; justify-content: space-between; align-items: center; gap: 8px; flex-wrap: wrap; }
    .actions { display: flex; gap: 8px; align-items: center; }
    .ok { color: #15803d !important; }
    .warn { color: #b45309 !important; }
  `
})
export class TestimonialsListComponent {
  private readonly api = inject(TestimonialsApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly filters = [
    { label: 'Pending', value: 1 as number | undefined },
    { label: 'Approved', value: 2 as number | undefined },
    { label: 'Rejected', value: 3 as number | undefined },
    { label: 'All', value: undefined as number | undefined }
  ];

  readonly loading = signal(false);
  readonly items = signal<Testimonial[]>([]);
  readonly status = signal<number | undefined>(1);
  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalRecords = signal(0);

  constructor() {
    this.load();
  }

  stars(rating: number): number[] {
    return Array.from({ length: rating }, (_, i) => i);
  }

  setStatus(value: number | undefined): void {
    this.status.set(value);
    this.pageNumber.set(1);
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.api
      .getPaged({
        portfolioProfileId: this.portfolio.profileId(),
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize(),
        status: this.status()
      })
      .subscribe({
        next: (r) => {
          this.items.set(r.items);
          this.totalRecords.set(r.totalRecords);
          this.loading.set(false);
        },
        error: (err: Error) => {
          this.snackBar.open(err.message ?? 'Failed to load', 'Close', { duration: 5000 });
          this.loading.set(false);
        }
      });
  }

  onPage(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.load();
  }

  approve(t: Testimonial): void {
    this.api.approve(t.testimonialId).subscribe({
      next: () => {
        this.snackBar.open('Testimonial approved and published', 'Close', { duration: 3000 });
        this.load();
      },
      error: (err: Error) => this.snackBar.open(err.message ?? 'Approve failed', 'Close', { duration: 5000 })
    });
  }

  reject(t: Testimonial): void {
    this.api.reject(t.testimonialId).subscribe({
      next: () => {
        this.snackBar.open('Testimonial rejected', 'Close', { duration: 3000 });
        this.load();
      },
      error: (err: Error) => this.snackBar.open(err.message ?? 'Reject failed', 'Close', { duration: 5000 })
    });
  }

  deleteItem(t: Testimonial): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Delete Testimonial', message: `Delete the testimonial from "${t.authorName}"?` }
    });
    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) return;
      this.api.delete(t.testimonialId).subscribe({
        next: () => {
          this.snackBar.open('Testimonial deleted', 'Close', { duration: 3000 });
          this.load();
        },
        error: (err: Error) => this.snackBar.open(err.message ?? 'Delete failed', 'Close', { duration: 5000 })
      });
    });
  }
}
