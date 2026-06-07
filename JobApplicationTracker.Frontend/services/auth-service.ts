import httpClient from '@/lib/http-client';
import { API_ENDPOINTS } from '@/lib/endpoints';
import { LoginDto } from '@/types/Auth/SignInDto';
import { RegisterDto } from '@/types/Auth/SignUpDto';
import { AuthResponseDto } from '@/types/Auth/SignInResponseDto';
import { RefreshTokenDto } from '@/types/Auth/RefreshTokenRequestDto';

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