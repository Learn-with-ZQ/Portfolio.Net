import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { PortfolioContextService } from '../../services/portfolio-context.service';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-admin-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    ReactiveFormsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss'
})
export class AdminLayoutComponent {
  private readonly auth = inject(AuthService);
  private readonly fb = inject(FormBuilder);
  readonly portfolio = inject(PortfolioContextService);

  readonly opened = signal(true);

  readonly navItems: NavItem[] = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
    { label: 'Experience', icon: 'work', route: '/experience' },
    { label: 'Projects', icon: 'folder', route: '/projects' },
    { label: 'Education', icon: 'school', route: '/education' },
    { label: 'Skills', icon: 'psychology', route: '/skills' },
    { label: 'Awards', icon: 'emoji_events', route: '/awards' },
    { label: 'Certifications', icon: 'verified', route: '/certifications' },
    { label: 'Documents', icon: 'description', route: '/documents' },
    { label: 'Testimonials', icon: 'reviews', route: '/testimonials' },
    { label: 'References', icon: 'groups', route: '/references' },
    { label: 'Blog', icon: 'article', route: '/blog' },
    { label: 'Content', icon: 'notes', route: '/content' },
    { label: 'Companies', icon: 'business', route: '/companies' },
    { label: 'Deploy Details', icon: 'cloud_upload', route: '/deploy-details' },
    { label: 'Institutes', icon: 'account_balance', route: '/institutes' },
    { label: 'Degrees', icon: 'workspace_premium', route: '/degrees' },
    { label: 'Degree Levels', icon: 'stairs', route: '/degree-levels' },
    { label: 'Technologies', icon: 'memory', route: '/technologies' },
    { label: 'Cert Issuers', icon: 'verified_user', route: '/certification-issuers' },
    { label: 'Document Types', icon: 'category', route: '/document-types' },
    { label: 'Courses', icon: 'menu_book', route: '/courses' },
    { label: 'Analytics', icon: 'insights', route: '/analytics' }
  ];

  readonly profileForm = this.fb.nonNullable.group({
    profileId: [this.portfolio.profileId()]
  });

  applyProfile(): void {
    this.portfolio.setProfileId(this.profileForm.controls.profileId.value);
  }

  logout(): void {
    this.auth.logout().subscribe();
  }
}
