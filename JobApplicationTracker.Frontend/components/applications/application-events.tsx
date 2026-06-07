'use client';

import { useState } from 'react';
import { format } from 'date-fns';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { eventService } from '@/services/event-service';
import {
  JAEventDto,
  eventTypeLabels,
  eventTypeColors,
} from '@/types/JAObjects/JAEvents/JAEventDto';
import { JAEventType } from '@/types/Enums/JAEventType';
import { eventTypeLabels as eventTypeLabelsEnum, eventTypeColors as eventTypeColorsEnum } from '@/types/Enums/JAEventType';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Checkbox } from '@/components/ui/checkbox';
import { CreateEventModal } from '@/components/applications/create-event-modal';
import { Plus, Clock, MapPin, Calendar } from 'lucide-react';
import { cn } from '@/lib/utils';
import { toast } from 'sonner';

interface ApplicationEventsProps {
  applicationId: string;
  events: JAEventDto[];
}

export function ApplicationEvents({ applicationId, events }: ApplicationEventsProps) {
  const [showCreateModal, setShowCreateModal] = useState(false);
  const queryClient = useQueryClient();

  const toggleMutation = useMutation({
    mutationFn: ({ id, completed }: { id: string; completed: boolean }) =>
      eventService.markCompleted(id, completed),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
    },
    onError: () => {
      toast.error('Failed to update event');
    },
  });

  const sortedEvents = [...events].sort(
    (a, b) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime()
  );

  const upcomingEvents = sortedEvents.filter(e => new Date(e.eventDate) >= new Date());
  const completedEvents = sortedEvents.filter(e => new Date(e.eventDate) < new Date());

  return (
    <Card className="bg-card border-border">
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle className="text-lg font-semibold">Events</CardTitle>
        <Button size="sm" onClick={() => setShowCreateModal(true)}>
          <Plus className="h-4 w-4 mr-1" />
          Add Event
        </Button>
      </CardHeader>
      <CardContent>
        {events.length === 0 ? (
          <div className="text-center py-8">
            <Calendar className="h-10 w-10 text-muted-foreground/50 mx-auto mb-3" />
            <p className="text-sm text-muted-foreground">No events scheduled</p>
            <p className="text-xs text-muted-foreground/70 mt-1">
              Add interviews, calls, or follow-ups
            </p>
          </div>
        ) : (
          <div className="space-y-6">
            {/* Upcoming Events */}
            {upcomingEvents.length > 0 && (
              <div>
                <h4 className="text-sm font-medium text-muted-foreground mb-3">
                  Upcoming
                </h4>
                <div className="space-y-3">
                  {upcomingEvents.map((event) => (
                    <EventItem
                      key={event.id}
                      event={event}
                      onToggleComplete={(completed) =>
                        toggleMutation.mutate({ id: event.id, completed })
                      }
                    />
                  ))}
                </div>
              </div>
            )}

            {/* Completed Events */}
            {completedEvents.length > 0 && (
              <div>
                <h4 className="text-sm font-medium text-muted-foreground mb-3">
                  Completed
                </h4>
                <div className="space-y-3">
                  {completedEvents.map((event) => (
                    <EventItem
                      key={event.id}
                      event={event}
                      onToggleComplete={(completed) =>
                        toggleMutation.mutate({ id: event.id, completed })
                      }
                    />
                  ))}
                </div>
              </div>
            )}
          </div>
        )}

        {/* Create Event Modal */}
        <CreateEventModal
          open={showCreateModal}
          onOpenChange={setShowCreateModal}
          applicationId={applicationId}
        />
      </CardContent>
    </Card>
  );
}

interface EventItemProps {
  event: JAEventDto;
  onToggleComplete: (completed: boolean) => void;
}

function EventItem({ event, onToggleComplete }: EventItemProps) {
  const isCompleted = new Date(event.eventDate) < new Date();
  return (
    <div
      className={cn(
        'flex items-start gap-3 p-3 rounded-lg border border-border',
        isCompleted && 'opacity-60'
      )}
    >
      <div className="flex-1 min-w-0">
        <div className="flex items-center gap-2 flex-wrap">
          <Badge
            variant="outline"
            className={cn('text-xs', eventTypeColorsEnum[event.eventType])}
          >
            {eventTypeLabelsEnum[event.eventType]}
          </Badge>
        </div>
        {event.eventName && (
          <p
            className={cn(
              'text-sm mt-1',
              isCompleted ? 'line-through text-muted-foreground' : 'text-foreground'
            )}
          >
            {event.eventName}
          </p>
        )}
        <div className="flex items-center gap-3 mt-2 text-xs text-muted-foreground">
          <span className="flex items-center gap-1">
            <Clock className="h-3 w-3" />
            {format(new Date(event.eventDate), 'MMM d, yyyy h:mm a')}
          </span>
        </div>
        {event.note && (
          <p className="text-xs text-muted-foreground mt-2 italic">
            {event.note}
          </p>
        )}
      </div>
    </div>
  );
}
