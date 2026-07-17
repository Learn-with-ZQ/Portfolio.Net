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
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-deploy-details-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './deploy-details-form.component.html',
  styleUrl: './deploy-details-form.component.scss'
})
export class DeployDetailsFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(DeployDetailApiService);
  private readonly companyApi = inject(CompanyApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);
  readonly companies = signal<LookupItem[]>([]);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    deployDetailName: ['', Validators.required],
    deployedTo: ['', Validators.required],
    deployedByCompanyId: [null as number | null]
  });

  ngOnInit(): void {
    this.companyApi.lookup().subscribe({ next: (items) => this.companies.set(items) });

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
          deployDetailName: detail.deployDetailName,
          deployedTo: detail.deployedTo,
          deployedByCompanyId: detail.deployedByCompanyId ?? null
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load deploy detail', 'Close', { duration: 5000 });
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
      deployDetailName: v.deployDetailName,
      deployedTo: v.deployedTo,
      deployedByCompanyId: v.deployedByCompanyId ?? undefined
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, deployDetailId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Deploy detail updated' : 'Deploy detail created', 'Close', {
          duration: 3000
        });
        void this.router.navigate(['/deploy-details']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/deploy-details']);
  }
}
