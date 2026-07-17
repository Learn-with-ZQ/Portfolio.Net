import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  CreateInstituteRequest,
  Institute,
  UpdateInstituteRequest
} from '../models/institute.models';
import { LookupItem } from '../models/lookup.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class InstituteApiService extends ApiBaseService {
  private readonly path = '/api/institutes';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<Institute>> {
    return this.fetchPaged<Institute>(this.path, query);
  }

  getById(id: number): Observable<Institute> {
    return this.fetchById<Institute>(this.path, id);
  }

  create(body: CreateInstituteRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateInstituteRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
