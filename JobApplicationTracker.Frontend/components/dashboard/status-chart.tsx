'use client';

import { DashboardStatsDto } from '@/types/Dashboard/DashboardStatsDto';
import { JAStatusType, jaStatusLabels } from '@/types/Enums/JAStatusType';
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

const statusColors: Record<JAStatusType, string> = {
  [JAStatusType.Wishlist]: '#6b7280',
  [JAStatusType.Applied]: '#3b82f6',
  [JAStatusType.Task]: '#06b6d4',
  [JAStatusType.Interview]: '#6366f1',
  [JAStatusType.Offer]: '#f59e0b',
  [JAStatusType.Accepted]: '#22c55e',
  [JAStatusType.Rejected]: '#ef4444',
};

export function StatusChart({ stats }: StatusChartProps) {
  const data = Object.entries(stats.applicationsByStatus)
    .map(([status, count]) => ({
      status: jaStatusLabels[Number(status) as JAStatusType],
      count,
      statusKey: Number(status) as JAStatusType,
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
