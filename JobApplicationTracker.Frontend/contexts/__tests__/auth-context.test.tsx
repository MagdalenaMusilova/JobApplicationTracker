import React from 'react'
import { renderHook, act, waitFor } from '@testing-library/react'
import { AuthProvider, useAuth } from '../auth-context'
import { authService } from '@/services/auth-service'

// Mock auth service
jest.mock('@/services/auth-service')
const mockedAuthService = authService as jest.Mocked<typeof authService>

// Mock localStorage
const localStorageMock = (() => {
  let store: Record<string, string> = {}

  return {
    getItem: (key: string) => store[key] || null,
    setItem: (key: string, value: string) => {
      store[key] = value.toString()
    },
    removeItem: (key: string) => {
      delete store[key]
    },
    clear: () => {
      store = {}
    },
  }
})()

Object.defineProperty(window, 'localStorage', {
  value: localStorageMock,
})

describe('AuthContext', () => {
  beforeEach(() => {
    localStorageMock.clear()
    jest.clearAllMocks()
  })

  const wrapper = ({ children }: { children: React.ReactNode }) => (
    <AuthProvider>{children}</AuthProvider>
  )

  it('should throw error when useAuth is used outside AuthProvider', () => {
    // Suppress console.error for this test
    const consoleSpy = jest.spyOn(console, 'error').mockImplementation()

    expect(() => renderHook(() => useAuth())).toThrow(
      'useAuth must be used within an AuthProvider'
    )

    consoleSpy.mockRestore()
  })

  it('should initialize with no user when no token exists', async () => {
    const { result } = renderHook(() => useAuth(), { wrapper })

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false)
    })

    expect(result.current.user).toBeNull()
    expect(result.current.isAuthenticated).toBe(false)
  })

  it('should restore user from localStorage on mount', async () => {
    localStorageMock.setItem('accessToken', 'stored-token')
    localStorageMock.setItem('userEmail', 'stored@example.com')

    const { result } = renderHook(() => useAuth(), { wrapper })

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false)
    })

    expect(result.current.user).toEqual({ email: 'stored@example.com' })
    expect(result.current.isAuthenticated).toBe(true)
  })

  it('should login successfully', async () => {
    const mockResponse = {
      token: 'mock-access-token',
      refreshToken: 'mock-refresh-token',
      expiresAt: '2024-12-31T00:00:00Z',
    }

    mockedAuthService.login.mockResolvedValueOnce(mockResponse)

    const { result } = renderHook(() => useAuth(), { wrapper })

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false)
    })

    await act(async () => {
      await result.current.login({
        email: 'test@example.com',
        password: 'password123',
      })
    })

    expect(result.current.user).toEqual({ email: 'test@example.com' })
    expect(result.current.isAuthenticated).toBe(true)
    expect(localStorageMock.getItem('accessToken')).toBe('mock-access-token')
    expect(localStorageMock.getItem('userEmail')).toBe('test@example.com')
  })

  it('should register successfully', async () => {
    const mockResponse = {
      token: 'mock-access-token',
      refreshToken: 'mock-refresh-token',
      expiresAt: '2024-12-31T00:00:00Z',
    }

    mockedAuthService.register.mockResolvedValueOnce(mockResponse)

    const { result } = renderHook(() => useAuth(), { wrapper })

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false)
    })

    await act(async () => {
      await result.current.register({
        email: 'newuser@example.com',
        password: 'password123',
        firstName: 'New',
        lastName: 'User',
        dateOfBirth: '1990-01-01',
        phoneNumber: '+1234567890',
      })
    })

    expect(result.current.user).toEqual({ email: 'newuser@example.com' })
    expect(result.current.isAuthenticated).toBe(true)
  })

  it('should logout successfully', async () => {
    localStorageMock.setItem('accessToken', 'stored-token')
    localStorageMock.setItem('userEmail', 'stored@example.com')

    mockedAuthService.logout.mockResolvedValueOnce()

    const { result } = renderHook(() => useAuth(), { wrapper })

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false)
    })

    await act(async () => {
      await result.current.logout()
    })

    expect(result.current.user).toBeNull()
    expect(result.current.isAuthenticated).toBe(false)
    expect(localStorageMock.getItem('accessToken')).toBeNull()
    expect(localStorageMock.getItem('userEmail')).toBeNull()
  })

  it('should handle login error', async () => {
    mockedAuthService.login.mockRejectedValueOnce(new Error('Invalid credentials'))

    const { result } = renderHook(() => useAuth(), { wrapper })

    await waitFor(() => {
      expect(result.current.isLoading).toBe(false)
    })

    await expect(
      act(async () => {
        await result.current.login({
          email: 'wrong@example.com',
          password: 'wrongpassword',
        })
      })
    ).rejects.toThrow('Invalid credentials')

    expect(result.current.user).toBeNull()
    expect(result.current.isAuthenticated).toBe(false)
  })
})
