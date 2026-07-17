import { Component, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { debounceTime, Subject } from 'rxjs';
import { PagedResult } from '../../core/models/api.models';
import { Degree } from '../../core/models/degree.models';
import { DegreeApiService } from '../../core/services/degree.service';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { ConfirmDialogComponent } from '../../shared/components/confirm-dialog/confirm-dialog.component';
import { PageHeaderComponent } from '../../shared/components/page-header/page-header.component';

@Component({
  selector: 'app-degrees-list',
  imports: [
    RouterLink,
    PageHeaderComponent,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './degrees-list.component.html',
  styleUrl: './degrees-list.component.scss'
})
export class DegreesListComponent {
  private readonly api = inject(DegreeApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroyRef = inject(DestroyRef);
  private readonly searchSubject = new Subject<string>();

  readonly loading = signal(false);
  readonly items = signal<Degree[]>([]);
  readonly error = signal<string | null>(null);
  readonly searchTerm = signal('');
  readonly pageNumber = signal(1);
  readonly pageSize = signal(10);
  readonly totalRecords = signal(0);

  readonly displayedColumns = ['degreeName', 'actions'];

  constructor() {
    this.searchSubject.pipe(debounceTime(300), takeUntilDestroyed(this.destroyRef)).subscribe((term) => {
      this.searchTerm.set(term);
      this.pageNumber.set(1);
      this.load();
    });
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.api
      .getPaged({
        portfolioProfileId: this.portfolio.profileId(),
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize(),
        searchTerm: this.searchTerm() || undefined
      })
      .subscribe({
        next: (result: PagedResult<Degree>) => {
          this.items.set(result.items);
          this.totalRecords.set(result.totalRecords);
          this.loading.set(false);
        },
        error: (err: Error) => {
          this.error.set(err.message ?? 'Failed to load degrees');
          this.loading.set(false);
        }
      });
  }

  onSearch(value: string): void {
    this.searchSubject.next(value);
  }

  onPage(event: PageEvent): void {
    this.pageNumber.set(event.pageIndex + 1);
    this.pageSize.set(event.pageSize);
    this.load();
  }

  deleteItem(item: Degree): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Degree',
        message: `Are you sure you want to delete "${item.degreeName}"?`
      }
    });

    ref.afterClosed().subscribe((confirmed) => {
      if (!confirmed) {
        return;
      }

      this.api.delete(item.degreeId).subscribe({
        next: () => {
          this.snackBar.open('Degree deleted', 'Close', { duration: 3000 });
          this.load();
        },
        error: (err: Error) => {
          this.snackBar.open(err.message ?? 'Delete failed', 'Close', { duration: 5000 });
        }
      });
    });
  }
}
