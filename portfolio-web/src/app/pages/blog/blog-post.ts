import { Component, computed, inject } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { monthYear, splitTags } from '../../core/utils/format';
import { markdownToHtml } from '../../core/utils/markdown';

@Component({
  selector: 'app-blog-post',
  imports: [RouterLink, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './blog-post.html',
  styleUrl: './blog-post.scss'
})
export class BlogPostPage {
  private readonly api = inject(PortfolioApiService);
  private readonly route = inject(ActivatedRoute);
  private readonly sanitizer = inject(DomSanitizer);
  readonly monthYear = monthYear;
  readonly splitTags = splitTags;

  private readonly slug = this.route.snapshot.paramMap.get('slug') ?? '';
  readonly state = toState(this.api.getBlogPost(this.slug));

  readonly html = computed<SafeHtml | null>(() => {
    const st = this.state();
    if (st.status !== 'success' || !st.data) return null;
    return this.sanitizer.bypassSecurityTrustHtml(markdownToHtml(st.data.contentMarkdown));
  });
}
