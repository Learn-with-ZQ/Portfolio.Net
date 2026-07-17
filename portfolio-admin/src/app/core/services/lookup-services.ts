import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  Company,
  CreateCompanyRequest,
  CreateDeployDetailRequest,
  DeployDetail,
  LookupItem,
  UpdateCompanyRequest,
  UpdateDeployDetailRequest
} from '../models/lookup.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class CompanyApiService extends ApiBaseService {
  private readonly path = '/api/companies';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<Company>> {
    return this.fetchPaged<Company>(this.path, query);
  }

  getById(id: number): Observable<Company> {
    return this.fetchById<Company>(this.path, id);
  }

  create(body: CreateCompanyRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateCompanyRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class DeployDetailApiService extends ApiBaseService {
  private readonly path = '/api/deploy-details';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<DeployDetail>> {
    return this.fetchPaged<DeployDetail>(this.path, query);
  }

  getById(id: number): Observable<DeployDetail> {
    return this.fetchById<DeployDetail>(this.path, id);
  }

  create(body: CreateDeployDetailRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateDeployDetailRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
