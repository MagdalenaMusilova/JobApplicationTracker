// Enums
export enum ApplicationStatus {
  Applied = 0,
  PhoneScreen = 1,
  Interview = 2,
  TechnicalAssessment = 3,
  FinalRound = 4,
  OfferReceived = 5,
  Accepted = 6,
  Rejected = 7,
  Withdrawn = 8,
}

export enum WorkMode {
  OnSite = 0,
  Remote = 1,
  Hybrid = 2,
}

export enum ApplicationEventType {
  Interview = 0,
  PhoneScreen = 1,
  TechnicalAssessment = 2,
  FollowUp = 3,
  Other = 4,
}

// Auth DTOs
export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponseDto {
  token: string;
  refreshToken: string;
  expiresAt: string;
}

export interface RefreshTokenDto {
  refreshToken: string;
}

// Application DTOs
export interface JobApplicationDto {
  id: string;
  companyName: string;
  jobTitle: string;
  jobDescription?: string;
  notes?: string;
  createdAt: string;
  updatedAt: string;
  currentStatus: ApplicationStatus;
  statusHistory: ApplicationStatusHistoryDto[];
  event?: ApplicationEventDto;
}

export interface JobApplicationListDto {
  id: string;
  companyName: string;
  jobTitle: string;
  currentStatus: ApplicationStatus;
  appliedDate: string;
  updatedAt: string;
  nextEventDate?: string;
  nextEventType?: ApplicationEventType;
}

// User Resume DTO - matches backend exactly
export interface UserResumeDto {
  id: string;
  userId: string;
  workExperiences: WorkExperienceDto[];
  education: EducationDto[];
  trainings: TrainingDto[];
  skills: JobSkillDto[];
  uncategorizedSkillUsages: SkillUsageDto[];
}

export interface WorkExperienceDto {
  id?: string;
  startDate?: string;
  endDate?: string;
  company?: string;
  position?: string;
  jobDescription: string[];
  skills: SkillUsageDto[];
  notes?: string;
}

export interface EducationDto {
  id?: string;
  degree?: string;
  isFinished: boolean;
  school?: string;
  majors: string[];
  skills: SkillUsageDto[];
  notes?: string;
}

export interface TrainingDto {
  id?: string;
  startDate?: string;
  endDate?: string;
  name?: string;
  type?: string;
  certification?: string[];
  skills: SkillUsageDto[];
  notes?: string;
}

export interface JobSkillDto {
  id?: string;
  name?: string;
  aliases?: string[];
  skills: SkillUsageDto[];
  notes?: string;
}

export interface SkillUsageDto {
  id?: string;
  skill: JobSkillDto;
  description: string;
}

// User Account DTO - includes auth fields
export interface UserAccountDto {
  id: string;
  username: string;
  email: string;
  createdAt: string;
  resume?: UserResumeDto;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface CreateJobApplicationDto {
  companyName: string;
  jobTitle: string;
  jobDescription?: string;
  notes?: string;
}

export interface UpdateJobApplicationDto {
  companyName?: string;
  jobTitle?: string;
  jobDescription?: string;
  notes?: string;
}

// Status History DTOs
export interface ApplicationStatusHistoryDto {
  id: string;
  applicationId: string;
  status: ApplicationStatus;
  changedAt: string;
  notes?: string;
}

export interface UpdateStatusDto {
  newStatus: ApplicationStatus;
  notes?: string;
}

// Event DTOs
export interface ApplicationEventDto {
  id: string;
  applicationId: string;
  eventType: ApplicationEventType;
  scheduledAt: string;
  description?: string;
  location?: string;
  notes?: string;
  isCompleted: boolean;
  createdAt: string;
  companyName?: string;
  jobTitle?: string;
}

export interface CreateApplicationEventDto {
  applicationId: string;
  eventType: ApplicationEventType;
  scheduledAt: string;
  description?: string;
  location?: string;
  notes?: string;
}

export interface UpdateApplicationEventDto {
  eventType?: ApplicationEventType;
  scheduledAt?: string;
  description?: string;
  location?: string;
  notes?: string;
  isCompleted?: boolean;
}

// User Profile DTOs
export interface UserProfileDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  linkedInUrl?: string;
  portfolioUrl?: string;
  currentJobTitle?: string;
  yearsOfExperience?: number;
  skills: string[];
  preferredWorkModes: WorkMode[];
  preferredLocations: string[];
  salaryExpectationMin?: number;
  salaryExpectationMax?: number;
  cvFileName?: string;
  cvUploadedAt?: string;
}

export interface UpdateUserProfileDto {
  firstName?: string;
  lastName?: string;
  phone?: string;
  linkedInUrl?: string;
  portfolioUrl?: string;
  currentJobTitle?: string;
  yearsOfExperience?: number;
  skills?: string[];
  preferredWorkModes?: WorkMode[];
  preferredLocations?: string[];
  salaryExpectationMin?: number;
  salaryExpectationMax?: number;
}

