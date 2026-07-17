import { SlicePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { Paragraph } from '../../core/models/portfolio.models';
import { ParagraphsApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-paragraphs-list',
  imports: [
    SlicePipe,
    RouterLink,
    PageHeaderComponent,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  template: `
    <app-page-header
      title="Content Blocks"
      subtitle="Career story, about and site copy (paragraphs)"
      actionLabel="New Block"
      actionLink="new"
    />

    @if (loading()) {
      <div class="center"><mat-spinner diameter="40" /></div>
    } @else {
      <table mat-table [dataSource]="items()" class="mat-elevation-z1">
        <ng-container matColumnDef="type">
          <th mat-header-cell *matHeaderCellDef>Type</th>
          <td mat-cell *matCellDef="let p"><span class="type">{{ p.paragraphTypeName }}</span></td>
        </ng-container>

        <ng-container matColumnDef="title">
          <th mat-header-cell *matHeaderCellDef>Title / text</th>
          <td mat-cell *matCellDef="let p">
            <strong>{{ p.paragraphTitle || '(untitled)' }}</strong>
            <div class="muted">{{ p.paragraphText | slice: 0 : 100 }}{{ p.paragraphText.length > 100 ? '…' : '' }}</div>
          </td>
        </ng-container>

        <ng-container matColumnDef="active">
          <th mat-header-cell *matHeaderCellDef>Active</th>
          <td mat-cell *matCellDef="let p">
            <mat-icon [class.ok]="p.isActive" [class.off]="!p.isActive">
              {{ p.isActive ? 'check_circle' : 'pause_circle' }}
            </mat-icon>
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef></th>
          <td mat-cell *matCellDef="let p" class="row-actions">
            <a mat-icon-button [routerLink]="[p.paragraphId, 'edit']" aria-label="Edit"><mat-icon>edit</mat-icon></a>
            <button mat-icon-button (click)="deleteItem(p)" aria-label="Delete"><mat-icon>delete</mat-icon></button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
      </table>

      @if (!items().length) {
        <p class="empty">No content blocks yet.</p>
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
    .type {
      background: rgba(81,43,212,0.1); color: #512bd4; font-size: 0.75rem; font-weight: 700;
      padding: 3px 10px; border-radius: 999px;
    }
    .ok { color: #15803d; }
    .off { color: rgba(0,0,0,0.35); }
  `
})
export class ParagraphsListComponent {
  private readonly api = inject(ParagraphsApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly items = signal<Paragraph[]>([]);
  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalRecords = signal(0);

  readonly displayedColumns = ['type', 'title', 'active', 'actions'];

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
          this.snackBar.open(err.message ?? 'Failed to load content blocks', 'Close', { duration: 5000 });
          this.loading.set(false);
        }
      });
  }

  onPage(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.load();
  }

  deleteItem(p: Paragraph): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Delete Block', message: `Delete "${p.paragraphTitle || p.paragraphTypeName}"?` }
    });
    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) return;
      this.api.delete(p.paragraphId).subscribe({
        next: () => {
          this.snackBar.open('Content block deleted', 'Close', { duration: 3000 });
          this.load();
        },
        error: (err: Error) => this.snackBar.open(err.message ?? 'Delete failed', 'Close', { duration: 5000 })
      });
    });
  }
}
