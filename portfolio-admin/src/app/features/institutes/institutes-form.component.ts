import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { InstituteApiService } from '../../core/services/institute.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-institutes-form',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './institutes-form.component.html',
  styleUrl: './institutes-form.component.scss'
})
export class InstitutesFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(InstituteApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    instituteName: ['', Validators.required],
    location: ['']
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
          instituteName: detail.instituteName,
          location: detail.location ?? ''
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load institute', 'Close', { duration: 5000 });
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
      instituteName: v.instituteName,
      location: v.location || undefined
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, instituteId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Institute updated' : 'Institute created', 'Close', { duration: 3000 });
        void this.router.navigate(['/institutes']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/institutes']);
  }
}
