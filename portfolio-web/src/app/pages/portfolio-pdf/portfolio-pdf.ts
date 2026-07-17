import { isPlatformBrowser } from '@angular/common';
import { Component, PLATFORM_ID, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { forkJoin, of, catchError } from 'rxjs';
import { SITE } from '../../core/content/site-content';
import {
  DocumentItem,
  Reference,
  ResumeData,
  SkillFull,
  Testimonial
} from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { dateRange, monthYear } from '../../core/utils/format';

interface BookData {
  resume: ResumeData;
  testimonials: Testimonial[];
  references: Reference[];
  documents: DocumentItem[];
}

@Component({
  selector: 'app-portfolio-pdf',
  imports: [MatButtonModule, MatIconModule, MatProgressSpinnerModule],
  templateUrl: './portfolio-pdf.html',
  styleUrl: './portfolio-pdf.scss'
})
export class PortfolioPdfPage {
  private readonly api = inject(PortfolioApiService);
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  readonly site = SITE;
  readonly dateRange = dateRange;
  readonly monthYear = monthYear;

  readonly state = toState(
    forkJoin({
      resume: this.api.getResume(),
      // Optional sections tolerate failure so the book still renders.
      testimonials: this.api.getTestimonials().pipe(catchError(() => of([] as Testimonial[]))),
      references: this.api.getReferences().pipe(catchError(() => of([] as Reference[]))),
      documents: this.api.getDocuments().pipe(catchError(() => of([] as DocumentItem[])))
    })
  );

  skillLine(skill: SkillFull): string {
    return skill.details.map((d) => d.skillDetailName).join(', ');
  }

  sections(d: BookData): { label: string; count: number }[] {
    return [
      { label: 'Roles', count: d.resume.experience.length },
      { label: 'Projects', count: d.resume.projects.length },
      { label: 'Skill areas', count: d.resume.skills.length },
      { label: 'Certifications', count: d.resume.certifications.length },
      { label: 'Awards', count: d.resume.awards.length },
      { label: 'Testimonials', count: d.testimonials.length }
    ];
  }

  print(): void {
    if (this.isBrowser) window.print();
  }
}
