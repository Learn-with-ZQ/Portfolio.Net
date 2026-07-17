import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import {
  Award,
  AwardDetail,
  Certification,
  CertificationDetail,
  CreateAwardRequest,
  CreateCertificationRequest,
  CreateDocumentRequest,
  CreateEducationRequest,
  CreateExperienceRequest,
  CreateProjectRequest,
  CreateSkillRequest,
  Document,
  DocumentDetail,
  Education,
  EducationDetail,
  Experience,
  ExperienceDetail,
  Project,
  ProjectDetail,
  Skill,
  SkillDetail,
  UpdateAwardRequest,
  UpdateCertificationRequest,
  UpdateDocumentRequest,
  UpdateEducationRequest,
  UpdateExperienceRequest,
  UpdateProjectRequest,
  UpdateSkillRequest
} from '../models/portfolio.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class ExperienceApiService extends ApiBaseService {
  private readonly path = '/api/experience';

  getPaged(query: PagedQuery): Observable<PagedResult<Experience>> {
    return this.fetchPaged<Experience>(this.path, query);
  }

  getById(id: number): Observable<ExperienceDetail> {
    return this.fetchById<ExperienceDetail>(this.path, id);
  }

  create(body: CreateExperienceRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateExperienceRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class ProjectsApiService extends ApiBaseService {
  private readonly path = '/api/projects';

  getPaged(query: PagedQuery): Observable<PagedResult<Project>> {
    return this.fetchPaged<Project>(this.path, query);
  }

  getById(id: number): Observable<ProjectDetail> {
    return this.fetchById<ProjectDetail>(this.path, id);
  }

  create(body: CreateProjectRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateProjectRequest) {
    return this.putItem(this.path, id, body);
  }

  syncTechnologies(id: number, technologyIds: number[]): Observable<void> {
    return this.http
      .put(`${this.apiUrl}${this.path}/${id}/technologies`, { technologyIds })
      .pipe(map(() => undefined));
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class EducationApiService extends ApiBaseService {
  private readonly path = '/api/education';

  getPaged(query: PagedQuery): Observable<PagedResult<Education>> {
    return this.fetchPaged<Education>(this.path, query);
  }

  getById(id: number): Observable<EducationDetail> {
    return this.fetchById<EducationDetail>(this.path, id);
  }

  create(body: CreateEducationRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateEducationRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class SkillsApiService extends ApiBaseService {
  private readonly path = '/api/skills';

  getPaged(query: PagedQuery): Observable<PagedResult<Skill>> {
    return this.fetchPaged<Skill>(this.path, query);
  }

  getById(id: number): Observable<SkillDetail> {
    return this.fetchById<SkillDetail>(this.path, id);
  }

  create(body: CreateSkillRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateSkillRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class AwardsApiService extends ApiBaseService {
  private readonly path = '/api/awards';

  getPaged(query: PagedQuery): Observable<PagedResult<Award>> {
    return this.fetchPaged<Award>(this.path, query);
  }

  getById(id: number): Observable<AwardDetail> {
    return this.fetchById<AwardDetail>(this.path, id);
  }

  create(body: CreateAwardRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateAwardRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class CertificationsApiService extends ApiBaseService {
  private readonly path = '/api/certifications';

  getPaged(query: PagedQuery): Observable<PagedResult<Certification>> {
    return this.fetchPaged<Certification>(this.path, query);
  }

  getById(id: number): Observable<CertificationDetail> {
    return this.fetchById<CertificationDetail>(this.path, id);
  }

  create(body: CreateCertificationRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateCertificationRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}

@Injectable({ providedIn: 'root' })
export class DocumentsApiService extends ApiBaseService {
  private readonly path = '/api/documents';

  getPaged(query: PagedQuery): Observable<PagedResult<Document>> {
    return this.fetchPaged<Document>(this.path, query);
  }

  getById(id: number): Observable<DocumentDetail> {
    return this.fetchById<DocumentDetail>(this.path, id);
  }

  create(body: CreateDocumentRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateDocumentRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
