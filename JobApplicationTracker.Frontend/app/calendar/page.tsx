'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Skeleton } from '@/components/ui/skeleton';
import { 
  ChevronLeft, 
  ChevronRight, 
  Calendar as CalendarIcon,
  Video,
  Phone,
  Users,
  FileText,
  Clock,
  MapPin
} from 'lucide-react';
import { eventService } from '@/services/event-service';
import { JAEventType, eventTypeLabels, eventTypeColors } from '@/types/Enums/JAEventType';
import type { JAEventDto } from '@/types/JAObjects/JAEvents/JAEventDto';
import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameMonth, isSameDay, addMonths, subMonths, isToday } from 'date-fns';
import Link from 'next/link';

const eventTypeIcons: Record<JAEventType, React.ElementType> = {
  [JAEventType.Interview]: Users,
  [JAEventType.PhoneScreen]: Phone,
  [JAEventType.TechnicalAssessment]: FileText,
  [JAEventType.FollowUp]: Clock,
  [JAEventType.Other]: CalendarIcon,
};

const eventTypeBgColors: Record<JAEventType, string> = {
  [JAEventType.Interview]: 'bg-indigo-500',
  [JAEventType.PhoneScreen]: 'bg-cyan-500',
  [JAEventType.TechnicalAssessment]: 'bg-purple-500',
  [JAEventType.FollowUp]: 'bg-amber-500',
  [JAEventType.Other]: 'bg-gray-500',
};

// Group events by date
function groupEventsByDate(events: JAEventDto[]): Map<string, JAEventDto[]> {
  const grouped = new Map<string, JAEventDto[]>();
  
  events.forEach(event => {
    const dateKey = format(new Date(event.eventDate), 'yyyy-MM-dd');
    const existing = grouped.get(dateKey) || [];
    grouped.set(dateKey, [...existing, event]);
  });
  
  return grouped;
}

