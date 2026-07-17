import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { CertificationIssuerApiService } from '../../core/services/certification-issuer.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-certification-issuers-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './certification-issuers-form.component.html',
  styleUrl: './certification-issuers-form.component.scss'
})
export class CertificationIssuersFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(CertificationIssuerApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    issuerName: ['', Validators.required],
    issuerWebsite: ['']
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
          issuerName: detail.issuerName,
          issuerWebsite: detail.issuerWebsite ?? ''
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load certification issuer', 'Close', { duration: 5000 });
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
      issuerName: v.issuerName,
      issuerWebsite: v.issuerWebsite || undefined
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, certificationIssuerId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Certification issuer updated' : 'Certification issuer created', 'Close', { duration: 3000 });
        void this.router.navigate(['/certification-issuers']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/certification-issuers']);
  }
}
