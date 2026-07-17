import { Component, inject, OnInit, signal } from '@angular/core';
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
import { LookupItem } from '../../core/models/lookup.models';
import { CertificationIssuerApiService } from '../../core/services/certification-issuer.service';
import { CertificationsApiService } from '../../core/services/module-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-certifications-form',
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
  templateUrl: './certifications-form.component.html',
  styleUrl: './certifications-form.component.scss'
})
export class CertificationsFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(CertificationsApiService);
  private readonly issuerApi = inject(CertificationIssuerApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);
  readonly issuers = signal<LookupItem[]>([]);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    certificationIssuerId: [0, [Validators.required, Validators.min(1)]],
    certificationName: ['', Validators.required],
    credentialId: [''],
    credentialUrl: [''],
    issueDate: ['', Validators.required],
    expiryDate: [''],
    doesNotExpire: [false],
    sortOrder: [0]
  });

  ngOnInit(): void {
    this.issuerApi.lookup().subscribe({ next: (items) => this.issuers.set(items) });

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
          certificationIssuerId: detail.certificationIssuerId,
          certificationName: detail.certificationName,
          credentialId: detail.credentialId ?? '',
          credentialUrl: detail.credentialUrl ?? '',
          issueDate: this.formatDate(detail.issueDate),
          expiryDate: detail.expiryDate ? this.formatDate(detail.expiryDate) : '',
          doesNotExpire: detail.doesNotExpire,
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load certification', 'Close', { duration: 5000 });
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
      certificationIssuerId: v.certificationIssuerId,
      certificationName: v.certificationName,
      credentialId: v.credentialId || undefined,
      credentialUrl: v.credentialUrl || undefined,
      issueDate: v.issueDate,
      expiryDate: v.doesNotExpire ? undefined : v.expiryDate || undefined,
      doesNotExpire: v.doesNotExpire,
      sortOrder: v.sortOrder
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, {
          ...base,
          certificationId: this.entityId!,
          rowVersion: this.rowVersion
        })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Certification updated' : 'Certification created', 'Close', {
          duration: 3000
        });
        void this.router.navigate(['/certifications']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/certifications']);
  }
}
