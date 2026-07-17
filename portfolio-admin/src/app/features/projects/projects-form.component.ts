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
import { map, switchMap } from 'rxjs';
import { LookupItem } from '../../core/models/lookup.models';
import { CourseApiService } from '../../core/services/course.service';
import { CompanyApiService } from '../../core/services/lookup-services';
import { ProjectsApiService } from '../../core/services/module-services';
import { TechnologyApiService } from '../../core/services/technology.service';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-projects-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './projects-form.component.html',
  styleUrl: './projects-form.component.scss'
})
export class ProjectsFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(ProjectsApiService);
  private readonly companyApi = inject(CompanyApiService);
  private readonly courseApi = inject(CourseApiService);
  private readonly technologyApi = inject(TechnologyApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);
  readonly companies = signal<LookupItem[]>([]);
  readonly courses = signal<LookupItem[]>([]);
  readonly technologies = signal<LookupItem[]>([]);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    projectName: ['', Validators.required],
    projectSummary: [''],
    practice: [''],
    companyId: [null as number | null],
    courseId: [null as number | null],
    technologyIds: [[] as number[]],
    startDate: ['', Validators.required],
    endDate: [''],
    sortOrder: [0]
  });

  ngOnInit(): void {
    this.companyApi.lookup().subscribe({ next: (items) => this.companies.set(items) });
    this.courseApi.lookup().subscribe({ next: (items) => this.courses.set(items) });
    this.technologyApi.lookup().subscribe({ next: (items) => this.technologies.set(items) });

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
          projectName: detail.projectName,
          projectSummary: detail.projectSummary ?? '',
          practice: detail.practice ?? '',
          companyId: detail.companyId ?? null,
          courseId: detail.courseId ?? null,
          technologyIds: detail.technologies?.map((t) => t.technologyId) ?? [],
          startDate: this.formatDate(detail.startDate),
          endDate: detail.endDate ? this.formatDate(detail.endDate) : '',
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load project', 'Close', { duration: 5000 });
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
      projectName: v.projectName,
      projectSummary: v.projectSummary || undefined,
      practice: v.practice || undefined,
      companyId: v.companyId ?? undefined,
      courseId: v.courseId ?? undefined,
      startDate: v.startDate,
      endDate: v.endDate || undefined,
      sortOrder: v.sortOrder
    };

    // On create the technologies are inserted as part of the request; on edit
    // they are reconciled through the dedicated sync endpoint after the update.
    const request$ = this.isEdit()
      ? this.api
          .update(this.entityId!, { ...base, projectId: this.entityId!, rowVersion: this.rowVersion })
          .pipe(switchMap(() => this.api.syncTechnologies(this.entityId!, v.technologyIds)))
      : this.api.create({ ...base, technologyIds: v.technologyIds }).pipe(map(() => undefined));

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Project updated' : 'Project created', 'Close', { duration: 3000 });
        void this.router.navigate(['/projects']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/projects']);
  }
}
