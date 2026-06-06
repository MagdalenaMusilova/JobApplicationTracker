import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  JobApplicationMinimalDto
} from '@/types';

export const applicationService = {
  async getAllMinimal(archived: boolean): Promise<JobApplicationMinimalDto> {
    const response = await httpClient.get<JobApplicationMinimalDto>(
        archived ? API_ENDPOINTS.APPLICATIONS.MINIMAL_ARCHIVED : API_ENDPOINTS.APPLICATIONS.MINIMAL,
    );
    return response.data;
  },
  
  async create(data: CreateJobApplicationDto): Promise<JobApplicationDto> {
    const response = await httpClient.post<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.BASE,
        data
    );
    return response.data;
  },
  
  async getById(applicationId): Promise<JobApplicationDto> {
    const response = await httpClient.get<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.BY_ID(applicationId)
    );
    return response.data;
  },

  async pushStatus(data: UpdateStatusDto): Promise<JobApplicationDto> {
    const response = await httpClient.post<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.PUSH_STATUS,
        data
    );
    return response.data;
  }
};