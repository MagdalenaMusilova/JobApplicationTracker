import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  DashboardStatsDto,
  JobApplicationListDto,
} from '@/types';
import {
  mockDashboardStats,
  getRecentApplications,
} from '@/lib/mock-data';

// Mock delay for simulating API calls
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Flag to use mock data
const USE_MOCK = true;

export const dashboardService = {
  async getStats(): Promise<DashboardStatsDto> {
    if (USE_MOCK) {
      await delay(500);
      return mockDashboardStats;
    }
    
    const response = await httpClient.get<DashboardStatsDto>(
      API_ENDPOINTS.DASHBOARD.STATS
    );
    return response.data;
  },

  async getRecent(): Promise<JobApplicationListDto[]> {
    if (USE_MOCK) {
      await delay(400);
      return getRecentApplications();
    }
    
    const response = await httpClient.get<JobApplicationListDto[]>(
      API_ENDPOINTS.DASHBOARD.RECENT
    );
    return response.data;
  },
};
