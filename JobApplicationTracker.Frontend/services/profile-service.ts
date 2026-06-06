import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  UserAccountDto,
  UserResumeDto,
  ChangePasswordDto,
} from '@/types';

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
};