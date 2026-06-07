export interface DashboardStatsDto {
  totalApplications: number;
  activeApplications: number;
  interviewsScheduled: number;
  offersReceived: number;
  applicationsByStatus: Record<number, number>;
  applicationsThisWeek: number;
  applicationsThisMonth: number;
}
