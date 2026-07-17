import { Routes } from '@angular/router';
import { adminGuard, authGuard } from './core/auth/auth.guard';
import { AdminLayoutComponent } from './core/layout/admin-layout/admin-layout.component';
import { LoginComponent } from './core/layout/login/login.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'experience',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/experience/experience.routes').then((m) => m.EXPERIENCE_ROUTES)
      },
      {
        path: 'projects',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/projects/projects.routes').then((m) => m.PROJECTS_ROUTES)
      },
      {
        path: 'education',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/education/education.routes').then((m) => m.EDUCATION_ROUTES)
      },
      {
        path: 'skills',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/skills/skills.routes').then((m) => m.SKILLS_ROUTES)
      },
      {
        path: 'awards',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/awards/awards.routes').then((m) => m.AWARDS_ROUTES)
      },
      {
        path: 'certifications',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/certifications/certifications.routes').then((m) => m.CERTIFICATIONS_ROUTES)
      },
      {
        path: 'documents',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/documents/documents.routes').then((m) => m.DOCUMENTS_ROUTES)
      },
      {
        path: 'testimonials',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/testimonials/testimonials.routes').then((m) => m.TESTIMONIALS_ROUTES)
      },
      {
        path: 'references',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/references/references.routes').then((m) => m.REFERENCES_ROUTES)
      },
      {
        path: 'blog',
        canActivate: [adminGuard],
        loadChildren: () => import('./features/blog/blog.routes').then((m) => m.BLOG_ROUTES)
      },
      {
        path: 'content',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/paragraphs/paragraphs.routes').then((m) => m.PARAGRAPHS_ROUTES)
      },
      {
        path: 'companies',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/companies/companies.routes').then((m) => m.COMPANIES_ROUTES)
      },
      {
        path: 'deploy-details',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/deploy-details/deploy-details.routes').then((m) => m.DEPLOY_DETAILS_ROUTES)
      },
      {
        path: 'institutes',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/institutes/institutes.routes').then((m) => m.INSTITUTES_ROUTES)
      },
      {
        path: 'degrees',
        canActivate: [adminGuard],
        loadChildren: () => import('./features/degrees/degrees.routes').then((m) => m.DEGREES_ROUTES)
      },
      {
        path: 'degree-levels',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/degree-levels/degree-levels.routes').then((m) => m.DEGREE_LEVELS_ROUTES)
      },
      {
        path: 'technologies',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/technologies/technologies.routes').then((m) => m.TECHNOLOGIES_ROUTES)
      },
      {
        path: 'certification-issuers',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/certification-issuers/certification-issuers.routes').then(
            (m) => m.CERTIFICATION_ISSUERS_ROUTES
          )
      },
      {
        path: 'document-types',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/document-types/document-types.routes').then((m) => m.DOCUMENT_TYPES_ROUTES)
      },
      {
        path: 'courses',
        canActivate: [adminGuard],
        loadChildren: () => import('./features/courses/courses.routes').then((m) => m.COURSES_ROUTES)
      },
      {
        path: 'analytics',
        canActivate: [adminGuard],
        loadChildren: () =>
          import('./features/analytics/analytics.routes').then((m) => m.ANALYTICS_ROUTES)
      }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
