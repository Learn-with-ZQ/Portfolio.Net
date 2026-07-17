import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { monthYear } from '../../core/utils/format';

@Component({
  selector: 'app-testimonials',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './testimonials.html',
  styleUrl: './testimonials.scss'
})
export class TestimonialsPage {
  private readonly api = inject(PortfolioApiService);
  private readonly fb = inject(FormBuilder);
  private readonly snack = inject(MatSnackBar);
  readonly monthYear = monthYear;

  readonly state = toState(this.api.getTestimonials());
  readonly submitting = signal(false);
  readonly submitted = signal(false);

  readonly relationships = ['Manager', 'Team Lead', 'Colleague', 'Professor', 'Mentor', 'Client'];

  readonly form = this.fb.nonNullable.group({
    authorName: ['', Validators.required],
    authorTitle: [''],
    authorCompany: [''],
    authorEmail: ['', Validators.email],
    relationship: [''],
    rating: [5],
    content: ['', [Validators.required, Validators.minLength(10)]],
    linkedInUrl: ['']
  });

  stars(rating?: number): number[] {
    return Array.from({ length: rating ?? 0 }, (_, i) => i);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    const v = this.form.getRawValue();
    this.api
      .submitTestimonial({
        authorName: v.authorName,
        authorTitle: v.authorTitle || undefined,
        authorCompany: v.authorCompany || undefined,
        authorEmail: v.authorEmail || undefined,
        relationship: v.relationship || undefined,
        rating: v.rating || undefined,
        content: v.content,
        linkedInUrl: v.linkedInUrl || undefined
      })
      .subscribe({
        next: () => {
          this.submitting.set(false);
          this.submitted.set(true);
          this.form.reset({ rating: 5 });
          this.snack.open('Thank you! Your testimonial is pending approval.', 'Close', { duration: 5000 });
        },
        error: (err: Error) => {
          this.submitting.set(false);
          this.snack.open(err.message ?? 'Could not submit. Please try again.', 'Close', { duration: 5000 });
        }
      });
  }
}
