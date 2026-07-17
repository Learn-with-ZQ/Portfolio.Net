import { Routes } from '@angular/router';
import { ParagraphsFormComponent } from './paragraphs-form.component';
import { ParagraphsListComponent } from './paragraphs-list.component';

export const PARAGRAPHS_ROUTES: Routes = [
  { path: '', component: ParagraphsListComponent },
  { path: 'new', component: ParagraphsFormComponent },
  { path: ':id/edit', component: ParagraphsFormComponent }
];
