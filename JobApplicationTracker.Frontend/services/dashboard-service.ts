import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  DashboardStatsDto,
  JobApplicationListDto,
} from '@/types';

export const dashboardService = {
  async getStats(): Promise<DashboardStatsDto> {
    const response = await httpClient.get<DashboardStatsDto>(
        API_ENDPOINTS.DASHBOARD.STATS
    );
    return response.data;
  },

  async getRecent(): Promise<JobApplicationListDto[]> {
    const response = await httpClient.get<JobApplicationListDto[]>(
        API_ENDPOINTS.DASHBOARD.RECENT
    );
    return response.data;
  },
};