import { LoginResponse } from '@/types/Auth/LoginResponse'
import { User } from '@/types/User/User'

export const mockUser: User = {
  id: 'test-user-id',
  email: 'test@example.com',
  firstName: 'Test',
  lastName: 'User',
  dateOfBirth: '1990-01-01',
  profilePicturePath: null,
  phoneNumber: '+1234567890',
}

export const mockLoginResponse: LoginResponse = {
  accessToken: 'mock-access-token',
  refreshToken: 'mock-refresh-token',
  user: mockUser,
}

export const mockAuthContext = {
  user: mockUser,
  login: jest.fn(),
  logout: jest.fn(),
  register: jest.fn(),
  isLoading: false,
  isAuthenticated: true,
}
