import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupItem } from '../../core/models/lookup.models';
import { DegreeApiService } from '../../core/services/degree.service';
import { DegreeLevelApiService } from '../../core/services/degree-level.service';
import { InstituteApiService } from '../../core/services/institute.service';
import { EducationApiService } from '../../core/services/module-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-education-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './education-form.component.html',
  styleUrl: './education-form.component.scss'
})
export class EducationFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(EducationApiService);
  private readonly instituteApi = inject(InstituteApiService);
  private readonly degreeApi = inject(DegreeApiService);
  private readonly degreeLevelApi = inject(DegreeLevelApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);
  readonly institutes = signal<LookupItem[]>([]);
  readonly degrees = signal<LookupItem[]>([]);
  readonly degreeLevels = signal<LookupItem[]>([]);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    degreeLevelId: [0, [Validators.required, Validators.min(1)]],
    degreeId: [0, [Validators.required, Validators.min(1)]],
    instituteId: [0, [Validators.required, Validators.min(1)]],
    startDate: ['', Validators.required],
    endDate: [''],
    gpa: [null as number | null],
    cgpa: [null as number | null],
    sortOrder: [0]
  });

  ngOnInit(): void {
    this.instituteApi.lookup().subscribe({ next: (items) => this.institutes.set(items) });
    this.degreeApi.lookup().subscribe({ next: (items) => this.degrees.set(items) });
    this.degreeLevelApi.lookup().subscribe({ next: (items) => this.degreeLevels.set(items) });

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
          degreeLevelId: detail.degreeLevelId,
          degreeId: detail.degreeId,
          instituteId: detail.instituteId,
          startDate: this.formatDate(detail.startDate),
          endDate: detail.endDate ? this.formatDate(detail.endDate) : '',
          gpa: detail.gpa ?? null,
          cgpa: detail.cgpa ?? null,
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load education', 'Close', { duration: 5000 });
        this.loading.set(false);
      }
    });
  }

  private formatDate(value: string): string {
    return value?.substring(0, 10) ?? '';
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
      degreeLevelId: v.degreeLevelId,
      degreeId: v.degreeId,
      instituteId: v.instituteId,
      startDate: v.startDate,
      endDate: v.endDate || undefined,
      gpa: v.gpa ?? undefined,
      cgpa: v.cgpa ?? undefined,
      sortOrder: v.sortOrder
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, educationId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Education updated' : 'Education created', 'Close', {
          duration: 3000
        });
        void this.router.navigate(['/education']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/education']);
  }
}
