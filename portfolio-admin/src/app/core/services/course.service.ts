import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedQuery, PagedResult } from '../models/api.models';
import { Course, CreateCourseRequest, LookupItem, UpdateCourseRequest } from '../models/course.models';
import { ApiBaseService } from './api-base.service';

@Injectable({ providedIn: 'root' })
export class CourseApiService extends ApiBaseService {
  private readonly path = '/api/courses';

  lookup(): Observable<LookupItem[]> {
    return this.fetchLookup(this.path);
  }

  getPaged(query: PagedQuery): Observable<PagedResult<Course>> {
    return this.fetchPaged<Course>(this.path, query);
  }

  getById(id: number): Observable<Course> {
    return this.fetchById<Course>(this.path, id);
  }

  create(body: CreateCourseRequest) {
    return this.postItem(this.path, body);
  }

  update(id: number, body: UpdateCourseRequest) {
    return this.putItem(this.path, id, body);
  }

  delete(id: number) {
    return this.removeItem(this.path, id);
  }
}
