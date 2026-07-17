import { Routes } from '@angular/router';
import { CertificationsFormComponent } from './certifications-form.component';
import { CertificationsListComponent } from './certifications-list.component';

export const CERTIFICATIONS_ROUTES: Routes = [
  { path: '', component: CertificationsListComponent },
  { path: 'new', component: CertificationsFormComponent },
  { path: ':id/edit', component: CertificationsFormComponent }
];
