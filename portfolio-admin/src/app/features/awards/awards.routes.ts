import { Routes } from '@angular/router';
import { AwardsFormComponent } from './awards-form.component';
import { AwardsListComponent } from './awards-list.component';

export const AWARDS_ROUTES: Routes = [
  { path: '', component: AwardsListComponent },
  { path: 'new', component: AwardsFormComponent },
  { path: ':id/edit', component: AwardsFormComponent }
];
