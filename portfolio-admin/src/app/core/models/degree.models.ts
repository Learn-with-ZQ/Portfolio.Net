import { LookupItem } from './lookup.models';

export interface Degree {
  degreeId: number;
  degreeName: string;
  rowVersion: string;
}

export interface CreateDegreeRequest {
  degreeName: string;
}

export interface UpdateDegreeRequest extends CreateDegreeRequest {
  degreeId: number;
  rowVersion: string;
}

export type { LookupItem };
