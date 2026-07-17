import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange } from '../../core/utils/format';

@Component({
  selector: 'app-awards',
  imports: [MatIconModule, MatProgressSpinnerModule],
  template: `
    <section class="page">
      <h1 class="page-title">Awards</h1>
      <p class="page-subtitle">Recognition and achievements.</p>

      @let s = state();
      @if (s.status === 'loading') {
        <div class="state"><mat-spinner diameter="44" /><span>Loading…</span></div>
      } @else if (s.status === 'error') {
        <div class="state"><mat-icon>error_outline</mat-icon><span>{{ s.error }}</span></div>
      } @else if (!s.data?.length) {
        <div class="state"><mat-icon>emoji_events</mat-icon><span>No awards listed yet.</span></div>
      } @else {
        <div class="grid grid-cards">
          @for (award of s.data; track award.awardId) {
            <article class="award">
              <span class="award-icon"><mat-icon>emoji_events</mat-icon></span>
              <h3>{{ award.awardName }}</h3>
              <p class="range">{{ dateRange(award.startDate, award.endDate) }}</p>
            </article>
          }
        </div>
      }
    </section>
  `,
  styles: `
    .award {
      background: #fff; border: 1px solid rgba(0,0,0,0.06); border-radius: 16px; padding: 24px;
      text-align: center; display: flex; flex-direction: column; align-items: center; gap: 8px;
    }
    .award-icon {
      display: inline-grid; place-items: center; width: 56px; height: 56px;
      border-radius: 50%; background: linear-gradient(135deg, #ffb300, #f57c00); color: #fff;
    }
    .award-icon mat-icon { font-size: 30px; height: 30px; width: 30px; }
    .award h3 { margin: 6px 0 0; font-size: 1.08rem; }
    .range { margin: 0; color: var(--brand-dark); font-size: 0.84rem; font-weight: 500; }
  `
})
export class AwardsPage {
  private readonly api = inject(PortfolioApiService);
  readonly dateRange = dateRange;
  readonly state = toState(this.api.getAwards());
}
