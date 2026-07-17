import { Routes } from '@angular/router';
import { DegreeLevelsFormComponent } from './degree-levels-form.component';
import { DegreeLevelsListComponent } from './degree-levels-list.component';

export const DEGREE_LEVELS_ROUTES: Routes = [
  { path: '', component: DegreeLevelsListComponent },
  { path: 'new', component: DegreeLevelsFormComponent },
  { path: ':id/edit', component: DegreeLevelsFormComponent }
];
