import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { Reference } from '../../core/models/portfolio.models';
import { ReferencesApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-references-list',
  imports: [
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
      title="References"
      subtitle="Professional references and contact visibility"
      actionLabel="New Reference"
      actionLink="new"
    />

    @if (loading()) {
      <div class="center"><mat-spinner diameter="40" /></div>
    } @else {
      <table mat-table [dataSource]="items()" class="mat-elevation-z1">
        <ng-container matColumnDef="fullName">
          <th mat-header-cell *matHeaderCellDef>Name</th>
          <td mat-cell *matCellDef="let r">
            <strong>{{ r.fullName }}</strong>
            <div class="muted">{{ r.designation }}@if (r.designation && r.organization) {, }{{ r.organization }}</div>
          </td>
        </ng-container>

        <ng-container matColumnDef="relationship">
          <th mat-header-cell *matHeaderCellDef>Relationship</th>
          <td mat-cell *matCellDef="let r">{{ r.relationship || '—' }}</td>
        </ng-container>

        <ng-container matColumnDef="contact">
          <th mat-header-cell *matHeaderCellDef>Contact visible</th>
          <td mat-cell *matCellDef="let r">
            <mat-icon [class.ok]="r.isContactVisible" [class.off]="!r.isContactVisible">
              {{ r.isContactVisible ? 'visibility' : 'visibility_off' }}
            </mat-icon>
          </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef></th>
          <td mat-cell *matCellDef="let r" class="row-actions">
            <a mat-icon-button [routerLink]="[r.referenceId, 'edit']" aria-label="Edit"><mat-icon>edit</mat-icon></a>
            <button mat-icon-button (click)="deleteItem(r)" aria-label="Delete"><mat-icon>delete</mat-icon></button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
      </table>

      @if (!items().length) {
        <p class="empty">No references yet.</p>
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
    .ok { color: #15803d; }
    .off { color: rgba(0,0,0,0.35); }
  `
})
export class ReferencesListComponent {
  private readonly api = inject(ReferencesApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly items = signal<Reference[]>([]);
  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalRecords = signal(0);

  readonly displayedColumns = ['fullName', 'relationship', 'contact', 'actions'];

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
          this.snackBar.open(err.message ?? 'Failed to load references', 'Close', { duration: 5000 });
          this.loading.set(false);
        }
      });
  }

  onPage(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.load();
  }

  deleteItem(r: Reference): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: { title: 'Delete Reference', message: `Delete "${r.fullName}"?` }
    });
    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) return;
      this.api.delete(r.referenceId).subscribe({
        next: () => {
          this.snackBar.open('Reference deleted', 'Close', { duration: 3000 });
          this.load();
        },
        error: (err: Error) => this.snackBar.open(err.message ?? 'Delete failed', 'Close', { duration: 5000 })
      });
    });
  }
}
