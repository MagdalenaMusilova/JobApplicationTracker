import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import { DashboardStatsDto } from '@/types/Dashboard/DashboardStatsDto';
import { JobApplicationMinimalDto } from '@/types/JAObjects/JobApplications/JobApplicationMinimalDto';

export const dashboardService = {
  async getStats(): Promise<DashboardStatsDto> {
    const response = await httpClient.get<DashboardStatsDto>(
        API_ENDPOINTS.DASHBOARD.STATS
    );
    return response.data;
  },

  async getRecent(): Promise<JobApplicationMinimalDto[]> {
    const response = await httpClient.get<JobApplicationMinimalDto[]>(
        API_ENDPOINTS.DASHBOARD.RECENT
    );
    return response.data;
  },
};