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
    ALL: '/applications/all',
    NOT_FINISHED: '/applications/notFinished',
    MINIMAL: '/applications/minimal',
    MINIMAL_ARCHIVED: '/applications/minimal?archived',
    BY_ID: (id: string) => `/applications/${id}`,
    DENY: (id: string) => `/applications/${id}/deny`,
    PUSH_STATUS: '/applications/entry',
    STATUS_ENTRY: (id: string) => `/applications/entry/${id}`,
  },
  
  // Events
  EVENTS: {
    BASE: '/events',
    BY_ID: (id: string) => `/events/${id}`,
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
    REFRESH: '/match/refresh',
    ANALYZE: '/jobListings/match',
  },
  
  // Dashboard
  DASHBOARD: {
    STATS: '/dashboard/stats',
    RECENT: '/dashboard/recent',
  },
} as const;
