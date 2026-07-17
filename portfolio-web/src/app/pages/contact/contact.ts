import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { SITE } from '../../core/content/site-content';

@Component({
  selector: 'app-contact',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <section class="page">
      <h1 class="page-title">Contact</h1>
      <p class="page-subtitle">Have a question or an opportunity? Send a message.</p>

      <div class="contact-grid">
        <form class="form" [formGroup]="form" (ngSubmit)="submit()">
          <mat-form-field appearance="outline">
            <mat-label>Your name</mat-label>
            <input matInput formControlName="name" autocomplete="name" />
            @if (form.controls.name.touched && form.controls.name.invalid) {
              <mat-error>Name is required.</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Email</mat-label>
            <input matInput type="email" formControlName="email" autocomplete="email" />
            @if (form.controls.email.touched && form.controls.email.invalid) {
              <mat-error>Enter a valid email.</mat-error>
            }
          </mat-form-field>

          <mat-form-field appearance="outline">
            <mat-label>Message</mat-label>
            <textarea matInput rows="6" formControlName="message"></textarea>
            @if (form.controls.message.touched && form.controls.message.invalid) {
              <mat-error>Please write a short message.</mat-error>
            }
          </mat-form-field>

          <button mat-flat-button color="primary" type="submit">
            <mat-icon>send</mat-icon> Send message
          </button>
          <p class="hint">This opens your email client addressed to {{ site.email }}.</p>
        </form>

        <aside class="info">
          <h2>Reach me directly</h2>
          <a class="info-row" [href]="'mailto:' + site.email">
            <mat-icon>mail</mat-icon><span>{{ site.email }}</span>
          </a>
          <div class="info-row"><mat-icon>place</mat-icon><span>{{ site.location }}</span></div>
          <div class="socials">
            @for (social of site.socials; track social.label) {
              <a mat-stroked-button [href]="social.url" target="_blank" rel="noopener">
                <mat-icon>{{ social.icon }}</mat-icon> {{ social.label }}
              </a>
            }
          </div>
        </aside>
      </div>
    </section>
  `,
  styles: `
    .contact-grid { display: grid; gap: 28px; grid-template-columns: 1fr; }
    .form { display: flex; flex-direction: column; }
    .form mat-form-field { width: 100%; }
    .form button { align-self: flex-start; }
    .hint { color: var(--mat-sys-on-surface-variant, #5f6368); font-size: 0.82rem; margin: 12px 0 0; }
    .info {
      background: #fff; border: 1px solid rgba(0,0,0,0.06); border-radius: 16px; padding: 24px; align-self: start;
      h2 { margin: 0 0 16px; font-size: 1.15rem; }
    }
    .info-row { display: flex; align-items: center; gap: 10px; padding: 8px 0; color: var(--mat-sys-on-surface, #26303b); }
    .info-row mat-icon { color: var(--brand); }
    .socials { display: flex; flex-wrap: wrap; gap: 10px; margin-top: 16px; }
    @media (min-width: 900px) {
      .contact-grid { grid-template-columns: 1.4fr 1fr; }
    }
  `
})
export class ContactPage {
  private readonly fb = inject(FormBuilder);
  readonly site = SITE;

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    message: ['', [Validators.required, Validators.minLength(10)]]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const { name, email, message } = this.form.getRawValue();
    const subject = encodeURIComponent(`Portfolio enquiry from ${name}`);
    const body = encodeURIComponent(`${message}\n\n— ${name} (${email})`);
    window.location.href = `mailto:${this.site.email}?subject=${subject}&body=${body}`;
  }
}
