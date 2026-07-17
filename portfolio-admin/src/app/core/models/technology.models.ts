import { LookupItem } from './lookup.models';

export interface Technology {
  technologyId: number;
  technologyName: string;
  category?: string;
  rowVersion: string;
}

export interface CreateTechnologyRequest {
  technologyName: string;
  category?: string;
}

export interface UpdateTechnologyRequest extends CreateTechnologyRequest {
  technologyId: number;
  rowVersion: string;
}

export type { LookupItem };
