import { Routes } from '@angular/router';
import { EducationFormComponent } from './education-form.component';
import { EducationListComponent } from './education-list.component';

export const EDUCATION_ROUTES: Routes = [
  { path: '', component: EducationListComponent },
  { path: 'new', component: EducationFormComponent },
  { path: ':id/edit', component: EducationFormComponent }
];
