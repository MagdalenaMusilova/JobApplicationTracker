export interface UserProfileDto {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  linkedInUrl?: string;
  portfolioUrl?: string;
  currentPosition?: string;
  yearsOfExperience?: number;
  skills: string[];
  preferredWorkModes: number[];
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
  currentPosition?: string;
  yearsOfExperience?: number;
  skills?: string[];
  preferredWorkModes?: number[];
  preferredLocations?: string[];
  salaryExpectationMin?: number;
  salaryExpectationMax?: number;
}