// Match DTOs
export interface JobMatchResultDto {
  matchScore: number;
  matchReasons: string[];
  missingSkills: string[];
  recommendations: string[];
  overallAssessment: string;
}

export interface JobMatchDto {
  companyName: string;
  jobTitle: string;
  jobUrl: string;
  location?: string;
  workMode: WorkMode;
  salaryMin?: number;
  salaryMax?: number;
  matchScore: number;
  matchReasons: string[];
  source: string;
  postedDate?: string;
}

export interface MatchFilterDto {
  keywords?: string[];
  locations?: string[];
  workModes?: WorkMode[];
  minSalary?: number;
  sources?: string[];
}

// Dashboard DTOs
export interface DashboardStatsDto {
  totalApplications: number;
  activeApplications: number;
  interviewsScheduled: number;
  offersReceived: number;
  applicationsByStatus: Record<ApplicationStatus, number>;
  applicationsThisWeek: number;
  applicationsThisMonth: number;
}

// Paginated Response
export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

// Filter/Search params for applications
export interface ApplicationFilterParams {
  search?: string;
  statuses?: ApplicationStatus[];
  dateFrom?: string;
  dateTo?: string;
  sortBy?: 'appliedDate' | 'companyName' | 'status' | 'updatedAt';
  sortDirection?: 'asc' | 'desc';
  pageNumber?: number;
  pageSize?: number;
}

// Helper functions for enums
export const applicationStatusLabels: Record<ApplicationStatus, string> = {
  [ApplicationStatus.Applied]: 'Applied',
  [ApplicationStatus.PhoneScreen]: 'Phone Screen',
  [ApplicationStatus.Interview]: 'Interview',
  [ApplicationStatus.TechnicalAssessment]: 'Technical Assessment',
  [ApplicationStatus.FinalRound]: 'Final Round',
  [ApplicationStatus.OfferReceived]: 'Offer Received',
  [ApplicationStatus.Accepted]: 'Accepted',
  [ApplicationStatus.Rejected]: 'Rejected',
  [ApplicationStatus.Withdrawn]: 'Withdrawn',
};

export const workModeLabels: Record<WorkMode, string> = {
  [WorkMode.OnSite]: 'On-Site',
  [WorkMode.Remote]: 'Remote',
  [WorkMode.Hybrid]: 'Hybrid',
};

export const eventTypeLabels: Record<ApplicationEventType, string> = {
  [ApplicationEventType.Interview]: 'Interview',
  [ApplicationEventType.PhoneScreen]: 'Phone Screen',
  [ApplicationEventType.TechnicalAssessment]: 'Technical Assessment',
  [ApplicationEventType.FollowUp]: 'Follow Up',
  [ApplicationEventType.Other]: 'Other',
};

export const applicationStatusColors: Record<ApplicationStatus, string> = {
  [ApplicationStatus.Applied]: 'bg-blue-500/20 text-blue-400 border-blue-500/30',
  [ApplicationStatus.PhoneScreen]: 'bg-cyan-500/20 text-cyan-400 border-cyan-500/30',
  [ApplicationStatus.Interview]: 'bg-indigo-500/20 text-indigo-400 border-indigo-500/30',
  [ApplicationStatus.TechnicalAssessment]: 'bg-purple-500/20 text-purple-400 border-purple-500/30',
  [ApplicationStatus.FinalRound]: 'bg-amber-500/20 text-amber-400 border-amber-500/30',
  [ApplicationStatus.OfferReceived]: 'bg-emerald-500/20 text-emerald-400 border-emerald-500/30',
  [ApplicationStatus.Accepted]: 'bg-green-500/20 text-green-400 border-green-500/30',
  [ApplicationStatus.Rejected]: 'bg-red-500/20 text-red-400 border-red-500/30',
  [ApplicationStatus.Withdrawn]: 'bg-gray-500/20 text-gray-400 border-gray-500/30',
};

export const eventTypeColors: Record<ApplicationEventType, string> = {
  [ApplicationEventType.Interview]: 'bg-indigo-500/20 text-indigo-400 border-indigo-500/30',
  [ApplicationEventType.PhoneScreen]: 'bg-cyan-500/20 text-cyan-400 border-cyan-500/30',
  [ApplicationEventType.TechnicalAssessment]: 'bg-purple-500/20 text-purple-400 border-purple-500/30',
  [ApplicationEventType.FollowUp]: 'bg-amber-500/20 text-amber-400 border-amber-500/30',
  [ApplicationEventType.Other]: 'bg-gray-500/20 text-gray-400 border-gray-500/30',
};
