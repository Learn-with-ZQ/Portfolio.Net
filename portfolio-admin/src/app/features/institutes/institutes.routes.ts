import { Routes } from '@angular/router';
import { InstitutesFormComponent } from './institutes-form.component';
import { InstitutesListComponent } from './institutes-list.component';

export const INSTITUTES_ROUTES: Routes = [
  { path: '', component: InstitutesListComponent },
  { path: 'new', component: InstitutesFormComponent },
  { path: ':id/edit', component: InstitutesFormComponent }
];
