import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EducationFull } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange } from '../../core/utils/format';

@Component({
  selector: 'app-education',
  imports: [MatIconModule, MatProgressSpinnerModule],
  templateUrl: './education.html',
  styleUrl: './education.scss'
})
export class EducationPage {
  private readonly api = inject(PortfolioApiService);
  readonly dateRange = dateRange;
  readonly state = toState(this.api.getEducationFull());

  degreeLabel(e: EducationFull): string {
    return e.degreePrefix ? `${e.degreePrefix} ${e.degreeName}` : e.degreeName;
  }
}
