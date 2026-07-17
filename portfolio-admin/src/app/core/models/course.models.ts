import { LookupItem } from './lookup.models';

export type { LookupItem };

export interface Course {
  courseId: number;
  courseName: string;
  instituteId?: number;
  instituteName?: string;
  sortOrder: number;
  rowVersion: string;
}

export interface CreateCourseRequest {
  courseName: string;
  instituteId?: number;
  sortOrder: number;
}

export interface UpdateCourseRequest extends CreateCourseRequest {
  courseId: number;
  rowVersion: string;
}
