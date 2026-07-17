import { LookupItem } from './lookup.models';

export interface DocumentType {
  documentTypeId: number;
  typeName: string;
  rowVersion: string;
}

export interface CreateDocumentTypeRequest {
  typeName: string;
}

export interface UpdateDocumentTypeRequest extends CreateDocumentTypeRequest {
  documentTypeId: number;
  rowVersion: string;
}

export type { LookupItem };
