export interface ApiEnvelope<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors: string[];
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
}

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
  expiresInSeconds: number;
  tokenType: string;
  roles: string[];
}

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface PagedQuery {
  portfolioProfileId: number;
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  isPublic?: boolean;
  isPublished?: boolean;
  status?: number;
  category?: string;
}
