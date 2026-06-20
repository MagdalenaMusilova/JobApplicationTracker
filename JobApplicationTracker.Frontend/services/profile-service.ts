import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import { UserAccountDto } from '@/types/User/UserAccountDto';
import { UserResumeDto } from '@/types/User/UserResumeDto';
import { ChangePasswordDto } from '@/types/User/ChangePasswordDto';
import { WorkExperienceDto, EducationDto, TrainingDto, JobSkillDto } from '@/types/User/Resume/UserResumeDto';

export const profileService = {
  // Get user account with resume
  async getAccount(): Promise<UserAccountDto> {
    const response = await httpClient.get<UserAccountDto>(
        API_ENDPOINTS.PROFILE.BASE
    );
    return response.data;
  },

  // Get just the resume
  async getResume(): Promise<UserResumeDto> {
    const response = await httpClient.get<UserResumeDto>(
        `${API_ENDPOINTS.PROFILE.BASE}/resume`
    );
    return response.data;
  },

  // Update resume
  async updateResume(data: Partial<UserResumeDto>): Promise<UserResumeDto> {
    const response = await httpClient.put<UserResumeDto>(
        `${API_ENDPOINTS.PROFILE.BASE}/resume`,
        data
    );
    return response.data;
  },

  // Change password
  async changePassword(data: ChangePasswordDto): Promise<void> {
    await httpClient.post(
        `${API_ENDPOINTS.PROFILE.BASE}/change-password`,
        data
    );
  },

  // Update email
  async updateEmail(newEmail: string): Promise<UserAccountDto> {
    const response = await httpClient.put<UserAccountDto>(
        `${API_ENDPOINTS.PROFILE.BASE}/email`,
        { email: newEmail }
    );
    return response.data;
  },

  // Update username
  async updateUsername(newUsername: string): Promise<UserAccountDto> {
    const response = await httpClient.put<UserAccountDto>(
        `${API_ENDPOINTS.PROFILE.BASE}/username`,
        { username: newUsername }
    );
    return response.data;
  },

  async uploadCV(file: File): Promise<{ fileName: string; uploadedAt: string }> {
    const formData = new FormData();
    formData.append('file', file);

    const response = await httpClient.post<{ fileName: string; uploadedAt: string }>(
        API_ENDPOINTS.PROFILE.UPLOAD_CV,
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        }
    );
    return response.data;
  },

  async downloadCV(): Promise<Blob> {
    const response = await httpClient.get(
        API_ENDPOINTS.PROFILE.DOWNLOAD_CV,
        { responseType: 'blob' }
    );
    return response.data;
  },

  async deleteCV(): Promise<void> {
    await httpClient.delete(API_ENDPOINTS.PROFILE.UPLOAD_CV);
  },

  // Delete user resume (clear all resume data)
  async deleteResume(resumeId: string): Promise<void> {
    await httpClient.delete(`${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}`);
  },

  // Extract resume data from PDF
  async extractFromPdf(file: File): Promise<UserResumeDto> {
    const formData = new FormData();
    formData.append('file', file);

    const response = await httpClient.post<UserResumeDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/extract`,
      formData,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }
    );
    return response.data;
  },

  // Work Experience CRUD
  async addWorkExperience(resumeId: string, data: WorkExperienceDto): Promise<WorkExperienceDto> {
    const response = await httpClient.post<WorkExperienceDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/work-experience`,
      data
    );
    return response.data;
  },

  async updateWorkExperience(resumeId: string, experienceId: string, data: WorkExperienceDto): Promise<WorkExperienceDto> {
    const response = await httpClient.put<WorkExperienceDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/work-experience/${experienceId}`,
      data
    );
    return response.data;
  },

  async deleteWorkExperience(resumeId: string, experienceId: string): Promise<void> {
    await httpClient.delete(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/work-experience/${experienceId}`
    );
  },

  // Education CRUD
  async addEducation(resumeId: string, data: EducationDto): Promise<EducationDto> {
    const response = await httpClient.post<EducationDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/education`,
      data
    );
    return response.data;
  },

  async updateEducation(resumeId: string, educationId: string, data: EducationDto): Promise<EducationDto> {
    const response = await httpClient.put<EducationDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/education/${educationId}`,
      data
    );
    return response.data;
  },

  async deleteEducation(resumeId: string, educationId: string): Promise<void> {
    await httpClient.delete(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/education/${educationId}`
    );
  },

  // Training CRUD
  async addTraining(resumeId: string, data: TrainingDto): Promise<TrainingDto> {
    const response = await httpClient.post<TrainingDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/training`,
      data
    );
    return response.data;
  },

  async updateTraining(resumeId: string, trainingId: string, data: TrainingDto): Promise<TrainingDto> {
    const response = await httpClient.put<TrainingDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/training/${trainingId}`,
      data
    );
    return response.data;
  },

  async deleteTraining(resumeId: string, trainingId: string): Promise<void> {
    await httpClient.delete(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/training/${trainingId}`
    );
  },

  // Skill CRUD
  async addSkill(resumeId: string, data: JobSkillDto): Promise<JobSkillDto> {
    const response = await httpClient.post<JobSkillDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/skill`,
      data
    );
    return response.data;
  },

  async updateSkill(resumeId: string, skillId: string, data: JobSkillDto): Promise<JobSkillDto> {
    const response = await httpClient.put<JobSkillDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/skill/${skillId}`,
      data
    );
    return response.data;
  },

  async deleteSkill(resumeId: string, skillId: string): Promise<void> {
    await httpClient.delete(
      `${API_ENDPOINTS.PROFILE.BASE}/resume/${resumeId}/skill/${skillId}`
    );
  },
};