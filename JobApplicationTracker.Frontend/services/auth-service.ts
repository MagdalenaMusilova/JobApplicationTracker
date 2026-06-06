import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import {
  LoginDto,
  RegisterDto,
  AuthResponseDto,
  RefreshTokenDto,
} from '@/types';

export const authService = {
  async login(data: LoginDto): Promise<AuthResponseDto> {
    const response = await httpClient.post<AuthResponseDto>(
        API_ENDPOINTS.AUTH.LOGIN,
        data
    );
    return response.data;
  },

  async register(data: RegisterDto): Promise<AuthResponseDto> {
    const response = await httpClient.post<AuthResponseDto>(
        API_ENDPOINTS.AUTH.REGISTER,
        data
    );
    return response.data;
  },

  async refreshToken(data: RefreshTokenDto): Promise<AuthResponseDto> {
    const response = await httpClient.post<AuthResponseDto>(
        API_ENDPOINTS.AUTH.REFRESH,
        data
    );
    return response.data;
  },

  async logout(): Promise<void> {
    await httpClient.post(API_ENDPOINTS.AUTH.LOGOUT);
  },
};