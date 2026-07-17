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

export interface CommandResult {
  statusCode: number;
  statusMessage: string;
  id?: number;
  isSuccess: boolean;
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
  status?: number;
  isPublished?: boolean;
  isPublic?: boolean;
  category?: string;
  eventType?: string;
  paragraphTypeId?: number;
  isActive?: boolean;
}
