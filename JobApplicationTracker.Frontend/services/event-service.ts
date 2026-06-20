import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import { JAEventDto } from '@/types/JAObjects/JAEvents/JAEventDto';
import { CreateJAEventDto } from '@/types/JAObjects/JAEvents/CreateJAEventDto';
import { UpdateJAEventDto } from '@/types/JAObjects/JAEvents/UpdateJAEventDto';

export const eventService = {
  async getAll(): Promise<JAEventDto[]> {
    const response = await httpClient.get<JAEventDto[]>(
        API_ENDPOINTS.EVENTS.BASE
    );
    return response.data ?? [];
  },

  async create(data: CreateJAEventDto): Promise<JAEventDto> {
    const response = await httpClient.post<JAEventDto>(
        API_ENDPOINTS.EVENTS.BASE,
        data
    );
    return response.data;
  },

  async update(id: string, data: UpdateJAEventDto): Promise<JAEventDto> {
    const response = await httpClient.put<JAEventDto>(
        API_ENDPOINTS.EVENTS.BY_ID(id),
        data
    );
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await httpClient.delete(API_ENDPOINTS.EVENTS.BY_ID(id));
  },

  async getUpcoming(): Promise<any[]> {
    const response = await httpClient.get<any[]>(
      API_ENDPOINTS.EVENTS.BASE
    );
    return response.data ?? [];
  },
};