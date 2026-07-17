import { Routes } from '@angular/router';
import { SITE } from './core/content/site-content';

const name = SITE.owner;

export const routes: Routes = [
  {
    path: '',
    title: `${name} — Portfolio`,
    loadComponent: () => import('./pages/home/home').then((m) => m.HomePage)
  },
  {
    path: 'about',
    title: `About — ${name}`,
    loadComponent: () => import('./pages/about/about').then((m) => m.AboutPage)
  },
  {
    path: 'recruiter',
    title: `For Recruiters — ${name}`,
    loadComponent: () => import('./pages/recruiter/recruiter').then((m) => m.RecruiterPage)
  },
  {
    path: 'experience',
    title: `Experience — ${name}`,
    loadComponent: () => import('./pages/experience/experience').then((m) => m.ExperiencePage)
  },
  {
    path: 'projects',
    title: `Projects — ${name}`,
    loadComponent: () => import('./pages/projects/projects').then((m) => m.ProjectsPage)
  },
  {
    path: 'projects/:id',
    title: `Project — ${name}`,
    loadComponent: () => import('./pages/projects/project-detail').then((m) => m.ProjectDetailPage)
  },
  {
    path: 'skills',
    title: `Skills — ${name}`,
    loadComponent: () => import('./pages/skills/skills').then((m) => m.SkillsPage)
  },
  {
    path: 'education',
    title: `Education — ${name}`,
    loadComponent: () => import('./pages/education/education').then((m) => m.EducationPage)
  },
  {
    path: 'awards',
    title: `Awards — ${name}`,
    loadComponent: () => import('./pages/awards/awards').then((m) => m.AwardsPage)
  },
  {
    path: 'certifications',
    title: `Certifications — ${name}`,
    loadComponent: () =>
      import('./pages/certifications/certifications').then((m) => m.CertificationsPage)
  },
  {
    path: 'resume',
    title: `Resume — ${name}`,
    loadComponent: () => import('./pages/resume/resume').then((m) => m.ResumePage)
  },
  {
    path: 'testimonials',
    title: `Testimonials — ${name}`,
    loadComponent: () => import('./pages/testimonials/testimonials').then((m) => m.TestimonialsPage)
  },
  {
    path: 'references',
    title: `References — ${name}`,
    loadComponent: () => import('./pages/references/references').then((m) => m.ReferencesPage)
  },
  {
    path: 'blog',
    title: `Blog — ${name}`,
    loadComponent: () => import('./pages/blog/blog').then((m) => m.BlogPage)
  },
  {
    path: 'blog/:slug',
    title: `Blog — ${name}`,
    loadComponent: () => import('./pages/blog/blog-post').then((m) => m.BlogPostPage)
  },
  {
    path: 'documents',
    title: `Documents — ${name}`,
    loadComponent: () => import('./pages/documents/documents').then((m) => m.DocumentsPage)
  },
  {
    path: 'downloads',
    title: `Downloads — ${name}`,
    loadComponent: () => import('./pages/downloads/downloads').then((m) => m.DownloadsPage)
  },
  {
    path: 'contact',
    title: `Contact — ${name}`,
    loadComponent: () => import('./pages/contact/contact').then((m) => m.ContactPage)
  },
  {
    path: 'assistant',
    title: `Assistant — ${name}`,
    loadComponent: () => import('./pages/assistant/assistant').then((m) => m.AssistantPage)
  },
  {
    path: 'portfolio-pdf',
    title: `Portfolio PDF — ${name}`,
    loadComponent: () => import('./pages/portfolio-pdf/portfolio-pdf').then((m) => m.PortfolioPdfPage)
  },
  { path: '**', redirectTo: '' }
];
