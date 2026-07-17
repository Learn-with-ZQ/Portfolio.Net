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
import { ReferencesApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-references-form',
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
          <mat-card-title>{{ isEdit() ? 'Edit Reference' : 'New Reference' }}</mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="save()" class="form">
            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Full name</mat-label>
                <input matInput formControlName="fullName" />
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Relationship</mat-label>
                <mat-select formControlName="relationship">
                  @for (r of relationships; track r) { <mat-option [value]="r">{{ r }}</mat-option> }
                </mat-select>
              </mat-form-field>
            </div>

            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Organization</mat-label>
                <input matInput formControlName="organization" />
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Designation</mat-label>
                <input matInput formControlName="designation" />
              </mat-form-field>
            </div>

            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Email</mat-label>
                <input matInput type="email" formControlName="email" />
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Phone</mat-label>
                <input matInput formControlName="phone" />
              </mat-form-field>
            </div>

            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>LinkedIn URL</mat-label>
                <input matInput formControlName="linkedInUrl" />
              </mat-form-field>
              <mat-form-field appearance="outline">
                <mat-label>Sort order</mat-label>
                <input matInput type="number" formControlName="sortOrder" />
              </mat-form-field>
            </div>

            <div class="checks">
              <mat-checkbox formControlName="isPublic">Show on public site</mat-checkbox>
              <mat-checkbox formControlName="isContactVisible">
                Contact details publicly visible (with the reference's permission)
              </mat-checkbox>
            </div>
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
    .row { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 0 16px; }
    .checks { display: flex; flex-direction: column; gap: 8px; margin-top: 4px; }
  `
})
export class ReferencesFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ReferencesApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly relationships = ['Manager', 'Team Lead', 'Professor', 'Mentor', 'Client', 'Colleague'];

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    fullName: ['', Validators.required],
    organization: [''],
    designation: [''],
    relationship: [''],
    email: ['', Validators.email],
    phone: [''],
    linkedInUrl: [''],
    isContactVisible: [false],
    isPublic: [true],
    sortOrder: [0]
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
          fullName: detail.fullName,
          organization: detail.organization ?? '',
          designation: detail.designation ?? '',
          relationship: detail.relationship ?? '',
          email: detail.email ?? '',
          phone: detail.phone ?? '',
          linkedInUrl: detail.linkedInUrl ?? '',
          isContactVisible: detail.isContactVisible,
          isPublic: detail.isPublic,
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load reference', 'Close', { duration: 5000 });
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
      fullName: v.fullName,
      organization: v.organization || undefined,
      designation: v.designation || undefined,
      relationship: v.relationship || undefined,
      email: v.email || undefined,
      phone: v.phone || undefined,
      linkedInUrl: v.linkedInUrl || undefined,
      isContactVisible: v.isContactVisible,
      isPublic: v.isPublic,
      sortOrder: v.sortOrder
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, referenceId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Reference updated' : 'Reference created', 'Close', { duration: 3000 });
        void this.router.navigate(['/references']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/references']);
  }
}
