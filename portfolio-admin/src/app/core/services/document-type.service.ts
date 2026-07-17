import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  CreateDocumentTypeRequest,
  DocumentType,
  LookupItem,
  UpdateDocumentTypeRequest
} from '../models/document-type.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class DocumentTypeApiService extends ApiBaseService {
  private readonly path = '/api/document-types';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<DocumentType>> {
    return this.fetchPaged<DocumentType>(this.path, query);
  }

  getById(id: number): Observable<DocumentType> {
    return this.fetchById<DocumentType>(this.path, id);
  }

  create(body: CreateDocumentTypeRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateDocumentTypeRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
