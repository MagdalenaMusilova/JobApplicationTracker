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
      <div className="space-y-8">
        {/* Header */}
        <div>
          <h1 className="text-3xl font-bold tracking-tight text-foreground">Dashboard</h1>
          <p className="text-muted-foreground mt-1">
            Track your job search progress at a glance
          </p>
        </div>

        {/* Stats Cards */}
        {statsLoading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            {[...Array(4)].map((_, i) => (
              <Skeleton key={i} className="h-32 rounded-lg" />
            ))}
          </div>
        ) : stats ? (
          <StatsCards stats={stats} />
        ) : null}

        {/* Main Grid */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Upcoming Events */}
          <div className="lg:col-span-1">
            {eventsLoading ? (
              <Skeleton className="h-[400px] rounded-lg" />
            ) : (
              <UpcomingEvents events={upcomingEvents || []} />
            )}
          </div>

          {/* Recent Applications */}
          <div className="lg:col-span-2">
            {recentLoading ? (
              <Skeleton className="h-[400px] rounded-lg" />
            ) : (
              <RecentApplications applications={recentApps || []} />
            )}
          </div>
        </div>

        {/* Status Distribution Chart */}
        {statsLoading ? (
          <Skeleton className="h-[300px] rounded-lg" />
        ) : stats ? (
          <StatusChart stats={stats} />
        ) : null}
      </div>
    </ProtectedLayout>
  );
}
