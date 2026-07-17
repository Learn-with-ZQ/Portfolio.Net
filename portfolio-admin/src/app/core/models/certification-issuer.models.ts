import { LookupItem } from './lookup.models';

export interface CertificationIssuer {
  certificationIssuerId: number;
  issuerName: string;
  issuerWebsite?: string;
  rowVersion: string;
}

export interface CreateCertificationIssuerRequest {
  issuerName: string;
  issuerWebsite?: string;
}

export interface UpdateCertificationIssuerRequest extends CreateCertificationIssuerRequest {
  certificationIssuerId: number;
  rowVersion: string;
}
