import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { BlogPost } from '../../core/models/portfolio.models';
import { BlogApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-blog-list',
  imports: [
    DatePipe,
    RouterLink,
    PageHeaderComponent,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule,
    MatProgressSpinnerModule
  ],
  template: `
    <app-page-header
      title="Blog"
      subtitle="Write, publish and manage posts"
      actionLabel="New Post"
      actionLink="new"
    />

    @if (loading()) {
      <div class="center"><mat-spinner diameter="40" /></div>
    } @else {
      <table mat-table [dataSource]="items()" class="mat-elevation-z1">
        <ng-container matColumnDef="title">
          <th mat-header-cell *matHeaderCellDef>Title</th>
          <td mat-cell *matCellDef="let p">
            <strong>{{ p.title }}</strong>
            <div class="muted">/{{ p.slug }}@if (p.category) { · {{ p.category }} }</div>
          </td>
        </ng-container>

        <ng-container matColumnDef="publishedAt">
          <th mat-header-cell *matHeaderCellDef>Published</th>
          <td mat-cell *matCellDef="let p">{{ p.publishedAt ? (p.publishedAt | date: 'mediumDate') : '—' }}</td>
        </ng-container>

        <ng-container matColumnDef="isPublished">
          <th mat-header-cell *matHeaderCellDef>Live</th>
          <td mat-cell *matCellDef="let p">
            <mat-slide-toggle [checked]="p.isPublished" (change)="togglePublish(p)" />
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef></th>
          <td mat-cell *matCellDef="let p" class="row-actions">
            <a mat-icon-button [routerLink]="[p.blogPostId, 'edit']" aria-label="Edit"><mat-icon>edit</mat-icon></a>
            <button mat-icon-button (click)="deleteItem(p)" aria-label="Delete"><mat-icon>delete</mat-icon></button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
      </table>

      @if (!items().length) {
        <p class="empty">No posts yet — write the first one.</p>
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
    table { width: 100%; }
    .center { display: grid; place-items: center; padding: 48px; }
    .muted { color: rgba(0,0,0,0.55); font-size: 0.82rem; }
    .empty { color: rgba(0,0,0,0.55); padding: 24px 0; }
    .row-actions { white-space: nowrap; text-align: right; }
  `
})
export class BlogListComponent {
  private readonly api = inject(BlogApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly items = signal<BlogPost[]>([]);
  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalRecords = signal(0);

  readonly displayedColumns = ['title', 'publishedAt', 'isPublished', 'actions'];

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.api
      .getPaged({
        portfolioProfileId: this.portfolio.profileId(),
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize()
      })
      .subscribe({
        next: (r) => {
          this.items.set(r.items);
          this.totalRecords.set(r.totalRecords);
          this.loading.set(false);
        },
        error: (err: Error) => {
          this.snackBar.open(err.message ?? 'Failed to load posts', 'Close', { duration: 5000 });
          this.loading.set(false);
        }
      });
  }

  onPage(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.load();
  }

  togglePublish(p: BlogPost): void {
    const request$ = p.isPublished ? this.api.unpublish(p.blogPostId) : this.api.publish(p.blogPostId);
    request$.subscribe({
      next: () => {
        this.snackBar.open(p.isPublished ? 'Post unpublished' : 'Post published', 'Close', { duration: 3000 });
        this.load();
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Update failed', 'Close', { duration: 5000 });
        this.load();
      }
    });
  }

  deleteItem(p: BlogPost): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Delete Post', message: `Delete "${p.title}"?` }
    });
    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) return;
      this.api.delete(p.blogPostId).subscribe({
        next: () => {
          this.snackBar.open('Post deleted', 'Close', { duration: 3000 });
          this.load();
        },
        error: (err: Error) => this.snackBar.open(err.message ?? 'Delete failed', 'Close', { duration: 5000 })
      });
    });
  }
}
