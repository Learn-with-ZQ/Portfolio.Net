import { Routes } from '@angular/router';
import { TechnologiesFormComponent } from './technologies-form.component';
import { TechnologiesListComponent } from './technologies-list.component';

export const TECHNOLOGIES_ROUTES: Routes = [
  { path: '', component: TechnologiesListComponent },
  { path: 'new', component: TechnologiesFormComponent },
  { path: ':id/edit', component: TechnologiesFormComponent }
];
