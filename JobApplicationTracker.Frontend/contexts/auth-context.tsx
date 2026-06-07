'use client';

import React, { createContext, useContext, useState, useEffect, useCallback, ReactNode } from 'react';
import { authService } from '@/services/auth-service';
import { LoginDto } from '@/types/Auth/SignInDto';
import { RegisterDto } from '@/types/Auth/SignUpDto';
import { AuthResponseDto } from '@/types/Auth/SignInResponseDto';

interface User {
  email: string;
  id?: string;
}

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (data: LoginDto) => Promise<void>;
  register: (data: RegisterDto) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // Check for existing auth on mount
  useEffect(() => {
    const checkAuth = () => {
      const token = localStorage.getItem('accessToken');
      const storedEmail = localStorage.getItem('userEmail');
      
      if (token && storedEmail) {
        setUser({ email: storedEmail });
      }
      setIsLoading(false);
    };

    checkAuth();
  }, []);

  const handleAuthResponse = (response: AuthResponseDto, email: string) => {
    localStorage.setItem('accessToken', response.token);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('tokenExpiry', response.expiresAt);
    localStorage.setItem('userEmail', email);
    setUser({ email });
  };

  const login = useCallback(async (data: LoginDto) => {
    setIsLoading(true);
    try {
      const response = await authService.login(data);
      handleAuthResponse(response, data.email);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const register = useCallback(async (data: RegisterDto) => {
    setIsLoading(true);
    try {
      const response = await authService.register(data);
      handleAuthResponse(response, data.email);
    } finally {
      setIsLoading(false);
    }
  }, []);

  const logout = useCallback(async () => {
    setIsLoading(true);
    try {
      await authService.logout();
    } finally {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('tokenExpiry');
      localStorage.removeItem('userEmail');
      setUser(null);
      setIsLoading(false);
    }
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        isAuthenticated: !!user,
        isLoading,
        login,
        register,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
