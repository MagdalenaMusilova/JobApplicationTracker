'use client';

import { useQuery } from '@tanstack/react-query';
import { dashboardService } from '@/services/dashboard-service';
import { eventService } from '@/services/event-service';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { StatsCards } from '@/components/dashboard/stats-cards';
import { UpcomingEvents } from '@/components/dashboard/upcoming-events';
import { RecentApplications } from '@/components/dashboard/recent-applications';
import { StatusChart } from '@/components/dashboard/status-chart';
import { Skeleton } from '@/components/ui/skeleton';

export default function DashboardPage() {
  const { data: stats, isLoading: statsLoading } = useQuery({
    queryKey: ['dashboard', 'stats'],
    queryFn: () => dashboardService.getStats(),
  });

  const { data: recentApps, isLoading: recentLoading } = useQuery({
    queryKey: ['dashboard', 'recent'],
    queryFn: () => dashboardService.getRecent(),
  });

  const { data: upcomingEvents, isLoading: eventsLoading } = useQuery({
    queryKey: ['events', 'upcoming'],
    queryFn: () => eventService.getUpcoming(),
  });

  return (
    <ProtectedLayout>
      <div className="space-y-6">
        {/* Header */}
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
          <p className="text-muted-foreground mt-1">
            Track your job search progress
          </p>
        </div>

        {/* Quick Stats Bar */}
        {statsLoading ? (
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            {[...Array(4)].map((_, i) => (
              <Skeleton key={i} className="h-24 rounded-lg" />
            ))}
          </div>
        ) : stats ? (
          <StatsCards stats={stats} />
        ) : null}

        {/* Main Content Grid */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Status Distribution */}
          <div>
            {statsLoading ? (
              <Skeleton className="h-[400px] rounded-lg" />
            ) : stats ? (
              <StatusChart stats={stats} />
            ) : null}
          </div>

          {/* Upcoming Events */}
          <div>
            {eventsLoading ? (
              <Skeleton className="h-[400px] rounded-lg" />
            ) : (
              <UpcomingEvents events={upcomingEvents || []} />
            )}
          </div>
        </div>

        {/* Recent Applications */}
        <div>
          {recentLoading ? (
            <Skeleton className="h-[300px] rounded-lg" />
          ) : (
            <RecentApplications applications={recentApps || []} />
          )}
        </div>
      </div>
    </ProtectedLayout>
  );
}
