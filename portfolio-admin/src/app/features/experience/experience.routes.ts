import { Routes } from '@angular/router';
import { ExperienceFormComponent } from './experience-form.component';
import { ExperienceListComponent } from './experience-list.component';

export const EXPERIENCE_ROUTES: Routes = [
  { path: '', component: ExperienceListComponent },
  { path: 'new', component: ExperienceFormComponent },
  { path: ':id/edit', component: ExperienceFormComponent }
];
