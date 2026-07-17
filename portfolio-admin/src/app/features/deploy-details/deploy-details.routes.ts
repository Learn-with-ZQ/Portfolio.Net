import { Routes } from '@angular/router';
import { DeployDetailsFormComponent } from './deploy-details-form.component';
import { DeployDetailsListComponent } from './deploy-details-list.component';

export const DEPLOY_DETAILS_ROUTES: Routes = [
  { path: '', component: DeployDetailsListComponent },
  { path: 'new', component: DeployDetailsFormComponent },
  { path: ':id/edit', component: DeployDetailsFormComponent }
];
