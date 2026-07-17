import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  CreateTechnologyRequest,
  LookupItem,
  Technology,
  UpdateTechnologyRequest
} from '../models/technology.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class TechnologyApiService extends ApiBaseService {
  private readonly path = '/api/technologies';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<Technology>> {
    return this.fetchPaged<Technology>(this.path, query);
  }

  getById(id: number): Observable<Technology> {
    return this.fetchById<Technology>(this.path, id);
  }

  create(body: CreateTechnologyRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateTechnologyRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
