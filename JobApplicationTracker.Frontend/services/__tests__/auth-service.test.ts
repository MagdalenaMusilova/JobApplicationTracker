import { authService } from '../auth-service'
import { LoginDto } from '@/types/Auth/SignInDto'
import { RegisterDto } from '@/types/Auth/SignUpDto'
import httpClient from '@/lib/http-client'

// Mock the http client
jest.mock('@/lib/http-client')
const mockedHttpClient = httpClient as jest.Mocked<typeof httpClient>

describe('authService', () => {
  afterEach(() => {
    jest.clearAllMocks()
  })

  describe('login', () => {
    it('should successfully login a user', async () => {
      const loginData: LoginDto = {
        email: 'test@example.com',
        password: 'password123',
      }

      const mockResponse = {
        data: {
          token: 'mock-access-token',
          refreshToken: 'mock-refresh-token',
          expiresAt: '2024-12-31T00:00:00Z',
        },
      }

      mockedHttpClient.post.mockResolvedValueOnce(mockResponse)

      const response = await authService.login(loginData)

      expect(mockedHttpClient.post).toHaveBeenCalledWith(
        expect.stringContaining('/auth/login'),
        loginData
      )
      expect(response).toEqual(mockResponse.data)
      expect(response.token).toBe('mock-access-token')
    })
  })

  describe('register', () => {
    it('should successfully register a new user', async () => {
      const registerData: RegisterDto = {
        email: 'newuser@example.com',
        password: 'password123',
        firstName: 'New',
        lastName: 'User',
        dateOfBirth: '1990-01-01',
        phoneNumber: '+1234567890',
      }

      const mockResponse = {
        data: {
          token: 'mock-access-token',
          refreshToken: 'mock-refresh-token',
          expiresAt: '2024-12-31T00:00:00Z',
        },
      }

      mockedHttpClient.post.mockResolvedValueOnce(mockResponse)

      const response = await authService.register(registerData)

      expect(mockedHttpClient.post).toHaveBeenCalledWith(
        expect.stringContaining('/auth/register'),
        registerData
      )
      expect(response).toEqual(mockResponse.data)
    })
  })

  describe('refreshToken', () => {
    it('should successfully refresh the token', async () => {
      const refreshData = {
        refreshToken: 'old-refresh-token',
      }

      const mockResponse = {
        data: {
          token: 'new-access-token',
          refreshToken: 'new-refresh-token',
          expiresAt: '2024-12-31T00:00:00Z',
        },
      }

      mockedHttpClient.post.mockResolvedValueOnce(mockResponse)

      const response = await authService.refreshToken(refreshData)

      expect(mockedHttpClient.post).toHaveBeenCalledWith(
        expect.stringContaining('/auth/refresh'),
        refreshData
      )
      expect(response).toEqual(mockResponse.data)
    })
  })

  describe('logout', () => {
    it('should successfully logout', async () => {
      mockedHttpClient.post.mockResolvedValueOnce({ data: null })

      await authService.logout()

      expect(mockedHttpClient.post).toHaveBeenCalledWith(
        expect.stringContaining('/auth/logout')
      )
    })
  })

  describe('error handling', () => {
    it('should handle login errors', async () => {
      const loginData: LoginDto = {
        email: 'wrong@example.com',
        password: 'wrongpassword',
      }

      mockedHttpClient.post.mockRejectedValueOnce(new Error('Invalid credentials'))

      await expect(authService.login(loginData)).rejects.toThrow('Invalid credentials')
    })
  })
})
