import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  CreateDegreeLevelRequest,
  DegreeLevel,
  UpdateDegreeLevelRequest
} from '../models/degree-level.models';
import { LookupItem } from '../models/lookup.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class DegreeLevelApiService extends ApiBaseService {
  private readonly path = '/api/degree-levels';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<DegreeLevel>> {
    return this.fetchPaged<DegreeLevel>(this.path, query);
  }

  getById(id: number): Observable<DegreeLevel> {
    return this.fetchById<DegreeLevel>(this.path, id);
  }

  create(body: CreateDegreeLevelRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateDegreeLevelRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
