'use client';

import { useRouter } from 'next/navigation';
import { Badge } from '@/components/ui/badge';
import { Clock } from 'lucide-react';
import { eventTypeLabels } from '@/types/Enums/JAEventType';
import type { JAEventDto } from '@/types/JAObjects/JAEvents/JAEventDto';
import { format } from 'date-fns';

interface EventCardProps {
  event: JAEventDto;
  icon: React.ElementType;
  bgColor: string;
}

export function EventCard({ event, icon: Icon, bgColor }: EventCardProps) {
  const router = useRouter();
  const label = eventTypeLabels[event.eventType];

  const handleClick = () => {
    router.push(`/applications/${event.jobApplicationId}`);
  };

  return (
    <button
      onClick={handleClick}
      className="block w-full p-4 border border-border rounded-lg hover:bg-accent/50 transition-colors text-left"
    >
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
    </button>
  );
}
