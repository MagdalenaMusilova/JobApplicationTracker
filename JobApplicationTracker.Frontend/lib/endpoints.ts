// API Endpoints configuration
export const API_ENDPOINTS = {
  // Auth
  AUTH: {
    LOGIN: '/auth/login',
    REGISTER: '/auth/register',
    REFRESH: '/auth/refresh',
    LOGOUT: '/auth/logout',
  },
  
  // Applications
  APPLICATIONS: {
    BASE: '/applications',
    BY_ID: (id: string) => `/applications/${id}`,
    UPDATE_STATUS: (id: string) => `/applications/${id}/status`,
  },
  
  // Events
  EVENTS: {
    BASE: '/events',
    BY_ID: (id: string) => `/events/${id}`,
    BY_APPLICATION: (appId: string) => `/applications/${appId}/events`,
    UPCOMING: '/events/upcoming',
  },
  
  // Profile
  PROFILE: {
    BASE: '/profile',
    UPLOAD_CV: '/profile/cv',
    DOWNLOAD_CV: '/profile/cv/download',
  },
  
  // Match
  MATCH: {
    BASE: '/match',
    FIND: '/match/find',
  },
  
  // Dashboard
  DASHBOARD: {
    STATS: '/dashboard/stats',
    RECENT: '/dashboard/recent',
  },
} as const;
