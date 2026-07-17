import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ExperienceFull } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange, durationBetween } from '../../core/utils/format';

@Component({
  selector: 'app-experience',
  imports: [MatIconModule, MatProgressSpinnerModule],
  templateUrl: './experience.html',
  styleUrl: './experience.scss'
})
export class ExperiencePage {
  private readonly api = inject(PortfolioApiService);
  readonly dateRange = dateRange;
  readonly durationBetween = durationBetween;
  readonly state = toState(this.api.getExperienceFull());

  currentCount(items: ExperienceFull[]): number {
    return items.filter((e) => e.isCurrent).length;
  }
}
