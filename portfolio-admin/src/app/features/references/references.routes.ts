import { Routes } from '@angular/router';
import { ReferencesFormComponent } from './references-form.component';
import { ReferencesListComponent } from './references-list.component';

export const REFERENCES_ROUTES: Routes = [
  { path: '', component: ReferencesListComponent },
  { path: 'new', component: ReferencesFormComponent },
  { path: ':id/edit', component: ReferencesFormComponent }
];
