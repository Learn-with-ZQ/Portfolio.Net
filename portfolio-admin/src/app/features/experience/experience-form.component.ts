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
import { CompanyApiService, DeployDetailApiService } from '../../core/services/lookup-services';
import { ExperienceApiService } from '../../core/services/module-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-experience-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './experience-form.component.html',
  styleUrl: './experience-form.component.scss'
})
export class ExperienceFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ExperienceApiService);
  private readonly companyApi = inject(CompanyApiService);
  private readonly deployDetailApi = inject(DeployDetailApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);
  readonly companies = signal<LookupItem[]>([]);
  readonly deployDetails = signal<LookupItem[]>([]);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    designation: ['', Validators.required],
    companyId: [null as number | null],
    deployDetailId: [null as number | null],
    startDate: ['', Validators.required],
    endDate: [''],
    sortOrder: [0]
  });

  ngOnInit(): void {
    this.companyApi.lookup().subscribe({ next: (items) => this.companies.set(items) });
    this.deployDetailApi.lookup().subscribe({ next: (items) => this.deployDetails.set(items) });

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
          designation: detail.designation,
          companyId: detail.companyId ?? null,
          deployDetailId: detail.deployDetailId ?? null,
          startDate: this.formatDate(detail.startDate),
          endDate: detail.endDate ? this.formatDate(detail.endDate) : '',
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load experience', 'Close', { duration: 5000 });
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
      designation: v.designation,
      companyId: v.companyId ?? undefined,
      deployDetailId: v.deployDetailId ?? undefined,
      startDate: v.startDate,
      endDate: v.endDate || undefined,
      sortOrder: v.sortOrder
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, experienceId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Experience updated' : 'Experience created', 'Close', {
          duration: 3000
        });
        void this.router.navigate(['/experience']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/experience']);
  }
}
