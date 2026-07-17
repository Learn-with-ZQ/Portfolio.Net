import { Routes } from '@angular/router';
import { CertificationIssuersFormComponent } from './certification-issuers-form.component';
import { CertificationIssuersListComponent } from './certification-issuers-list.component';

export const CERTIFICATION_ISSUERS_ROUTES: Routes = [
  { path: '', component: CertificationIssuersListComponent },
  { path: 'new', component: CertificationIssuersFormComponent },
  { path: ':id/edit', component: CertificationIssuersFormComponent }
];
