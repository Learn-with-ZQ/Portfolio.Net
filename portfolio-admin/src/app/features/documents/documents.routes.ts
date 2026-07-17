import { Routes } from '@angular/router';
import { DocumentsFormComponent } from './documents-form.component';
import { DocumentsListComponent } from './documents-list.component';

export const DOCUMENTS_ROUTES: Routes = [
  { path: '', component: DocumentsListComponent },
  { path: 'new', component: DocumentsFormComponent },
  { path: ':id/edit', component: DocumentsFormComponent }
];
