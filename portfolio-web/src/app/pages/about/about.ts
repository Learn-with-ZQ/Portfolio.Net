import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { SITE } from '../../core/content/site-content';

@Component({
  selector: 'app-about',
  imports: [MatIconModule],
  template: `
    <section class="page">
      <h1 class="page-title">About</h1>
      <p class="page-subtitle">{{ site.about.intro }}</p>

      <div class="about-grid">
        <div class="prose">
          @for (para of site.about.paragraphs; track $index) {
            <p>{{ para }}</p>
          }
        </div>

        <aside class="side">
          <h2>What I work with</h2>
          <ul class="highlights">
            @for (h of site.about.highlights; track h) {
              <li><mat-icon>check_circle</mat-icon><span>{{ h }}</span></li>
            }
          </ul>
          <div class="facts">
            <div><mat-icon>person</mat-icon> {{ site.owner }}</div>
            <div><mat-icon>badge</mat-icon> {{ site.role }}</div>
            <div><mat-icon>place</mat-icon> {{ site.location }}</div>
          </div>
        </aside>
      </div>
    </section>
  `,
  styles: `
    .about-grid {
      display: grid;
      gap: 32px;
      grid-template-columns: 1fr;
    }
    .prose p {
      font-size: 1.05rem;
      line-height: 1.75;
      color: var(--mat-sys-on-surface, #26303b);
      margin: 0 0 18px;
    }
    .side {
      background: #fff;
      border: 1px solid rgba(0, 0, 0, 0.06);
      border-radius: 16px;
      padding: 24px;
      align-self: start;
      h2 { margin: 0 0 16px; font-size: 1.15rem; }
    }
    .highlights {
      list-style: none;
      padding: 0;
      margin: 0 0 20px;
      display: grid;
      gap: 12px;
      li { display: flex; align-items: center; gap: 10px; font-weight: 500; }
      mat-icon { color: var(--brand); }
    }
    .facts {
      display: grid;
      gap: 10px;
      padding-top: 18px;
      border-top: 1px solid rgba(0, 0, 0, 0.06);
      color: var(--mat-sys-on-surface-variant, #5f6368);
      div { display: flex; align-items: center; gap: 10px; }
      mat-icon { color: var(--brand); font-size: 20px; height: 20px; width: 20px; }
    }
    @media (min-width: 900px) {
      .about-grid { grid-template-columns: 1.7fr 1fr; }
    }
  `
})
export class AboutPage {
  readonly site = SITE;
}
