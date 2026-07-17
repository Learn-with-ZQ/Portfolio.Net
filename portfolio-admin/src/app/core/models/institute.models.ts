import { LookupItem } from './lookup.models';

export interface Institute {
  instituteId: number;
  instituteName: string;
  location?: string;
  rowVersion: string;
}

export interface CreateInstituteRequest {
  instituteName: string;
  location?: string;
}

export interface UpdateInstituteRequest extends CreateInstituteRequest {
  instituteId: number;
  rowVersion: string;
}
