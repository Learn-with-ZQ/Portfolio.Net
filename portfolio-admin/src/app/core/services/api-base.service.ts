import { HttpClient, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiEnvelope, CommandResult, PagedQuery, PagedResult } from '../models/api.models';
import { LookupItem } from '../models/lookup.models';

export abstract class ApiBaseService {
  protected readonly http = inject(HttpClient);
  protected readonly apiUrl = environment.apiUrl;

  protected unwrap<T>(envelope: ApiEnvelope<T>): T {
    if (!envelope.success || envelope.data === undefined) {
      throw new Error(envelope.message ?? envelope.errors?.join(', ') ?? 'Request failed');
    }
    return envelope.data;
  }

  protected buildParams(query: PagedQuery): HttpParams {
    let params = new HttpParams();
    const entries: Record<string, string | number | boolean | undefined> = {
      portfolioProfileId: query.portfolioProfileId,
      pageNumber: query.pageNumber,
      pageSize: query.pageSize,
      searchTerm: query.searchTerm,
      status: query.status,
      isPublished: query.isPublished,
      isPublic: query.isPublic,
      category: query.category,
      eventType: query.eventType,
      paragraphTypeId: query.paragraphTypeId,
      isActive: query.isActive
    };

    for (const [key, value] of Object.entries(entries)) {
      if (value !== undefined && value !== null && value !== '') {
        params = params.set(key, String(value));
      }
    }

    return params;
  }

  protected fetchPaged<T>(path: string, query: PagedQuery): Observable<PagedResult<T>> {
    return this.http
      .get<ApiEnvelope<PagedResult<T>>>(`${this.apiUrl}${path}/paged`, {
        params: this.buildParams(query)
      })
      .pipe(map((env) => this.unwrap(env)));
  }

  protected fetchLookup(path: string): Observable<LookupItem[]> {
    return this.http
      .get<ApiEnvelope<LookupItem[]>>(`${this.apiUrl}${path}/lookup`)
      .pipe(map((env) => this.unwrap(env)));
  }

  protected fetchById<T>(path: string, id: number): Observable<T> {
    return this.http
      .get<ApiEnvelope<T>>(`${this.apiUrl}${path}/${id}`)
      .pipe(map((env) => this.unwrap(env)));
  }

  protected postItem(path: string, body: unknown): Observable<CommandResult> {
    return this.http
      .post<ApiEnvelope<CommandResult>>(`${this.apiUrl}${path}`, body)
      .pipe(map((env) => this.unwrap(env)));
  }

  protected putItem(path: string, id: number, body: unknown): Observable<CommandResult> {
    return this.http
      .put<ApiEnvelope<CommandResult>>(`${this.apiUrl}${path}/${id}`, body)
      .pipe(map((env) => this.unwrap(env)));
  }

  protected removeItem(path: string, id: number): Observable<void> {
    return this.http.delete(`${this.apiUrl}${path}/${id}`).pipe(map(() => undefined));
  }
}
