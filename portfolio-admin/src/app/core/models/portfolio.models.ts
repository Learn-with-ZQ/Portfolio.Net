export interface Experience {
  experienceId: number;
  designation: string;
  companyName?: string;
  startDate: string;
  endDate?: string;
  isCurrent: boolean;
  sortOrder: number;
}

export interface ExperienceDetail extends Experience {
  portfolioProfileId: number;
  companyId?: number;
  deployDetailId?: number;
  deployDetailName?: string;
  deployedTo?: string;
  rowVersion: string;
}

export interface CreateExperienceRequest {
  portfolioProfileId: number;
  designation: string;
  companyId?: number;
  deployDetailId?: number;
  startDate: string;
  endDate?: string;
  sortOrder: number;
}

export interface UpdateExperienceRequest extends CreateExperienceRequest {
  experienceId: number;
  rowVersion: string;
}

export interface Project {
  projectId: number;
  projectName: string;
  contextName?: string;
  technologies: string;
  startDate: string;
  endDate?: string;
  sortOrder: number;
}

export interface ProjectTechnology {
  projectTechnologyId: number;
  technologyId: number;
  technologyName: string;
  rowVersion: string;
}

// The GetById endpoint returns `technologies` as a structured array (unlike the
// paged list, where it is a comma-joined string), so override that field here.
export interface ProjectDetail extends Omit<Project, 'technologies'> {
  portfolioProfileId: number;
  projectSummary?: string;
  practice?: string;
  companyId?: number;
  companyName?: string;
  courseId?: number;
  courseName?: string;
  technologies: ProjectTechnology[];
  rowVersion: string;
}

export interface CreateProjectRequest {
  portfolioProfileId: number;
  projectName: string;
  projectSummary?: string;
  practice?: string;
  companyId?: number;
  courseId?: number;
  startDate: string;
  endDate?: string;
  sortOrder: number;
  technologyIds: number[];
}

export interface UpdateProjectRequest extends Omit<CreateProjectRequest, 'technologyIds'> {
  projectId: number;
  rowVersion: string;
}

export interface Education {
  educationId: number;
  degree: string;
  instituteName: string;
  startDate: string;
  endDate?: string;
  gpa?: number;
  cgpa?: number;
  sortOrder: number;
}

export interface EducationDetail extends Education {
  portfolioProfileId: number;
  degreeLevelId: number;
  degreeLevelName: string;
  degreePrefix: string;
  degreeId: number;
  degreeName: string;
  instituteId: number;
  location?: string;
  rowVersion: string;
}

export interface CreateEducationRequest {
  portfolioProfileId: number;
  degreeLevelId: number;
  degreeId: number;
  instituteId: number;
  startDate: string;
  endDate?: string;
  gpa?: number;
  cgpa?: number;
  sortOrder: number;
}

export interface UpdateEducationRequest extends CreateEducationRequest {
  educationId: number;
  rowVersion: string;
}

export interface Skill {
  skillId: number;
  skillName: string;
  sortOrder: number;
  detailCount: number;
}

export interface SkillDetail extends Skill {
  portfolioProfileId: number;
  rowVersion: string;
}

export interface CreateSkillRequest {
  portfolioProfileId: number;
  skillName: string;
  sortOrder: number;
}

export interface UpdateSkillRequest extends CreateSkillRequest {
  skillId: number;
  rowVersion: string;
}

export interface Award {
  awardId: number;
  awardName: string;
  startDate: string;
  endDate?: string;
  sortOrder: number;
}

export interface AwardDetail extends Award {
  portfolioProfileId: number;
  rowVersion: string;
}

export interface CreateAwardRequest {
  portfolioProfileId: number;
  awardName: string;
  startDate: string;
  endDate?: string;
  sortOrder: number;
}

export interface UpdateAwardRequest extends CreateAwardRequest {
  awardId: number;
  rowVersion: string;
}

export interface Certification {
  certificationId: number;
  certificationName: string;
  issuerName: string;
  issueDate: string;
  expiryDate?: string;
  doesNotExpire: boolean;
  sortOrder: number;
}

export interface CertificationDetail extends Certification {
  portfolioProfileId: number;
  certificationIssuerId: number;
  issuerWebsite?: string;
  credentialId?: string;
  credentialUrl?: string;
  rowVersion: string;
}

export interface CreateCertificationRequest {
  portfolioProfileId: number;
  certificationIssuerId: number;
  certificationName: string;
  credentialId?: string;
  credentialUrl?: string;
  issueDate: string;
  expiryDate?: string;
  doesNotExpire: boolean;
  sortOrder: number;
}

