import { Injectable } from '@angular/core';
import { Observable, forkJoin, of, switchMap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { map } from 'rxjs';
import {
  Award,
  BlogPost,
  BlogPostDetail,
  Certification,
  DocumentDetail,
  DocumentItem,
  Education,
  EducationFull,
  Experience,
  ExperienceFull,
  Project,
  ProjectFull,
  Reference,
  ResumeData,
  Skill,
  SkillFull,
  SubmitTestimonial,
  Testimonial
} from '../models/portfolio.models';
import { ApiBaseService } from './api-base.service';

/**
 * Single read-only gateway to every public portfolio module endpoint.
 * All methods target the profile configured in `environment.portfolioProfileId`.
 */
@Injectable({ providedIn: 'root' })
export class PortfolioApiService extends ApiBaseService {
  getExperience(): Observable<Experience[]> {
    return this.fetchAll<Experience>('/api/experience');
  }

  getProjects(): Observable<Project[]> {
    return this.fetchAll<Project>('/api/projects');
  }

  /** Full project with summary, technology list and highlight bullets. */
  getProject(id: number): Observable<ProjectFull> {
    return this.fetchById<ProjectFull>('/api/projects', id);
  }

  /** Education entries enriched with their courses (list + getById each). */
  getEducationFull(): Observable<EducationFull[]> {
    return this.getEducation().pipe(
      switchMap((list) =>
        list.length
          ? forkJoin(list.map((e) => this.fetchById<EducationFull>('/api/education', e.educationId)))
          : of<EducationFull[]>([])
      )
    );
  }

  getEducation(): Observable<Education[]> {
    return this.fetchAll<Education>('/api/education');
  }

  getSkills(): Observable<Skill[]> {
    return this.fetchAll<Skill>('/api/skills');
  }

  getAwards(): Observable<Award[]> {
    return this.fetchAll<Award>('/api/awards');
  }

  getCertifications(): Observable<Certification[]> {
    return this.fetchAll<Certification>('/api/certifications');
  }

  /** Public documents only (server-side IsPublic filter). */
  getDocuments(): Observable<DocumentItem[]> {
    return this.fetchAll<DocumentItem>('/api/documents', 200, true);
  }

  /** Full document incl. storage path — used to build resume download links. */
  getDocument(id: number): Observable<DocumentDetail> {
    return this.fetchById<DocumentDetail>('/api/documents', id);
  }

  /* --- Resume aggregation ------------------------------------------------ */

  /** Experience with per-role bullet points (list + getById per item). */
  getExperienceFull(): Observable<ExperienceFull[]> {
    return this.getExperience().pipe(
      switchMap((list) =>
        list.length
          ? forkJoin(
              list.map((e) => this.fetchById<ExperienceFull>('/api/experience', e.experienceId))
            )
          : of<ExperienceFull[]>([])
      )
    );
  }

  /** Skills with their keyword items (list + getById per item). */
  getSkillsFull(): Observable<SkillFull[]> {
    return this.getSkills().pipe(
      switchMap((list) =>
        list.length
          ? forkJoin(list.map((s) => this.fetchById<SkillFull>('/api/skills', s.skillId)))
          : of<SkillFull[]>([])
      )
    );
  }

  /* --- New modules ------------------------------------------------------- */

  /** Published (approved) testimonials. */
  getTestimonials(): Observable<Testimonial[]> {
    return this.fetchAllQuery<Testimonial>('/api/testimonials', { status: 2 });
  }

  /** Submit a public testimonial (created Pending, awaiting approval). */
  submitTestimonial(body: SubmitTestimonial): Observable<void> {
    return this.http
      .post<unknown>(`${environment.apiUrl}/api/testimonials/submit`, {
        portfolioProfileId: this.profileId,
        ...body
      })
      .pipe(map(() => void 0));
  }

  /** Public references (contact fields already redacted server-side). */
  getReferences(): Observable<Reference[]> {
    return this.fetchAllQuery<Reference>('/api/references', { isPublic: true });
  }

  /** Published blog posts, optionally filtered by category. */
  getBlogPosts(category?: string): Observable<BlogPost[]> {
    return this.fetchAllQuery<BlogPost>('/api/blog', { isPublished: true, category });
  }

  /** A single published post by slug (full markdown content). */
  getBlogPost(slug: string): Observable<BlogPostDetail> {
    return this.http
      .get<unknown>(
        `${environment.apiUrl}/api/blog/slug/${encodeURIComponent(slug)}?portfolioProfileId=${this.profileId}`
      )
      .pipe(map((raw) => this.parser.parse<BlogPostDetail>(raw)));
  }

  /** One payload with every section the resume renders and exports. */
  getResume(): Observable<ResumeData> {
    return forkJoin({
      experience: this.getExperienceFull(),
      projects: this.getProjects(),
      education: this.getEducation(),
      skills: this.getSkillsFull(),
      certifications: this.getCertifications(),
      awards: this.getAwards()
    });
  }
}
