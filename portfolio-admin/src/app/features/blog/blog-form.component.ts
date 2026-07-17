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
import { BlogApiService } from '../../core/services/platform-services';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';
import { normalizeRowVersion } from '../../shared/utils/row-version.util';

const SLUG_PATTERN = /^[a-z0-9]+(?:-[a-z0-9]+)*$/;

@Component({
  selector: 'app-blog-form',
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
          <mat-card-title>{{ isEdit() ? 'Edit Post' : 'New Post' }}</mat-card-title>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="save()" class="form">
            <mat-form-field appearance="outline" class="full">
              <mat-label>Title</mat-label>
              <input matInput formControlName="title" (blur)="suggestSlug()" />
            </mat-form-field>

            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Slug</mat-label>
                <input matInput formControlName="slug" />
                <mat-hint>lowercase-hyphen-separated</mat-hint>
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Category</mat-label>
                <mat-select formControlName="category">
                  @for (c of categories; track c) { <mat-option [value]="c">{{ c }}</mat-option> }
                </mat-select>
              </mat-form-field>
            </div>

            <mat-form-field appearance="outline" class="full">
              <mat-label>Summary</mat-label>
              <textarea matInput rows="2" formControlName="summary"></textarea>
            </mat-form-field>

            <mat-form-field appearance="outline" class="full">
              <mat-label>Content (Markdown)</mat-label>
              <textarea matInput rows="16" formControlName="contentMarkdown" class="mono"></textarea>
            </mat-form-field>

            <div class="row">
              <mat-form-field appearance="outline">
                <mat-label>Tags (comma-separated)</mat-label>
                <input matInput formControlName="tags" />
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Read time (minutes)</mat-label>
                <input matInput type="number" formControlName="readTimeMinutes" />
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Sort Order</mat-label>
                <input matInput type="number" formControlName="sortOrder" />
              </mat-form-field>
            </div>

            @if (!isEdit()) {
              <mat-checkbox formControlName="isPublished">Publish immediately</mat-checkbox>
            }
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
    .row { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 0 16px; }
    .mono { font-family: 'Cascadia Code', Consolas, monospace; font-size: 0.9rem; }
  `
})
export class BlogFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly api = inject(BlogApiService);
  private readonly portfolio = inject(PortfolioContextService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  readonly categories = ['.NET', 'Angular', 'SQL Server', 'System Design', 'Data Science', 'Architecture'];

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEdit = signal(false);

  private entityId?: number;
  private rowVersion = '';

  readonly form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    slug: ['', [Validators.required, Validators.pattern(SLUG_PATTERN)]],
    summary: [''],
    contentMarkdown: ['', Validators.required],
    category: [''],
    tags: [''],
    readTimeMinutes: [null as number | null],
    sortOrder: [0],
    isPublished: [false]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit.set(true);
      this.entityId = +id;
      this.loadEntity(this.entityId);
    }
  }

  suggestSlug(): void {
    if (this.form.controls.slug.value || !this.form.controls.title.value) return;
    const slug = this.form.controls.title.value
      .toLowerCase()
      .replace(/[^a-z0-9\s-]/g, '')
      .trim()
      .replace(/\s+/g, '-');
    this.form.controls.slug.setValue(slug);
  }

  private loadEntity(id: number): void {
    this.loading.set(true);
    this.api.getById(id).subscribe({
      next: (detail) => {
        this.rowVersion = normalizeRowVersion(detail.rowVersion);
        this.form.patchValue({
          title: detail.title,
          slug: detail.slug,
          summary: detail.summary ?? '',
          contentMarkdown: detail.contentMarkdown,
          category: detail.category ?? '',
          tags: detail.tags ?? '',
          readTimeMinutes: detail.readTimeMinutes ?? null,
          sortOrder: detail.sortOrder
        });
        this.loading.set(false);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Failed to load post', 'Close', { duration: 5000 });
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
      title: v.title,
      slug: v.slug,
      summary: v.summary || undefined,
      contentMarkdown: v.contentMarkdown,
      category: v.category || undefined,
      tags: v.tags || undefined,
      readTimeMinutes: v.readTimeMinutes ?? undefined,
      sortOrder: v.sortOrder
    };

    const request$ = this.isEdit()
      ? this.api.update(this.entityId!, { ...base, blogPostId: this.entityId!, rowVersion: this.rowVersion })
      : this.api.create({ ...base, isPublished: v.isPublished });

    request$.subscribe({
      next: () => {
        this.snackBar.open(this.isEdit() ? 'Post updated' : 'Post created', 'Close', { duration: 3000 });
        void this.router.navigate(['/blog']);
      },
      error: (err: Error) => {
        this.snackBar.open(err.message ?? 'Save failed', 'Close', { duration: 5000 });
        this.saving.set(false);
      }
    });
  }

  cancel(): void {
    void this.router.navigate(['/blog']);
  }
}
