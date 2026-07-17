import { Routes } from '@angular/router';
import { CoursesFormComponent } from './courses-form.component';
import { CoursesListComponent } from './courses-list.component';

export const COURSES_ROUTES: Routes = [
  { path: '', component: CoursesListComponent },
  { path: 'new', component: CoursesFormComponent },
  { path: ':id/edit', component: CoursesFormComponent }
];
