'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { eventService } from '@/services/event-service';
import { JAEventType } from '@/types/Enums/JAEventType';
import { CreateJAEventDto } from '@/types/JAObjects/JAEvents/CreateJAEventDto';
import { eventTypeLabels } from '@/types/Enums/JAEventType';
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
    eventType: JAEventType.Interview,
    eventDate: '',
    eventName: '',
    note: '',
    isWholeDay: false,
  });

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: CreateJAEventDto) => eventService.create(data),
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
      eventType: JAEventType.Interview,
      eventDate: '',
      eventName: '',
      note: '',
      isWholeDay: false,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.eventDate) {
      toast.error('Please select a date and time');
      return;
    }

    createMutation.mutate({
      jaStatusEntryId: '',
      eventName: formData.eventName || 'Event',
      eventType: formData.eventType,
      eventDate: new Date(formData.eventDate).toISOString(),
      isWholeDay: formData.isWholeDay,
      note: formData.note || undefined,
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
                        setFormData((prev) => ({
                          ...prev,
                          eventType: Number(value),
                        }))
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
                <Label htmlFor="eventDate">Date & Time *</Label>
                <Input
                    id="eventDate"
                    type="datetime-local"
                    value={formData.eventDate}
                    onChange={(e) =>
                        setFormData((prev) => ({
                          ...prev,
                          eventDate: e.target.value,
                        }))
                    }
                    className="bg-input"
                />
              </div>

              <div className="flex items-center space-x-2">
                <input
                    id="isWholeDay"
                    type="checkbox"
                    checked={formData.isWholeDay}
                    onChange={(e) =>
                        setFormData((prev) => ({
                          ...prev,
                          isWholeDay: e.target.checked,
                        }))
                    }
                />
                <Label htmlFor="isWholeDay">Whole day event</Label>
              </div>

              <div className="space-y-2">
                <Label htmlFor="eventName">Event Name</Label>
                <Input
                    id="eventName"
                    value={formData.eventName}
                    onChange={(e) =>
                        setFormData((prev) => ({
                          ...prev,
                          eventName: e.target.value,
                        }))
                    }
                    placeholder="e.g., Technical interview with engineering team"
                    className="bg-input"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="note">Notes</Label>
                <Textarea
                    id="note"
                    value={formData.note}
                    onChange={(e) =>
                        setFormData((prev) => ({
                          ...prev,
                          note: e.target.value,
                        }))
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