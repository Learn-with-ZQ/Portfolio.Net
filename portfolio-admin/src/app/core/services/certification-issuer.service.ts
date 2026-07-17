import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  CertificationIssuer,
  CreateCertificationIssuerRequest,
  UpdateCertificationIssuerRequest
} from '../models/certification-issuer.models';
import { LookupItem } from '../models/lookup.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class CertificationIssuerApiService extends ApiBaseService {
  private readonly path = '/api/certification-issuers';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<CertificationIssuer>> {
    return this.fetchPaged<CertificationIssuer>(this.path, query);
  }

  getById(id: number): Observable<CertificationIssuer> {
    return this.fetchById<CertificationIssuer>(this.path, id);
  }

  create(body: CreateCertificationIssuerRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateCertificationIssuerRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
