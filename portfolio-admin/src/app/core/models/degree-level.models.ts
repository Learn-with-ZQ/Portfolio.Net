import { LookupItem } from './lookup.models';

export interface DegreeLevel {
  degreeLevelId: number;
  degreeLevelName: string;
  degreePrefix: string;
  sortOrder: number;
  rowVersion: string;
}

export interface CreateDegreeLevelRequest {
  degreeLevelName: string;
  degreePrefix: string;
  sortOrder: number;
}

export interface UpdateDegreeLevelRequest extends CreateDegreeLevelRequest {
  degreeLevelId: number;
  rowVersion: string;
}
