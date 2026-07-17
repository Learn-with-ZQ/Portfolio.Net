import { Component, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { PortfolioContextService } from '../../core/services/portfolio-context.service';

@Component({
  selector: 'app-dashboard',
  imports: [MatCardModule, MatIconModule, RouterLink],
  template: `
    <section class="dashboard">
      <h1>Dashboard</h1>
      <p>Managing {{ portfolio.label() }}</p>

      <div class="cards">
        @for (item of modules; track item.route) {
          <a [routerLink]="item.route" class="card-link">
            <mat-card>
              <mat-card-header>
                <mat-icon mat-card-avatar>{{ item.icon }}</mat-icon>
                <mat-card-title>{{ item.title }}</mat-card-title>
                <mat-card-subtitle>{{ item.subtitle }}</mat-card-subtitle>
              </mat-card-header>
            </mat-card>
          </a>
        }
      </div>
    </section>
  `,
  styles: `
    .dashboard h1 {
      margin: 0 0 0.25rem;
    }

    .cards {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
      gap: 1rem;
      margin-top: 1.5rem;
    }

    .card-link {
      text-decoration: none;
      color: inherit;
    }
  `
})
export class DashboardComponent {
  readonly portfolio = inject(PortfolioContextService);

  readonly modules = [
    { title: 'Experience', subtitle: 'Work history', icon: 'work', route: '/experience' },
    { title: 'Projects', subtitle: 'Portfolio projects', icon: 'folder', route: '/projects' },
    { title: 'Education', subtitle: 'Academic records', icon: 'school', route: '/education' },
    { title: 'Skills', subtitle: 'Technical skills', icon: 'psychology', route: '/skills' },
    { title: 'Awards', subtitle: 'Achievements', icon: 'emoji_events', route: '/awards' },
    { title: 'Certifications', subtitle: 'Credentials', icon: 'verified', route: '/certifications' },
    { title: 'Documents', subtitle: 'Files & assets', icon: 'description', route: '/documents' }
  ];
}
