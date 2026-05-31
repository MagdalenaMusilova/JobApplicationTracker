import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  JobApplicationDto,
  JobApplicationListDto,
  CreateJobApplicationDto,
  UpdateJobApplicationDto,
  UpdateStatusDto,
  ApplicationFilterParams,
  PaginatedResponse,
} from '@/types';
import {
  mockApplications,
  mockApplicationsList,
  getApplicationById,
} from '@/lib/mock-data';

// Mock delay for simulating API calls
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Flag to use mock data
const USE_MOCK = true;

export const applicationService = {
  async getAll(params?: ApplicationFilterParams): Promise<PaginatedResponse<JobApplicationListDto>> {
    if (USE_MOCK) {
      await delay(600);
      
      let filtered = [...mockApplicationsList];
      
      // Apply filters
      if (params?.search) {
        const search = params.search.toLowerCase();
        filtered = filtered.filter(
          app =>
            app.companyName.toLowerCase().includes(search) ||
            app.jobTitle.toLowerCase().includes(search)
        );
      }
      
      if (params?.statuses && params.statuses.length > 0) {
        filtered = filtered.filter(app => params.statuses!.includes(app.currentStatus));
      }
      
      // Apply sorting
      if (params?.sortBy) {
        filtered.sort((a, b) => {
          let comparison = 0;
          switch (params.sortBy) {
            case 'appliedDate':
              comparison = new Date(a.appliedDate).getTime() - new Date(b.appliedDate).getTime();
              break;
            case 'companyName':
              comparison = a.companyName.localeCompare(b.companyName);
              break;
            case 'status':
              comparison = a.currentStatus - b.currentStatus;
              break;
            case 'updatedAt':
              comparison = new Date(a.updatedAt).getTime() - new Date(b.updatedAt).getTime();
              break;
            default:
              comparison = 0;
          }
          return params.sortDirection === 'desc' ? -comparison : comparison;
        });
      }
      
      // Pagination
      const pageSize = params?.pageSize || 10;
      const pageNumber = params?.pageNumber || 1;
      const totalCount = filtered.length;
      const totalPages = Math.ceil(totalCount / pageSize);
      const start = (pageNumber - 1) * pageSize;
      const items = filtered.slice(start, start + pageSize);
      
      return {
        items,
        totalCount,
        pageNumber,
        pageSize,
        totalPages,
        hasNextPage: pageNumber < totalPages,
        hasPreviousPage: pageNumber > 1,
      };
    }
    
    const response = await httpClient.get<PaginatedResponse<JobApplicationListDto>>(
      API_ENDPOINTS.APPLICATIONS.BASE,
      { params }
    );
    return response.data;
  },

  async getById(id: string): Promise<JobApplicationDto> {
    if (USE_MOCK) {
      await delay(400);
      const application = getApplicationById(id);
      if (!application) {
        throw new Error('Application not found');
      }
      return application;
    }
    
    const response = await httpClient.get<JobApplicationDto>(
      API_ENDPOINTS.APPLICATIONS.BY_ID(id)
    );
    return response.data;
  },

  async create(data: CreateJobApplicationDto): Promise<JobApplicationDto> {
    if (USE_MOCK) {
      await delay(800);
      const newId = 'app-' + Date.now();
      const newApp: JobApplicationDto = {
        id: newId,
        companyName: data.companyName,
        jobTitle: data.jobTitle,
        jobDescription: data.jobDescription,
        notes: data.notes,
        currentStatus: 0,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        statusHistory: [{
          id: 'sh-' + Date.now(),
          applicationId: newId,
          status: 0,
          changedAt: new Date().toISOString(),
        }],
        event: undefined,
      };
      mockApplications.push(newApp);
      return newApp;
    }
    
    const response = await httpClient.post<JobApplicationDto>(
      API_ENDPOINTS.APPLICATIONS.BASE,
      data
    );
    return response.data;
  },

  async update(id: string, data: UpdateJobApplicationDto): Promise<JobApplicationDto> {
    if (USE_MOCK) {
      await delay(600);
      const index = mockApplications.findIndex(app => app.id === id);
      if (index === -1) {
        throw new Error('Application not found');
      }
      const updated = {
        ...mockApplications[index],
        ...data,
        updatedAt: new Date().toISOString(),
      };
      mockApplications[index] = updated;
      return updated;
    }
    
    const response = await httpClient.put<JobApplicationDto>(
      API_ENDPOINTS.APPLICATIONS.BY_ID(id),
      data
    );
    return response.data;
  },

  async updateStatus(id: string, data: UpdateStatusDto): Promise<JobApplicationDto> {
    if (USE_MOCK) {
      await delay(500);
      const index = mockApplications.findIndex(app => app.id === id);
      if (index === -1) {
        throw new Error('Application not found');
      }
      const newHistory = {
        id: 'sh-' + Date.now(),
        applicationId: id,
        status: data.newStatus,
        changedAt: new Date().toISOString(),
        notes: data.notes,
      };
      const updated = {
        ...mockApplications[index],
        currentStatus: data.newStatus,
        updatedAt: new Date().toISOString(),
        statusHistory: [...mockApplications[index].statusHistory, newHistory],
      };
      mockApplications[index] = updated;
      return updated;
    }
    
    const response = await httpClient.patch<JobApplicationDto>(
      API_ENDPOINTS.APPLICATIONS.UPDATE_STATUS(id),
      data
    );
    return response.data;
  },

  async delete(id: string): Promise<void> {
    if (USE_MOCK) {
      await delay(400);
      const index = mockApplications.findIndex(app => app.id === id);
      if (index !== -1) {
        mockApplications.splice(index, 1);
      }
      return;
    }
    
    await httpClient.delete(API_ENDPOINTS.APPLICATIONS.BY_ID(id));
  },
};
