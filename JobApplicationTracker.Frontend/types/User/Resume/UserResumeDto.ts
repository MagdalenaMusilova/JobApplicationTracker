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
  id?: string | null;
  startDate?: string | null;
  endDate?: string | null;
  company?: string | null;
  position?: string | null;
  jobDescription: string[];
  skills: SkillUsageDto[];
  notes?: string | null;
}

export interface EducationDto {
  id?: string | null;
  degree?: string | null;
  isFinished: boolean;
  school?: string | null;
  majors: string[];
  skills: SkillUsageDto[];
  notes?: string | null;
}

export interface TrainingDto {
  id?: string | null;
  startDate?: string | null;
  endDate?: string | null;
  name?: string | null;
  type?: string | null;
  certification?: string[] | null;
  skills: SkillUsageDto[];
  notes?: string | null;
}

export interface JobSkillDto {
  id?: string | null;
  name?: string | null;
  aliases?: string[] | null;
  skills: SkillUsageDto[];
  notes?: string | null;
}

export interface SkillUsageDto {
  id?: string | null;
  skill: JobSkillDto;
  description: string;
}