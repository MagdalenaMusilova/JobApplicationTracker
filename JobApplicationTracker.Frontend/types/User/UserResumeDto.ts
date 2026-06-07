export interface UserResumeDto {
  id: string;
  userId: string;
  firstName: string;
  lastName: string;
  summary?: string;
  skills: string[];
  experience: WorkExperienceDto[];
  education: EducationDto[];
  projects: ProjectDto[];
  languages: string[];
  links: string[];
  cvFileName?: string;
  cvUploadedAt?: string;
}

export interface WorkExperienceDto {
  id: string;
  company: string;
  position: string;
  location?: string;
  startDate: string;
  endDate?: string;
  isCurrent: boolean;
  description?: string;
}

export interface EducationDto {
  id: string;
  institution: string;
  degree: string;
  fieldOfStudy?: string;
  startDate: string;
  endDate?: string;
  description?: string;
}

export interface ProjectDto {
  id: string;
  name: string;
  description?: string;
  url?: string;
  technologies: string[];
}
