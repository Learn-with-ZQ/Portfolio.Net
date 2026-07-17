import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  CreateDegreeRequest,
  Degree,
  LookupItem,
  UpdateDegreeRequest
} from '../models/degree.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class DegreeApiService extends ApiBaseService {
  private readonly path = '/api/degrees';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<Degree>> {
    return this.fetchPaged<Degree>(this.path, query);
  }

  getById(id: number): Observable<Degree> {
    return this.fetchById<Degree>(this.path, id);
  }

  create(body: CreateDegreeRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateDegreeRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
