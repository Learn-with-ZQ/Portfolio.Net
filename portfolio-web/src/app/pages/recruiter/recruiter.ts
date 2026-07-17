import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { SITE } from '../../core/content/site-content';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange } from '../../core/utils/format';

@Component({
  selector: 'app-recruiter',
  imports: [RouterLink, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './recruiter.html',
  styleUrl: './recruiter.scss'
})
export class RecruiterPage {
  private readonly api = inject(PortfolioApiService);
  readonly site = SITE;
  readonly recruiter = SITE.recruiter;
  readonly dateRange = dateRange;
  readonly experience = toState(this.api.getExperience());
}
