import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  ApplicationEventDto,
  CreateApplicationEventDto,
  UpdateApplicationEventDto,
} from '@/types';

export const eventService = {
  async getAll(): Promise<ApplicationEventDto[]> {
    const response = await httpClient.get<ApplicationEventDto[]>(
        API_ENDPOINTS.EVENTS.BASE
    );
    return response.data;
  },

  async getUpcoming(): Promise<ApplicationEventDto[]> {
    const response = await httpClient.get<ApplicationEventDto[]>(
        API_ENDPOINTS.EVENTS.UPCOMING
    );
    return response.data;
  },

  async getByApplication(applicationId: string): Promise<ApplicationEventDto[]> {
    const response = await httpClient.get<ApplicationEventDto[]>(
        API_ENDPOINTS.EVENTS.BY_APPLICATION(applicationId)
    );
    return response.data;
  },

  async create(data: CreateApplicationEventDto): Promise<ApplicationEventDto> {
    const response = await httpClient.post<ApplicationEventDto>(
        API_ENDPOINTS.EVENTS.BASE,
        data
    );
    return response.data;
  },

  async update(id: string, data: UpdateApplicationEventDto): Promise<ApplicationEventDto> {
    const response = await httpClient.put<ApplicationEventDto>(
        API_ENDPOINTS.EVENTS.BY_ID(id),
        data
    );
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await httpClient.delete(API_ENDPOINTS.EVENTS.BY_ID(id));
  },

  async markCompleted(id: string, completed: boolean): Promise<ApplicationEventDto> {
    return this.update(id, { isCompleted: completed });
  },
};