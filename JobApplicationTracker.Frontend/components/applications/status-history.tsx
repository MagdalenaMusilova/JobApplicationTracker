'use client';

import { format } from 'date-fns';
import {
  ApplicationStatusHistoryDto,
  applicationStatusLabels,
  applicationStatusColors,
} from '@/types';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { cn } from '@/lib/utils';

interface StatusHistoryProps {
  history: ApplicationStatusHistoryDto[];
}

export function StatusHistory({ history }: StatusHistoryProps) {
  // Sort by date descending (most recent first)
  const sortedHistory = [...history].sort(
    (a, b) => new Date(b.changedAt).getTime() - new Date(a.changedAt).getTime()
  );

  return (
    <Card className="bg-card border-border">
      <CardHeader>
        <CardTitle className="text-lg font-semibold">Status History</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="relative">
          {/* Timeline line */}
          <div className="absolute left-[11px] top-2 bottom-2 w-px bg-border" />

          <div className="space-y-6">
            {sortedHistory.map((item, index) => (
              <div key={item.id} className="relative flex gap-4">
                {/* Timeline dot */}
                <div
                  className={cn(
                    'relative z-10 w-6 h-6 rounded-full border-2 flex items-center justify-center',
                    index === 0
                      ? 'bg-primary border-primary'
                      : 'bg-card border-border'
                  )}
                >
                  <div
                    className={cn(
                      'w-2 h-2 rounded-full',
                      index === 0 ? 'bg-primary-foreground' : 'bg-muted-foreground'
                    )}
                  />
                </div>

                {/* Content */}
                <div className="flex-1 pb-2">
                  <div className="flex items-center gap-2 flex-wrap">
                    <Badge
                      variant="outline"
                      className={cn('text-xs', applicationStatusColors[item.status])}
                    >
                      {applicationStatusLabels[item.status]}
                    </Badge>
                    <span className="text-xs text-muted-foreground">
                      {format(new Date(item.changedAt), 'MMM d, yyyy')}
                    </span>
                  </div>
                  {item.notes && (
                    <p className="text-sm text-muted-foreground mt-2">
                      {item.notes}
                    </p>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
