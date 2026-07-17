import { Routes } from '@angular/router';
import { BlogFormComponent } from './blog-form.component';
import { BlogListComponent } from './blog-list.component';

export const BLOG_ROUTES: Routes = [
  { path: '', component: BlogListComponent },
  { path: 'new', component: BlogFormComponent },
  { path: ':id/edit', component: BlogFormComponent }
];
