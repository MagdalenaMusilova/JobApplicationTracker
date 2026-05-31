'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { eventService } from '@/services/event-service';
import {
  ApplicationEventType,
  CreateApplicationEventDto,
  eventTypeLabels,
} from '@/types';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Loader2 } from 'lucide-react';
import { toast } from 'sonner';

interface CreateEventModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  applicationId: string;
}

export function CreateEventModal({
  open,
  onOpenChange,
  applicationId,
}: CreateEventModalProps) {
  const [formData, setFormData] = useState({
    eventType: ApplicationEventType.Interview,
    scheduledAt: '',
    description: '',
    location: '',
    notes: '',
  });
  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: CreateApplicationEventDto) => eventService.create(data),
    onSuccess: () => {
      toast.success('Event created successfully');
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      onOpenChange(false);
      resetForm();
    },
    onError: () => {
      toast.error('Failed to create event');
    },
  });

  const resetForm = () => {
    setFormData({
      eventType: ApplicationEventType.Interview,
      scheduledAt: '',
      description: '',
      location: '',
      notes: '',
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.scheduledAt) {
      toast.error('Please select a date and time');
      return;
    }

    createMutation.mutate({
      applicationId,
      eventType: formData.eventType,
      scheduledAt: new Date(formData.scheduledAt).toISOString(),
      description: formData.description || undefined,
      location: formData.location || undefined,
      notes: formData.notes || undefined,
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add Event</DialogTitle>
            <DialogDescription>
              Schedule an interview, call, or follow-up for this application.
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="eventType">Event Type</Label>
              <Select
                value={formData.eventType.toString()}
                onValueChange={(value) =>
                  setFormData(prev => ({ ...prev, eventType: Number(value) }))
                }
              >
                <SelectTrigger className="bg-input">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {Object.entries(eventTypeLabels).map(([value, label]) => (
                    <SelectItem key={value} value={value}>
                      {label}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="scheduledAt">Date & Time *</Label>
              <Input
                id="scheduledAt"
                type="datetime-local"
                value={formData.scheduledAt}
                onChange={(e) =>
                  setFormData(prev => ({ ...prev, scheduledAt: e.target.value }))
                }
                className="bg-input"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="description">Description</Label>
              <Input
                id="description"
                value={formData.description}
                onChange={(e) =>
                  setFormData(prev => ({ ...prev, description: e.target.value }))
                }
                placeholder="e.g., Technical interview with engineering team"
                className="bg-input"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="location">Location</Label>
              <Input
                id="location"
                value={formData.location}
                onChange={(e) =>
                  setFormData(prev => ({ ...prev, location: e.target.value }))
                }
                placeholder="e.g., Zoom, Google Meet, Office"
                className="bg-input"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="notes">Notes</Label>
              <Textarea
                id="notes"
                value={formData.notes}
                onChange={(e) =>
                  setFormData(prev => ({ ...prev, notes: e.target.value }))
                }
                placeholder="Any additional notes..."
                rows={2}
                className="bg-input"
              />
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={() => onOpenChange(false)}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={createMutation.isPending}>
              {createMutation.isPending ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Creating...
                </>
              ) : (
                'Create Event'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
