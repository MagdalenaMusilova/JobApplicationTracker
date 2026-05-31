import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  LoginDto,
  RegisterDto,
  AuthResponseDto,
  RefreshTokenDto,
} from '@/types';

// Mock delay for simulating API calls
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// Flag to use mock data
const USE_MOCK = true;

export const authService = {
  async login(data: LoginDto): Promise<AuthResponseDto> {
    if (USE_MOCK) {
      await delay(800);
      // Simulate successful login with any credentials
      if (data.email && data.password) {
        const mockResponse: AuthResponseDto = {
          token: 'mock-jwt-token-' + Date.now(),
          refreshToken: 'mock-refresh-token-' + Date.now(),
          expiresAt: new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString(),
        };
        return mockResponse;
      }
      throw new Error('Invalid credentials');
    }
    
    const response = await httpClient.post<AuthResponseDto>(
      API_ENDPOINTS.AUTH.LOGIN,
      data
    );
    return response.data;
  },

  async register(data: RegisterDto): Promise<AuthResponseDto> {
    if (USE_MOCK) {
      await delay(1000);
      const mockResponse: AuthResponseDto = {
        token: 'mock-jwt-token-' + Date.now(),
        refreshToken: 'mock-refresh-token-' + Date.now(),
        expiresAt: new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString(),
      };
      return mockResponse;
    }
    
    const response = await httpClient.post<AuthResponseDto>(
      API_ENDPOINTS.AUTH.REGISTER,
      data
    );
    return response.data;
  },

  async refreshToken(data: RefreshTokenDto): Promise<AuthResponseDto> {
    if (USE_MOCK) {
      await delay(300);
      const mockResponse: AuthResponseDto = {
        token: 'mock-jwt-token-refreshed-' + Date.now(),
        refreshToken: 'mock-refresh-token-refreshed-' + Date.now(),
        expiresAt: new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString(),
      };
      return mockResponse;
    }
    
    const response = await httpClient.post<AuthResponseDto>(
      API_ENDPOINTS.AUTH.REFRESH,
      data
    );
    return response.data;
  },

  async logout(): Promise<void> {
    if (USE_MOCK) {
      await delay(200);
      return;
    }
    
    await httpClient.post(API_ENDPOINTS.AUTH.LOGOUT);
  },
};
