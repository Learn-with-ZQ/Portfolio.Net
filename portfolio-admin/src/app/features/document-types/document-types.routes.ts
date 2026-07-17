import { Routes } from '@angular/router';
import { DocumentTypesFormComponent } from './document-types-form.component';
import { DocumentTypesListComponent } from './document-types-list.component';

export const DOCUMENT_TYPES_ROUTES: Routes = [
  { path: '', component: DocumentTypesListComponent },
  { path: 'new', component: DocumentTypesFormComponent },
  { path: ':id/edit', component: DocumentTypesFormComponent }
];
