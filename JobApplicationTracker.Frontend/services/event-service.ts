import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  ApplicationEventDto,
  CreateApplicationEventDto,
  UpdateApplicationEventDto,
} from '@/types';
import {
  mockApplications,
  mockEvents,
  mockUpcomingEvents,
} from '@/lib/mock-data';

// Mock delay for simulating API calls
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Flag to use mock data
const USE_MOCK = true;

export const eventService = {
  async getAll(): Promise<ApplicationEventDto[]> {
    if (USE_MOCK) {
      await delay(400);
      return mockEvents;
    }
    
    const response = await httpClient.get<ApplicationEventDto[]>(
      API_ENDPOINTS.EVENTS.BASE
    );
    return response.data;
  },

  async getUpcoming(): Promise<ApplicationEventDto[]> {
    if (USE_MOCK) {
      await delay(300);
      return mockUpcomingEvents;
    }
    
    const response = await httpClient.get<ApplicationEventDto[]>(
      API_ENDPOINTS.EVENTS.UPCOMING
    );
    return response.data;
  },

  async getByApplication(applicationId: string): Promise<ApplicationEventDto[]> {
    if (USE_MOCK) {
      await delay(300);
      const app = mockApplications.find(a => a.id === applicationId);
      return app?.events || [];
    }
    
    const response = await httpClient.get<ApplicationEventDto[]>(
      API_ENDPOINTS.EVENTS.BY_APPLICATION(applicationId)
    );
    return response.data;
  },

  async create(data: CreateApplicationEventDto): Promise<ApplicationEventDto> {
    if (USE_MOCK) {
      await delay(600);
      const app = mockApplications.find(a => a.id === data.applicationId);
      const newEvent: ApplicationEventDto = {
        id: 'ev-' + Date.now(),
        ...data,
        isCompleted: false,
        createdAt: new Date().toISOString(),
        companyName: app?.companyName,
        jobTitle: app?.jobTitle,
      };
      if (app) {
        app.events.push(newEvent);
      }
      return newEvent;
    }
    
    const response = await httpClient.post<ApplicationEventDto>(
      API_ENDPOINTS.EVENTS.BASE,
      data
    );
    return response.data;
  },

  async update(id: string, data: UpdateApplicationEventDto): Promise<ApplicationEventDto> {
    if (USE_MOCK) {
      await delay(500);
      for (const app of mockApplications) {
        const eventIndex = app.events.findIndex(e => e.id === id);
        if (eventIndex !== -1) {
          const updated = {
            ...app.events[eventIndex],
            ...data,
          };
          app.events[eventIndex] = updated;
          return updated;
        }
      }
      throw new Error('Event not found');
    }
    
    const response = await httpClient.put<ApplicationEventDto>(
      API_ENDPOINTS.EVENTS.BY_ID(id),
      data
    );
    return response.data;
  },

  async delete(id: string): Promise<void> {
    if (USE_MOCK) {
      await delay(400);
      for (const app of mockApplications) {
        const eventIndex = app.events.findIndex(e => e.id === id);
        if (eventIndex !== -1) {
          app.events.splice(eventIndex, 1);
          return;
        }
      }
      return;
    }
    
    await httpClient.delete(API_ENDPOINTS.EVENTS.BY_ID(id));
  },

  async markCompleted(id: string, completed: boolean): Promise<ApplicationEventDto> {
    return this.update(id, { isCompleted: completed });
  },
};
