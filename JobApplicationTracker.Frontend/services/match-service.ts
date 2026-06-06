import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  JobMatchDto,
  MatchFilterDto,
} from '@/types';

export const matchService = {
  async getMatches(): Promise<JobMatchDto[]> {
    const response = await httpClient.get<JobMatchDto[]>(
        API_ENDPOINTS.MATCH.BASE
    );
    return response.data;
  },

  async refreshMatches(): Promise<JobMatchDto[]> {
    const response = await httpClient.post<JobMatchDto[]>(
        API_ENDPOINTS.MATCH.REFRESH
    );
    return response.data;
  },

  async findMatches(filters?: MatchFilterDto): Promise<JobMatchDto[]> {
    const response = await httpClient.post<JobMatchDto[]>(
        API_ENDPOINTS.MATCH.FIND,
        filters
    );
    return response.data;
  },
};