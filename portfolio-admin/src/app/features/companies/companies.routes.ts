import { Routes } from '@angular/router';
import { CompaniesFormComponent } from './companies-form.component';
import { CompaniesListComponent } from './companies-list.component';

export const COMPANIES_ROUTES: Routes = [
  { path: '', component: CompaniesListComponent },
  { path: 'new', component: CompaniesFormComponent },
  { path: ':id/edit', component: CompaniesFormComponent }
];
