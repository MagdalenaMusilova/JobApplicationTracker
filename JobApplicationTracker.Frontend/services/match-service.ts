import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  JobMatchDto,
  MatchFilterDto,
} from '@/types';
import { mockJobMatches } from '@/lib/mock-data';

// Mock delay for simulating API calls
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Flag to use mock data
const USE_MOCK = true;

export const matchService = {
  async getMatches(): Promise<JobMatchDto[]> {
    if (USE_MOCK) {
      await delay(800);
      return [...mockJobMatches].sort((a, b) => b.matchScore - a.matchScore);
    }
    
    const response = await httpClient.get<JobMatchDto[]>(
      API_ENDPOINTS.MATCH.BASE
    );
    return response.data;
  },

  async refreshMatches(): Promise<JobMatchDto[]> {
    if (USE_MOCK) {
      await delay(1500); // Simulate longer search time
      return [...mockJobMatches].sort((a, b) => b.matchScore - a.matchScore);
    }
    
    const response = await httpClient.post<JobMatchDto[]>(
      API_ENDPOINTS.MATCH.REFRESH
    );
    return response.data;
  },

  async findMatches(filters?: MatchFilterDto): Promise<JobMatchDto[]> {
    if (USE_MOCK) {
      await delay(1500); // Simulate longer search time
      
      let matches = [...mockJobMatches];
      
      // Apply filters if provided
      if (filters?.keywords?.length) {
        const keywords = filters.keywords.map(k => k.toLowerCase());
        matches = matches.filter(m =>
          keywords.some(k =>
            m.jobTitle.toLowerCase().includes(k) ||
            m.companyName.toLowerCase().includes(k) ||
            m.matchReasons.some(r => r.toLowerCase().includes(k))
          )
        );
      }
      
      if (filters?.locations?.length) {
        matches = matches.filter(m =>
          filters.locations?.some(l =>
            m.location?.toLowerCase().includes(l.toLowerCase())
          )
        );
      }
      
      if (filters?.workModes?.length) {
        matches = matches.filter(m =>
          filters.workModes?.includes(m.workMode)
        );
      }
      
      if (filters?.minSalary) {
        matches = matches.filter(m =>
          (m.salaryMax || 0) >= filters.minSalary!
        );
      }
      
      // Sort by match score
      matches.sort((a, b) => b.matchScore - a.matchScore);
      
      return matches;
    }
    
    const response = await httpClient.post<JobMatchDto[]>(
      API_ENDPOINTS.MATCH.FIND,
      filters
    );
    return response.data;
  },
};
