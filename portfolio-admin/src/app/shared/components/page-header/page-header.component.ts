import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-page-header',
  imports: [MatButtonModule, MatIconModule, RouterLink],
  template: `
    <div class="page-header">
      <div>
        <h1>{{ title() }}</h1>
        @if (subtitle()) {
          <p>{{ subtitle() }}</p>
        }
      </div>
      @if (actionLabel() && actionLink()) {
        <a mat-flat-button color="primary" [routerLink]="actionLink()">
          <mat-icon>add</mat-icon>
          {{ actionLabel() }}
        </a>
      }
    </div>
  `,
  styles: `
    .page-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    h1 {
      margin: 0;
      font-size: 1.75rem;
      font-weight: 600;
    }

    p {
      margin: 0.25rem 0 0;
      color: rgba(0, 0, 0, 0.6);
    }
  `
})
export class PageHeaderComponent {
  readonly title = input.required<string>();
  readonly subtitle = input<string>();
  readonly actionLabel = input<string>();
  readonly actionLink = input<string>();
}
