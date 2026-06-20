import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import { JobApplicationDto } from '@/types/JAObjects/JobApplications/JobApplicationDto';
import { JobApplicationMinimalDto } from '@/types/JAObjects/JobApplications/JobApplicationMinimalDto';
import { CreateJobApplicationDto } from '@/types/JAObjects/JobApplications/CreateJobApplicationDto';
import { UpdateJobApplicationDto } from '@/types/JAObjects/JobApplications/UpdateJobApplicationDto';
import { CreateJAStatusEntryDto } from '@/types/JAObjects/JAStatuses/CreateJAStatusEntryDto';
import { JAStatusEntryDto } from '@/types/JAObjects/JAStatuses/JAStatusEntryDto';

export const applicationService = {
  async getAll(): Promise<JobApplicationDto[]> {
    const response = await httpClient.get<JobApplicationDto[]>(API_ENDPOINTS.APPLICATIONS.BASE);
    return response.data;
  },

  async getAllFull(): Promise<JobApplicationDto[]> {
    const response = await httpClient.get<JobApplicationDto[]>(API_ENDPOINTS.APPLICATIONS.ALL);
    return response.data;
  },

  async getNotFinished(): Promise<JobApplicationMinimalDto[]> {
    const response = await httpClient.get<JobApplicationMinimalDto[]>(API_ENDPOINTS.APPLICATIONS.NOT_FINISHED);
    return response.data;
  },

  async getAllMinimal(mode: 'active' | 'archived' | 'all' = 'active'): Promise<JobApplicationMinimalDto[]> {
    let endpoint: string;
    switch (mode) {
      case 'archived':
        endpoint = API_ENDPOINTS.APPLICATIONS.MINIMAL_ARCHIVED;
        break;
      case 'all':
        endpoint = API_ENDPOINTS.APPLICATIONS.MINIMAL_ALL;
        break;
      default:
        endpoint = API_ENDPOINTS.APPLICATIONS.MINIMAL;
    }

    const response = await httpClient.get<JobApplicationMinimalDto[]>(endpoint);
    return response.data;
  },
  
  async create(data: CreateJobApplicationDto): Promise<JobApplicationDto> {
    const response = await httpClient.post<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.BASE,
        data
    );
    return response.data;
  },
  
  async getById(applicationId: string): Promise<JobApplicationDto> {
    const response = await httpClient.get<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.BY_ID(applicationId)
    );
    return response.data;
  },

  async update(applicationId: string, data: UpdateJobApplicationDto): Promise<JobApplicationDto> {
    const response = await httpClient.put<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.BY_ID(applicationId),
        data
    );
    return response.data;
  },

  async delete(applicationId: string): Promise<void> {
    await httpClient.delete(API_ENDPOINTS.APPLICATIONS.BY_ID(applicationId));
  },

  async deny(applicationId: string): Promise<JobApplicationDto> {
    const response = await httpClient.put<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.DENY(applicationId)
    );
    return response.data;
  },

  async updateStatus(data: CreateJAStatusEntryDto): Promise<JobApplicationDto> {
    const response = await httpClient.post<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.PUSH_STATUS,
        data
    );
    return response.data;
  },

  async updateStatusEntry(entryId: string, data: CreateJAStatusEntryDto): Promise<JAStatusEntryDto> {
    const response = await httpClient.put<JAStatusEntryDto>(
        API_ENDPOINTS.APPLICATIONS.STATUS_ENTRY(entryId),
        data
    );
    return response.data;
  },

  async deleteStatusEntry(entryId: string): Promise<JobApplicationDto> {
    const response = await httpClient.delete<JobApplicationDto>(
        API_ENDPOINTS.APPLICATIONS.STATUS_ENTRY(entryId)
    );
    return response.data;
  }
};