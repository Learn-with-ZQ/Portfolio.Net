import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ParagraphsApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-paragraphs-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  template: `
    @if (loading()) {
      <div class="center"><mat-spinner diameter="40" /></div>
    } @else {
      <mat-card>
        <mat-card-header>
          <mat-card-title>{{ isEdit() ? 'Edit Content Block' : 'New Content Block' }}</mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="save()" class="form">
            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Type</mat-label>
                <mat-select formControlName="paragraphTypeId">
                  @for (t of types; track t.id) { <mat-option [value]="t.id">{{ t.name }}</mat-option> }
                </mat-select>
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Title (optional)</mat-label>
                <input matInput formControlName="paragraphTitle" />
              </mat-form-field>
            </div>

            <mat-form-field appearance="outline" class="full">
              <mat-label>Text</mat-label>
              <textarea matInput rows="8" formControlName="paragraphText"></textarea>
            </mat-form-field>

            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Sort order</mat-label>
                <input matInput type="number" formControlName="sortOrder" />
              </mat-form-field>
            </div>

            <mat-checkbox formControlName="isActive">Active (shown on the site)</mat-checkbox>
          </form>
        </mat-card-content>

        <mat-card-actions align="end">
          <button mat-button type="button" (click)="cancel()">Cancel</button>
          <button mat-flat-button color="primary" [disabled]="saving()" (click)="save()">
            {{ saving() ? 'Saving…' : 'Save' }}
          </button>
        </mat-card-actions>
      </mat-card>
    }
  `,
  styles: `
    .center { display: grid; place-items: center; padding: 48px; }
    .form { display: flex; flex-direction: column; }
    .full { width: 100%; }
    .row { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 0 16px; }
  `
})
export class ParagraphsFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ParagraphsApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  /** Matches dbo.ParagraphType seed data. */
  readonly types = [
    { id: 1, name: 'Header' },
    { id: 2, name: 'DevNote' },
    { id: 3, name: 'About' },
    { id: 4, name: 'Footer' }
  ];

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    paragraphTypeId: [3, Validators.required],
    paragraphTitle: [''],
    paragraphText: ['', Validators.required],
    sortOrder: [0],
    isActive: [true]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.entityId = +id;
      this.loadEntity(this.entityId);
    }
  }

  private loadEntity(id: number): void {
    this.loading.set(true);
    this.api.getById(id).subscribe({
      next: (detail) => {
        this.rowVersion = normalizeRowVersion(detail.rowVersion);
        this.form.patchValue({
          paragraphTypeId: detail.paragraphTypeId,
          paragraphTitle: detail.paragraphTitle ?? '',
          paragraphText: detail.paragraphText,
          sortOrder: detail.sortOrder,
          isActive: detail.isActive
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load block', 'Close', { duration: 5000 });
        this.loading.set(false);
      }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const v = this.form.getRawValue();
    const base = {
      portfolioProfileId: this.portfolio.profileId(),
      paragraphTypeId: v.paragraphTypeId,
      paragraphTitle: v.paragraphTitle || undefined,
      paragraphText: v.paragraphText,
      sortOrder: v.sortOrder,
      isActive: v.isActive
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, paragraphId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Block updated' : 'Block created', 'Close', { duration: 3000 });
        void this.router.navigate(['/content']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/content']);
  }
}
