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
import { DocumentTypeApiService } from '../../core/services/document-type.service';
import { DocumentsApiService } from '../../core/services/module-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

@Component({
  selector: 'app-documents-form',
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
  templateUrl: './documents-form.component.html',
  styleUrl: './documents-form.component.scss'
})
export class DocumentsFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(DocumentsApiService);
  private readonly documentTypeApi = inject(DocumentTypeApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);
  readonly documentTypes = signal<LookupItem[]>([]);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    documentTypeId: [0, [Validators.required, Validators.min(1)]],
    documentTitle: ['', Validators.required],
    fileName: ['', Validators.required],
    fileExtension: ['', Validators.required],
    fileSizeBytes: [null as number | null],
    storagePath: ['', Validators.required],
    mimeType: [''],
    isPublic: [false],
    // Checked = view-only (inverts IsDownloadable on save/load).
    preventDownload: [false],
    versionNumber: [1, [Validators.required, Validators.min(1)]],
    sortOrder: [0]
  });

  ngOnInit(): void {
    this.documentTypeApi.lookup().subscribe({ next: (items) => this.documentTypes.set(items) });

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
          documentTypeId: detail.documentTypeId,
          documentTitle: detail.documentTitle,
          fileName: detail.fileName,
          fileExtension: detail.fileExtension,
          fileSizeBytes: detail.fileSizeBytes ?? null,
          storagePath: detail.storagePath,
          mimeType: detail.mimeType ?? '',
          isPublic: detail.isPublic,
          preventDownload: !detail.isDownloadable,
          versionNumber: detail.versionNumber,
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load document', 'Close', { duration: 5000 });
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
      documentTypeId: v.documentTypeId,
      documentTitle: v.documentTitle,
      fileName: v.fileName,
      fileExtension: v.fileExtension,
      fileSizeBytes: v.fileSizeBytes ?? undefined,
      storagePath: v.storagePath,
      mimeType: v.mimeType || undefined,
      isPublic: v.isPublic,
      isDownloadable: !v.preventDownload,
      versionNumber: v.versionNumber,
      sortOrder: v.sortOrder
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, documentId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create(base);

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Document updated' : 'Document created', 'Close', { duration: 3000 });
        void this.router.navigate(['/documents']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/documents']);
  }
}
