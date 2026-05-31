import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  UserAccountDto,
  UserResumeDto,
  ChangePasswordDto,
} from '@/types';
import { mockUserAccount, mockUserResume } from '@/lib/mock-data';

// Mock delay for simulating API calls
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Flag to use mock data
const USE_MOCK = true;

// Store for mock data updates
let currentAccount = { ...mockUserAccount };
let currentResume = { ...mockUserResume };

export const profileService = {
  // Get user account with resume
  async getAccount(): Promise<UserAccountDto> {
    if (USE_MOCK) {
      await delay(400);
      return { ...currentAccount, resume: currentResume };
    }
    
    const response = await httpClient.get<UserAccountDto>(
      API_ENDPOINTS.PROFILE.BASE
    );
    return response.data;
  },

  // Get just the resume
  async getResume(): Promise<UserResumeDto> {
    if (USE_MOCK) {
      await delay(300);
      return currentResume;
    }
    
    const response = await httpClient.get<UserResumeDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume`
    );
    return response.data;
  },

  // Update resume
  async updateResume(data: Partial<UserResumeDto>): Promise<UserResumeDto> {
    if (USE_MOCK) {
      await delay(600);
      currentResume = {
        ...currentResume,
        ...data,
      };
      return currentResume;
    }
    
    const response = await httpClient.put<UserResumeDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/resume`,
      data
    );
    return response.data;
  },

  // Change password
  async changePassword(data: ChangePasswordDto): Promise<void> {
    if (USE_MOCK) {
      await delay(800);
      // Simulate password validation
      if (data.currentPassword !== 'password123') {
        throw new Error('Current password is incorrect');
      }
      if (data.newPassword !== data.confirmNewPassword) {
        throw new Error('New passwords do not match');
      }
      return;
    }
    
    await httpClient.post(
      `${API_ENDPOINTS.PROFILE.BASE}/change-password`,
      data
    );
  },

  // Update email
  async updateEmail(newEmail: string): Promise<UserAccountDto> {
    if (USE_MOCK) {
      await delay(500);
      currentAccount = {
        ...currentAccount,
        email: newEmail,
      };
      return currentAccount;
    }
    
    const response = await httpClient.put<UserAccountDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/email`,
      { email: newEmail }
    );
    return response.data;
  },

  // Update username
  async updateUsername(newUsername: string): Promise<UserAccountDto> {
    if (USE_MOCK) {
      await delay(500);
      currentAccount = {
        ...currentAccount,
        username: newUsername,
      };
      return currentAccount;
    }
    
    const response = await httpClient.put<UserAccountDto>(
      `${API_ENDPOINTS.PROFILE.BASE}/username`,
      { username: newUsername }
    );
    return response.data;
  },

  async uploadCV(file: File): Promise<{ fileName: string; uploadedAt: string }> {
    if (USE_MOCK) {
      await delay(1500);
      return {
        fileName: file.name,
        uploadedAt: new Date().toISOString(),
      };
    }
    
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
    if (USE_MOCK) {
      await delay(500);
      return new Blob(['Mock CV content'], { type: 'application/pdf' });
    }
    
    const response = await httpClient.get(
      API_ENDPOINTS.PROFILE.DOWNLOAD_CV,
      { responseType: 'blob' }
    );
    return response.data;
  },

  async deleteCV(): Promise<void> {
    if (USE_MOCK) {
      await delay(400);
      return;
    }
    
    await httpClient.delete(API_ENDPOINTS.PROFILE.UPLOAD_CV);
  },
};
