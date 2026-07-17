/**
 * Read-only view models for the public website. These mirror the *Dto shapes
 * returned by the Portfolio API `/paged` and `/{id}` endpoints.
 */

export interface Experience {
  experienceId: number;
  designation: string;
  companyName?: string;
  startDate: string;
  endDate?: string;
  isCurrent: boolean;
  sortOrder: number;
}

export interface Project {
  projectId: number;
  projectName: string;
  contextName?: string;
  /** Comma-separated technology names as returned by the API. */
  technologies: string;
  startDate: string;
  endDate?: string;
  sortOrder: number;
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

export interface Skill {
  skillId: number;
  skillName: string;
  sortOrder: number;
  detailCount: number;
}

export interface Award {
  awardId: number;
  awardName: string;
  startDate: string;
  endDate?: string;
  sortOrder: number;
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

export interface DocumentItem {
  documentId: number;
  documentType: string;
  documentTitle: string;
  fileName: string;
  fileExtension: string;
  fileSizeBytes?: number;
  /** Public path/URL to the file (now returned by the list endpoint). */
  storagePath?: string;
  isPublic: boolean;
  /** When false, the site shows view-only (no download action). */
  isDownloadable: boolean;
  versionNumber: number;
  sortOrder: number;
  createdAt: string;
}

/** Full document (GET /api/documents/{id}) — includes the storage path. */
export interface DocumentDetail extends DocumentItem {
  portfolioProfileId: number;
  documentTypeId: number;
  storagePath: string;
  mimeType?: string;
  rowVersion: string;
}

/* --- Enriched shapes for the generated resume (GET /{module}/{id}) -------- */

export interface ExperienceDetailLine {
  experienceDetailName: string;
  sortOrder: number;
}

export interface ExperienceFull extends Experience {
  portfolioProfileId: number;
  deployedTo?: string;
  details: ExperienceDetailLine[];
}

export interface SkillDetailLine {
  skillDetailName: string;
  sortOrder: number;
}

export interface SkillFull extends Skill {
  details: SkillDetailLine[];
}

export interface ProjectDetailLine {
  projectDetailName: string;
  sortOrder: number;
}

export interface ProjectTechItem {
  technologyName: string;
}

/** Full project (GET /api/projects/{id}) — summary, tech list, highlights. */
export interface ProjectFull {
  projectId: number;
  portfolioProfileId: number;
  projectName: string;
  projectSummary?: string;
  practice?: string;
  companyName?: string;
  courseName?: string;
  startDate: string;
  endDate?: string;
  sortOrder: number;
  technologies: ProjectTechItem[];
  details: ProjectDetailLine[];
}

export interface CourseItem {
  courseName: string;
  sortOrder: number;
}

/** Full education (GET /api/education/{id}) — includes courses. */
export interface EducationFull {
  educationId: number;
  portfolioProfileId: number;
  degreeLevelName: string;
  degreePrefix: string;
  degreeName: string;
  instituteName: string;
  location?: string;
  startDate: string;
  endDate?: string;
  gpa?: number;
  cgpa?: number;
  sortOrder: number;
  courses: CourseItem[];
}

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
  createdAt: string;
}

export interface SubmitTestimonial {
  authorName: string;
  authorTitle?: string;
  authorCompany?: string;
  authorEmail?: string;
  relationship?: string;
  rating?: number;
  content: string;
  linkedInUrl?: string;
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

/** Everything the resume page renders / exports, in one payload. */
export interface ResumeData {
  experience: ExperienceFull[];
  projects: Project[];
  education: Education[];
  skills: SkillFull[];
  certifications: Certification[];
  awards: Award[];
}