export interface UpdateCertificationRequest extends CreateCertificationRequest {
  certificationId: number;
  rowVersion: string;
}

export interface Document {
  documentId: number;
  documentType: string;
  documentTitle: string;
  fileName: string;
  fileExtension: string;
  fileSizeBytes?: number;
  isPublic: boolean;
  isDownloadable: boolean;
  versionNumber: number;
  sortOrder: number;
  createdAt: string;
}

export interface DocumentDetail extends Document {
  portfolioProfileId: number;
  documentTypeId: number;
  storagePath: string;
  mimeType?: string;
  rowVersion: string;
}

export interface CreateDocumentRequest {
  portfolioProfileId: number;
  documentTypeId: number;
  documentTitle: string;
  fileName: string;
  fileExtension: string;
  fileSizeBytes?: number;
  storagePath: string;
  mimeType?: string;
  isPublic: boolean;
  isDownloadable: boolean;
  versionNumber: number;
  sortOrder: number;
}

export interface UpdateDocumentRequest extends CreateDocumentRequest {
  documentId: number;
  rowVersion: string;
}

/* --- Platform modules (Testimonials, References, Blog, Paragraphs, Analytics) --- */

export interface Testimonial {
  testimonialId: number;
  authorName: string;
  authorTitle?: string;
  authorCompany?: string;
  relationship?: string;
  rating?: number;
  content: string;
  linkedInUrl?: string;
  status: number;
  statusName: string;
  sortOrder: number;
  createdAt: string;
}

export interface Reference {
  referenceId: number;
  fullName: string;
  organization?: string;
  designation?: string;
  relationship?: string;
  email?: string;
  phone?: string;
  linkedInUrl?: string;
  isContactVisible: boolean;
  sortOrder: number;
}

export interface ReferenceDetail extends Reference {
  portfolioProfileId: number;
  isPublic: boolean;
  rowVersion: string;
}

export interface CreateReferenceRequest {
  portfolioProfileId: number;
  fullName: string;
  organization?: string;
  designation?: string;
  relationship?: string;
  email?: string;
  phone?: string;
  linkedInUrl?: string;
  isContactVisible: boolean;
  isPublic: boolean;
  sortOrder: number;
}

export interface UpdateReferenceRequest extends CreateReferenceRequest {
  referenceId: number;
  rowVersion: string;
}

export interface BlogPost {
  blogPostId: number;
  title: string;
  slug: string;
  summary?: string;
  category?: string;
  tags?: string;
  coverImagePath?: string;
  isPublished: boolean;
  publishedAt?: string;
  readTimeMinutes?: number;
  sortOrder: number;
}

export interface BlogPostDetail extends BlogPost {
  portfolioProfileId: number;
  contentMarkdown: string;
  rowVersion: string;
}

export interface CreateBlogPostRequest {
  portfolioProfileId: number;
  title: string;
  slug: string;
  summary?: string;
  contentMarkdown: string;
  category?: string;
  tags?: string;
  coverImagePath?: string;
  isPublished: boolean;
  readTimeMinutes?: number;
  sortOrder: number;
}

export interface UpdateBlogPostRequest extends Omit<CreateBlogPostRequest, 'isPublished'> {
  blogPostId: number;
  rowVersion: string;
}

export interface Paragraph {
  paragraphId: number;
  paragraphTypeId: number;
  paragraphTypeName: string;
  paragraphTitle?: string;
  paragraphText: string;
  sortOrder: number;
  isActive: boolean;
}

export interface ParagraphDetail extends Paragraph {
  portfolioProfileId: number;
  rowVersion: string;
}

export interface CreateParagraphRequest {
  portfolioProfileId: number;
  paragraphTypeId: number;
  paragraphTitle?: string;
  paragraphText: string;
  sortOrder: number;
  isActive: boolean;
}

export interface UpdateParagraphRequest extends CreateParagraphRequest {
  paragraphId: number;
  rowVersion: string;
}

export interface CountItem {
  label: string;
  count: number;
}

export interface AnalyticsSummary {
  totalEvents: number;
  uniqueVisitors: number;
  pageViews: number;
  resumeDownloads: number;
  certificateDownloads: number;
  projectViews: number;
  blogViews: number;
  contactRequests: number;
  byCountry: CountItem[];
  byBrowser: CountItem[];
  byDevice: CountItem[];
}

export interface AnalyticsEvent {
  analyticsEventId: number;
  eventType: string;
  entityId?: number;
  path?: string;
  visitorId?: string;
  country?: string;
  city?: string;
  browser?: string;
  device?: string;
  createdAt: string;
}
