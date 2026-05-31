'use client';

import Link from 'next/link';
import { format, isToday, isTomorrow, isPast } from 'date-fns';
import { ApplicationEventDto, eventTypeLabels, eventTypeColors } from '@/types';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Calendar, Clock, MapPin, ArrowRight } from 'lucide-react';
import { cn } from '@/lib/utils';

interface UpcomingEventsProps {
  events: ApplicationEventDto[];
}

function formatEventDate(dateString: string): string {
  const date = new Date(dateString);
  if (isToday(date)) return 'Today';
  if (isTomorrow(date)) return 'Tomorrow';
  return format(date, 'MMM d');
}

function formatEventTime(dateString: string): string {
  return format(new Date(dateString), 'h:mm a');
}

export function UpcomingEvents({ events }: UpcomingEventsProps) {
  const upcomingEvents = events
    .filter(e => !e.isCompleted && !isPast(new Date(e.scheduledAt)))
    .slice(0, 5);

  return (
    <Card className="bg-card border-border h-full">
      <CardHeader className="flex flex-row items-center justify-between pb-4">
        <CardTitle className="text-lg font-semibold">Upcoming Events</CardTitle>
        <Link href="/calendar">
          <Button variant="ghost" size="sm" className="text-muted-foreground hover:text-foreground">
            View all
            <ArrowRight className="ml-1 h-4 w-4" />
          </Button>
        </Link>
      </CardHeader>
      <CardContent className="space-y-4">
        {upcomingEvents.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-8 text-center">
            <Calendar className="h-10 w-10 text-muted-foreground/50 mb-3" />
            <p className="text-sm text-muted-foreground">No upcoming events</p>
            <p className="text-xs text-muted-foreground/70 mt-1">
              Events will appear here when scheduled
            </p>
          </div>
        ) : (
          upcomingEvents.map((event) => (
            <Link
              key={event.id}
              href={`/applications/${event.applicationId}`}
              className="block"
            >
              <div className="flex items-start gap-4 p-3 rounded-lg hover:bg-accent/50 transition-colors">
                <div className="flex flex-col items-center min-w-[50px]">
                  <span className="text-xs font-medium text-muted-foreground">
                    {formatEventDate(event.scheduledAt)}
                  </span>
                  <span className="text-lg font-bold text-foreground">
                    {format(new Date(event.scheduledAt), 'd')}
                  </span>
                </div>
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1">
                    <Badge
                      variant="outline"
                      className={cn('text-xs', eventTypeColors[event.eventType])}
                    >
                      {eventTypeLabels[event.eventType]}
                    </Badge>
                  </div>
                  <p className="font-medium text-foreground truncate">
                    {event.companyName}
                  </p>
                  <p className="text-sm text-muted-foreground truncate">
                    {event.jobTitle}
                  </p>
                  <div className="flex items-center gap-3 mt-2 text-xs text-muted-foreground">
                    <span className="flex items-center gap-1">
                      <Clock className="h-3 w-3" />
                      {formatEventTime(event.scheduledAt)}
                    </span>
                    {event.location && (
                      <span className="flex items-center gap-1">
                        <MapPin className="h-3 w-3" />
                        {event.location}
                      </span>
                    )}
                  </div>
                </div>
              </div>
            </Link>
          ))
        )}
      </CardContent>
    </Card>
  );
}
