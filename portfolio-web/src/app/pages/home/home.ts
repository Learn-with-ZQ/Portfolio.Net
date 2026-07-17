import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { SITE } from '../../core/content/site-content';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange, splitTags } from '../../core/utils/format';

@Component({
  selector: 'app-home',
  imports: [RouterLink, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class HomePage {
  private readonly api = inject(PortfolioApiService);
  readonly site = SITE;
  readonly dateRange = dateRange;
  readonly splitTags = splitTags;

  readonly state = toState(
    forkJoin({
      experience: this.api.getExperience(),
      projects: this.api.getProjects(),
      skills: this.api.getSkills(),
      certifications: this.api.getCertifications()
    })
  );
}
