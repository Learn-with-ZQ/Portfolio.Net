export interface LookupItem {
  id: number;
  name: string;
}

export interface Company {
  companyId: number;
  companyName: string;
  websiteUrl?: string;
  rowVersion: string;
}

export interface CreateCompanyRequest {
  companyName: string;
  websiteUrl?: string;
}

export interface UpdateCompanyRequest extends CreateCompanyRequest {
  companyId: number;
  rowVersion: string;
}

export interface DeployDetail {
  deployDetailId: number;
  deployDetailName: string;
  deployedTo: string;
  deployedByCompanyId?: number;
  companyName?: string;
  rowVersion: string;
}

export interface CreateDeployDetailRequest {
  deployDetailName: string;
  deployedTo: string;
  deployedByCompanyId?: number;
}

export interface UpdateDeployDetailRequest extends CreateDeployDetailRequest {
  deployDetailId: number;
  rowVersion: string;
}
