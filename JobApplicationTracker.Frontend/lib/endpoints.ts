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
    MINIMAL: '/applications/minimal',
    MINIMAL_ARCHIVED: '/applications/minimal?archived',
    BY_ID: (id: string) => `/applications/${id}`,
    PUSH_STATUS: '/applications/entry',
  },
  
  // Events
  EVENTS: {
    BASE: '/events',
    BY_ID: (id: string) => `where-am-i-used`,
    BY_APPLICATION: (appId: string) => `where-am-i-used`,
    UPCOMING: `where-am-i-used`,
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
    REFRESH: '/match/refresh'
  },
  
  // Dashboard
  DASHBOARD: {
    STATS: '/dashboard/stats',
    RECENT: '/dashboard/recent',
  },
} as const;
