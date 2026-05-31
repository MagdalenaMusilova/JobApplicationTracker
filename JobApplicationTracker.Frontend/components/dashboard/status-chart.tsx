'use client';

import {
  DashboardStatsDto,
  ApplicationStatus,
  applicationStatusLabels,
} from '@/types';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  Cell,
} from 'recharts';

interface StatusChartProps {
  stats: DashboardStatsDto;
}

const statusColors: Record<ApplicationStatus, string> = {
  [ApplicationStatus.Applied]: '#3b82f6',
  [ApplicationStatus.PhoneScreen]: '#06b6d4',
  [ApplicationStatus.Interview]: '#6366f1',
  [ApplicationStatus.TechnicalAssessment]: '#a855f7',
  [ApplicationStatus.FinalRound]: '#f59e0b',
  [ApplicationStatus.OfferReceived]: '#10b981',
  [ApplicationStatus.Accepted]: '#22c55e',
  [ApplicationStatus.Rejected]: '#ef4444',
  [ApplicationStatus.Withdrawn]: '#6b7280',
};

export function StatusChart({ stats }: StatusChartProps) {
  const data = Object.entries(stats.applicationsByStatus)
    .map(([status, count]) => ({
      status: applicationStatusLabels[Number(status) as ApplicationStatus],
      count,
      statusKey: Number(status) as ApplicationStatus,
    }))
    .filter(item => item.count > 0);

  if (data.length === 0) {
    return null;
  }

  return (
    <Card className="bg-card border-border">
      <CardHeader>
        <CardTitle className="text-lg font-semibold">Applications by Status</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="h-[250px]">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={data} layout="vertical" margin={{ left: 20, right: 20 }}>
              <XAxis type="number" hide />
              <YAxis
                type="category"
                dataKey="status"
                width={140}
                tick={{ fill: '#a1a1aa', fontSize: 12 }}
                axisLine={false}
                tickLine={false}
              />
              <Tooltip
                contentStyle={{
                  backgroundColor: '#1c1c22',
                  border: '1px solid #27272a',
                  borderRadius: '8px',
                  color: '#fafafa',
                }}
                cursor={{ fill: 'rgba(255,255,255,0.05)' }}
              />
              <Bar
                dataKey="count"
                radius={[0, 4, 4, 0]}
                maxBarSize={24}
              >
                {data.map((entry, index) => (
                  <Cell
                    key={`cell-${index}`}
                    fill={statusColors[entry.statusKey]}
                  />
                ))}
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
}
