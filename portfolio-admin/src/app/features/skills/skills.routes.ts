import { Routes } from '@angular/router';
import { SkillsFormComponent } from './skills-form.component';
import { SkillsListComponent } from './skills-list.component';

export const SKILLS_ROUTES: Routes = [
  { path: '', component: SkillsListComponent },
  { path: 'new', component: SkillsFormComponent },
  { path: ':id/edit', component: SkillsFormComponent }
];
