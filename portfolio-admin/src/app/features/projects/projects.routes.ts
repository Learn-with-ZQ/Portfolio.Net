import { Routes } from '@angular/router';
import { ProjectsFormComponent } from './projects-form.component';
import { ProjectsListComponent } from './projects-list.component';

export const PROJECTS_ROUTES: Routes = [
  { path: '', component: ProjectsListComponent },
  { path: 'new', component: ProjectsFormComponent },
  { path: ':id/edit', component: ProjectsFormComponent }
];
