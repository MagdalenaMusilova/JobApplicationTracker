'use client';

import { DashboardStatsDto } from '@/types/Dashboard/DashboardStatsDto';
import { Card, CardContent } from '@/components/ui/card';
import { Briefcase, Calendar, Trophy, TrendingUp } from 'lucide-react';

interface StatsCardsProps {
  stats: DashboardStatsDto;
}

export function StatsCards({ stats }: StatsCardsProps) {
  const cards = [
    {
      title: 'Total Applications',
      value: stats.totalApplications,
      description: `${stats.applicationsThisMonth} this month`,
      icon: Briefcase,
      color: 'text-blue-400',
      bgColor: 'bg-blue-500/10',
    },
    {
      title: 'Active',
      value: stats.activeApplications,
      description: 'In progress',
      icon: TrendingUp,
      color: 'text-emerald-400',
      bgColor: 'bg-emerald-500/10',
    },
    {
      title: 'Interviews',
      value: stats.interviewsScheduled,
      description: 'Scheduled',
      icon: Calendar,
      color: 'text-amber-400',
      bgColor: 'bg-amber-500/10',
    },
    {
      title: 'Offers',
      value: stats.offersReceived,
      description: 'Received',
      icon: Trophy,
      color: 'text-purple-400',
      bgColor: 'bg-purple-500/10',
    },
  ];

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
      {cards.map((card) => (
        <Card key={card.title} className="bg-card border-border">
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-sm font-medium text-muted-foreground">
                  {card.title}
                </p>
                <p className="text-3xl font-bold text-foreground mt-1">
                  {card.value}
                </p>
                <p className="text-xs text-muted-foreground mt-1">
                  {card.description}
                </p>
              </div>
              <div className={`p-3 rounded-lg ${card.bgColor}`}>
                <card.icon className={`h-6 w-6 ${card.color}`} />
              </div>
            </div>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
