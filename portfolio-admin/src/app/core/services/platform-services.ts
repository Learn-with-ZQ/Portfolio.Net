import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ApiEnvelope, PagedQuery, PagedResult } from '../models/api.models';
import {
  AnalyticsEvent,
  AnalyticsSummary,
  BlogPost,
  BlogPostDetail,
  CreateBlogPostRequest,
  CreateParagraphRequest,
  CreateReferenceRequest,
  Paragraph,
  ParagraphDetail,
  Reference,
  ReferenceDetail,
  Testimonial,
  UpdateBlogPostRequest,
  UpdateParagraphRequest,
  UpdateReferenceRequest
} from '../models/portfolio.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class TestimonialsApiService extends ApiBaseService {
  private readonly path = '/api/testimonials';

  getPaged(query: PagedQuery): Observable<PagedResult<Testimonial>> {
    return this.fetchPaged<Testimonial>(this.path, query);
  }

  approve(id: number): Observable<void> {
    return this.http.post(`${this.apiUrl}${this.path}/${id}/approve`, {}).pipe(map(() => undefined));
  }

  reject(id: number): Observable<void> {
    return this.http.post(`${this.apiUrl}${this.path}/${id}/reject`, {}).pipe(map(() => undefined));
  }

  delete(id: number): Observable<void> {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class ReferencesApiService extends ApiBaseService {
  private readonly path = '/api/references';

  getPaged(query: PagedQuery): Observable<PagedResult<Reference>> {
    return this.fetchPaged<Reference>(this.path, query);
  }

  getById(id: number): Observable<ReferenceDetail> {
    return this.fetchById<ReferenceDetail>(this.path, id);
  }

  create(body: CreateReferenceRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateReferenceRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class BlogApiService extends ApiBaseService {
  private readonly path = '/api/blog';

  getPaged(query: PagedQuery): Observable<PagedResult<BlogPost>> {
    return this.fetchPaged<BlogPost>(this.path, query);
  }

  getById(id: number): Observable<BlogPostDetail> {
    return this.fetchById<BlogPostDetail>(this.path, id);
  }

  create(body: CreateBlogPostRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateBlogPostRequest) {
    return this.putItem(this.path, id, body);
  }

  publish(id: number): Observable<void> {
    return this.http.post(`${this.apiUrl}${this.path}/${id}/publish`, {}).pipe(map(() => undefined));
  }

  unpublish(id: number): Observable<void> {
    return this.http.post(`${this.apiUrl}${this.path}/${id}/unpublish`, {}).pipe(map(() => undefined));
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class ParagraphsApiService extends ApiBaseService {
  private readonly path = '/api/paragraphs';

  getPaged(query: PagedQuery): Observable<PagedResult<Paragraph>> {
    return this.fetchPaged<Paragraph>(this.path, query);
  }

  getById(id: number): Observable<ParagraphDetail> {
    return this.fetchById<ParagraphDetail>(this.path, id);
  }

  create(body: CreateParagraphRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateParagraphRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class AnalyticsApiService extends ApiBaseService {
  private readonly path = '/api/analytics';

  getSummary(portfolioProfileId: number): Observable<AnalyticsSummary> {
    return this.http
      .get<ApiEnvelope<AnalyticsSummary>>(`${this.apiUrl}${this.path}/summary`, {
        params: { portfolioProfileId }
      })
      .pipe(map((env) => this.unwrap(env)));
  }

  getPaged(query: PagedQuery): Observable<PagedResult<AnalyticsEvent>> {
    return this.fetchPaged<AnalyticsEvent>(this.path, query);
  }
}