export default function CalendarPage() {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);

  const { data: events, isLoading } = useQuery({
    queryKey: ['events', 'all'],
    queryFn: () => eventService.getAll(),
  });

  const monthStart = startOfMonth(currentDate);
  const monthEnd = endOfMonth(currentDate);
  const daysInMonth = eachDayOfInterval({ start: monthStart, end: monthEnd });

  // Pad the beginning of the month to start on Sunday
  const startDay = monthStart.getDay();
  const paddedDays = Array(startDay).fill(null);

  const getEventsForDay = (date: Date) => {
    if (!events) return [];
    return events.filter(event => event && event.eventDate && isSameDay(new Date(event.eventDate), date));
  };

  const selectedDayEvents = selectedDate ? getEventsForDay(selectedDate) : [];

  // Get upcoming events (not completed, in the future) grouped by date
  const upcomingEvents = (events || [])
    .filter(e => e && e.eventDate && new Date(e.eventDate) >= new Date())
    .sort((a, b) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime());
  
  const groupedUpcomingEvents = groupEventsByDate(upcomingEvents);

  const goToPrevMonth = () => setCurrentDate(subMonths(currentDate, 1));
  const goToNextMonth = () => setCurrentDate(addMonths(currentDate, 1));

  return (
    <ProtectedLayout>
      <div className="p-6 space-y-6">
        <div>
          <h1 className="text-3xl font-bold">Calendar</h1>
          <p className="text-muted-foreground">View and manage your interview schedule</p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Calendar Grid */}
          <Card className="lg:col-span-2">
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-4">
              <CardTitle className="text-xl">
                {format(currentDate, 'MMMM yyyy')}
              </CardTitle>
              <div className="flex gap-1">
                <Button variant="ghost" size="icon" onClick={goToPrevMonth}>
                  <ChevronLeft className="h-4 w-4" />
                </Button>
                <Button variant="ghost" size="icon" onClick={goToNextMonth}>
                  <ChevronRight className="h-4 w-4" />
                </Button>
              </div>
            </CardHeader>
            <CardContent>
              {isLoading ? (
                <Skeleton className="h-[400px] w-full" />
              ) : (
                <div className="grid grid-cols-7 gap-1">
                  {/* Day headers */}
                  {['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'].map(day => (
                    <div key={day} className="p-2 text-center text-sm font-medium text-muted-foreground">
                      {day}
                    </div>
                  ))}
                  
                  {/* Empty cells for padding */}
                  {paddedDays.map((_, index) => (
                    <div key={`pad-${index}`} className="p-2 min-h-[80px]" />
                  ))}
                  
                  {/* Day cells */}
                  {daysInMonth.map(day => {
                    const dayEvents = getEventsForDay(day);
                    const isSelected = selectedDate && isSameDay(day, selectedDate);
                    const isCurrentDay = isToday(day);
                    const dayEventsCount = dayEvents.length;
                    
                    return (
                      <button
                        key={day.toISOString()}
                        onClick={() => setSelectedDate(day)}
                        className={`
                          p-2 min-h-[80px] border rounded-lg text-left transition-colors
                          hover:bg-accent/50
                          ${isSelected ? 'border-primary bg-primary/5' : 'border-transparent'}
                          ${!isSameMonth(day, currentDate) ? 'text-muted-foreground/50' : ''}
                        `}
                      >
                        <span className={`
                          inline-flex items-center justify-center w-7 h-7 rounded-full text-sm
                          ${isCurrentDay ? 'bg-primary text-primary-foreground font-bold' : ''}
                        `}>
                          {format(day, 'd')}
                        </span>
                        <div className="mt-1 space-y-1">
                          {dayEvents.slice(0, 2).map(event => {
                            const bgColor = eventTypeBgColors[event.eventType];
                            return (
                              <div
                                key={event.id}
                                className={`text-xs px-1.5 py-0.5 rounded truncate text-white ${bgColor}`}
                              >
                                {event.eventName}
                              </div>
                            );
                          })}
                          {dayEventsCount > 2 && (
                            <div className="text-xs text-muted-foreground px-1">
                              +{dayEventsCount - 2} more
                            </div>
                          )}
                        </div>
                      </button>
                    );
                  })}
                </div>
              )}
            </CardContent>
          </Card>

          {/* Selected Day Events */}
          <Card>
            <CardHeader>
              <CardTitle className="text-lg">
                {selectedDate ? format(selectedDate, 'EEEE, MMMM d') : 'Select a date'}
              </CardTitle>
            </CardHeader>
            <CardContent>
              {!selectedDate ? (
                <p className="text-muted-foreground text-sm">
                  Click on a date to view events
                </p>
              ) : selectedDayEvents.length === 0 ? (
                <p className="text-muted-foreground text-sm">
                  No events scheduled for this day
                </p>
              ) : (
                <div className="space-y-4">
                  {selectedDayEvents.map(event => (
                    <EventCard key={event.id} event={event} />
                  ))}
                </div>
              )}
            </CardContent>
          </Card>
        </div>

        {/* Upcoming Events List - Grouped by Date */}
        <Card>
          <CardHeader>
            <CardTitle>Upcoming Events</CardTitle>
          </CardHeader>
          <CardContent>
            {isLoading ? (
              <div className="space-y-3">
                {[1, 2, 3].map(i => (
                  <Skeleton key={i} className="h-20 w-full" />
                ))}
              </div>
            ) : upcomingEvents.length === 0 ? (
              <p className="text-muted-foreground">No upcoming events</p>
            ) : (
              <div className="space-y-6">
                {Array.from(groupedUpcomingEvents.entries()).map(([dateKey, dayEvents]) => (
                  <div key={dateKey}>
                    <h3 className="text-sm font-semibold text-muted-foreground mb-3 flex items-center gap-2">
                      <CalendarIcon className="h-4 w-4" />
                      {format(new Date(dateKey), 'EEEE, MMMM d, yyyy')}
                    </h3>
                    <div className="space-y-3 pl-6 border-l-2 border-border">
                      {dayEvents.map(event => (
                        <EventCard key={event.id} event={event} />
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </CardContent>
        </Card>
      </div>
    </ProtectedLayout>
  );
}

function EventCard({ event }: { event: JAEventDto }) {
  const Icon = eventTypeIcons[event.eventType];
  const bgColor = eventTypeBgColors[event.eventType];
  const label = eventTypeLabels[event.eventType];

  return (
    <div className="block p-4 border border-border rounded-lg hover:bg-accent/50 transition-colors">
      <div className="flex items-start gap-3">
        <div className={`p-2 rounded-lg ${bgColor}`}>
          <Icon className="h-4 w-4 text-white" />
        </div>
        <div className="flex-1 min-w-0">
          <div className="flex items-center gap-2 mb-1">
            <h4 className="font-medium truncate">{event.eventName || 'Unnamed Event'}</h4>
            <Badge variant="secondary" className="text-xs">
              {label}
            </Badge>
          </div>
          {event.note && <p className="text-sm text-muted-foreground truncate">{event.note}</p>}
          <div className="flex items-center gap-4 mt-2 text-xs text-muted-foreground">
            <span className="flex items-center gap-1">
              <Clock className="h-3 w-3" />
              {format(new Date(event.eventDate), 'h:mm a')}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}
