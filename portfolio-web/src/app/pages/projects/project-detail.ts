import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProjectFull } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange } from '../../core/utils/format';

@Component({
  selector: 'app-project-detail',
  imports: [RouterLink, MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './project-detail.html',
  styleUrl: './project-detail.scss'
})
export class ProjectDetailPage {
  private readonly api = inject(PortfolioApiService);
  private readonly route = inject(ActivatedRoute);
  readonly dateRange = dateRange;

  private readonly id = Number(this.route.snapshot.paramMap.get('id'));
  readonly state = toState(this.api.getProject(this.id));

  context(p: ProjectFull): string {
    return p.companyName || p.courseName || p.practice || 'Personal project';
  }
}
