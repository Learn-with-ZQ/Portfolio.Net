import { HttpClient, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedQuery, PagedResult } from '../models/api.models';
import { ApiResponseParser } from './api-response-parser';

/**
 * Base for every read service. All HTTP responses are funnelled through
 * {@link ApiResponseParser}, so services are agnostic to the envelope shape
 * the API happens to return.
 */
export abstract class ApiBaseService {
  protected readonly http = inject(HttpClient);
  protected readonly parser = inject(ApiResponseParser);
  protected readonly apiUrl = environment.apiUrl;
  protected readonly profileId = environment.portfolioProfileId;

  private buildParams(query: PagedQuery): HttpParams {
    let params = new HttpParams();
    const entries: Record<string, string | number | boolean | undefined> = {
      portfolioProfileId: query.portfolioProfileId,
      pageNumber: query.pageNumber,
      pageSize: query.pageSize,
      searchTerm: query.searchTerm,
      isPublic: query.isPublic,
      isPublished: query.isPublished,
      status: query.status,
      category: query.category
    };
    for (const [key, value] of Object.entries(entries)) {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    }
    return params;
  }

  /** Fetches a single page and returns the full paged result. */
  protected fetchPaged<T>(path: string, query: PagedQuery): Observable<PagedResult<T>> {
    return this.http
      .get<unknown>(`${this.apiUrl}${path}/paged`, { params: this.buildParams(query) })
      .pipe(map((raw) => this.parser.parse<PagedResult<T>>(raw)));
  }

  /** Fetches every item for the configured profile (single large page). */
  protected fetchAll<T>(path: string, pageSize = 100, isPublic?: boolean): Observable<T[]> {
    return this.fetchPaged<T>(path, {
      portfolioProfileId: this.profileId,
      pageNumber: 1,
      pageSize,
      isPublic
    }).pipe(map((result) => result.items));
  }

  /** Fetch all items with extra query fields (status, category, isPublished…). */
  protected fetchAllQuery<T>(path: string, query: Partial<PagedQuery>, pageSize = 100): Observable<T[]> {
    return this.fetchPaged<T>(path, {
      portfolioProfileId: this.profileId,
      pageNumber: 1,
      pageSize,
      ...query
    }).pipe(map((result) => result.items));
  }

  protected fetchById<T>(path: string, id: number): Observable<T> {
    return this.http
      .get<unknown>(`${this.apiUrl}${path}/${id}`)
      .pipe(map((raw) => this.parser.parse<T>(raw)));
  }
}
