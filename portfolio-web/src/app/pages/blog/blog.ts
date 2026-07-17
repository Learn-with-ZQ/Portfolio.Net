import { Component, computed, inject, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { BlogPost } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { monthYear, splitTags } from '../../core/utils/format';

@Component({
  selector: 'app-blog',
  imports: [RouterLink, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './blog.html',
  styleUrl: './blog.scss'
})
export class BlogPage {
  private readonly api = inject(PortfolioApiService);
  readonly monthYear = monthYear;
  readonly splitTags = splitTags;

  readonly state = toState(this.api.getBlogPosts());
  readonly category = signal<string>('all');

  readonly categories = computed(() => {
    const st = this.state();
    if (st.status !== 'success' || !st.data) return [];
    return [...new Set(st.data.map((p) => p.category).filter((c): c is string => !!c))].sort();
  });

  readonly visible = computed<BlogPost[]>(() => {
    const st = this.state();
    if (st.status !== 'success' || !st.data) return [];
    const cat = this.category();
    return cat === 'all' ? st.data : st.data.filter((p) => p.category === cat);
  });
}
