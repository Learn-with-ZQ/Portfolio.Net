import { Routes } from '@angular/router';
import { DegreesFormComponent } from './degrees-form.component';
import { DegreesListComponent } from './degrees-list.component';

export const DEGREES_ROUTES: Routes = [
  { path: '', component: DegreesListComponent },
  { path: 'new', component: DegreesFormComponent },
  { path: ':id/edit', component: DegreesFormComponent }
];
