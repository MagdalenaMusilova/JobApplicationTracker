export interface JobMatchDto {
  company: string;
  position: string;
  jobUrl: string;
  location?: string;
  workMode: number;
  salaryMin?: number;
  salaryMax?: number;
  matchScore: number;
  matchReasons: string[];
  source: string;
  postedDate?: string;
}

export interface JobMatchResultDto {
  matchScore: number;
  matchReasons: string[];
  missingSkills: string[];
  recommendations: string[];
  overallAssessment: string;
}
