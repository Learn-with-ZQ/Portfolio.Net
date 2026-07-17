import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { RouterLink } from '@angular/router';
import { Project } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange, splitTags } from '../../core/utils/format';

type SortKey = 'newest' | 'oldest' | 'name';

@Component({
  selector: 'app-projects',
  imports: [
    RouterLink,
    FormsModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatChipsModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './projects.html',
  styleUrl: './projects.scss'
})
export class ProjectsPage {
  private readonly api = inject(PortfolioApiService);
  readonly dateRange = dateRange;
  readonly splitTags = splitTags;

  readonly state = toState(this.api.getProjects());

  readonly search = signal('');
  readonly context = signal<string>('all');
  readonly sort = signal<SortKey>('newest');

  /** Distinct context labels for the filter chips. */
  readonly contexts = computed(() => {
    const st = this.state();
    if (st.status !== 'success' || !st.data) return [];
    return [...new Set(st.data.map((p) => p.contextName).filter((c): c is string => !!c))].sort();
  });

  readonly visible = computed<Project[]>(() => {
    const st = this.state();
    if (st.status !== 'success' || !st.data) return [];
    const term = this.search().trim().toLowerCase();
    const ctx = this.context();
    const sort = this.sort();

    let items = st.data.filter((p) => {
      const matchesCtx = ctx === 'all' || p.contextName === ctx;
      const haystack = `${p.projectName} ${p.contextName ?? ''} ${p.technologies}`.toLowerCase();
      return matchesCtx && (!term || haystack.includes(term));
    });

    items = [...items].sort((a, b) => {
      if (sort === 'name') return a.projectName.localeCompare(b.projectName);
      const da = new Date(a.startDate).getTime();
      const db = new Date(b.startDate).getTime();
      return sort === 'newest' ? db - da : da - db;
    });
    return items;
  });

  setContext(c: string): void {
    this.context.set(c);
  }
}
