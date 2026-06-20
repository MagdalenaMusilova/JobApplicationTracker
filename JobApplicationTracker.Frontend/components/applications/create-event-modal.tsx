'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { eventService } from '@/services/event-service';
import { JAEventType } from '@/types/Enums/JAEventType';
import { CreateJAEventDto } from '@/types/JAObjects/JAEvents/CreateJAEventDto';

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

import { Checkbox } from '@/components/ui/checkbox';
import { Separator } from '@/components/ui/separator';

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
    jaEvent: {
      jaStatusEntryId: '',
      eventType: JAEventType.Call,
      eventDate: '',
      eventName: '',
      note: '',
      isWholeDay: false,
    },
  });

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: CreateJAEventDto) => eventService.create(data),

    onSuccess: () => {
      toast.success('Event created successfully');

      queryClient.invalidateQueries({
        queryKey: ['application', applicationId],
      });

      queryClient.invalidateQueries({
        queryKey: ['events'],
      });

      onOpenChange(false);
      resetForm();
    },

    onError: (error: any) => {
      const errorMessage =
          error?.response?.data?.error || 'Failed to create event';

      toast.error(errorMessage);
    },
  });

  const resetForm = () => {
    setFormData({
      jaEvent: {
        jaStatusEntryId: '',
        eventType: JAEventType.Call,
        eventDate: '',
        eventName: '',
        note: '',
        isWholeDay: false,
      },
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.jaEvent.eventDate) {
      toast.error('Please select a date and time');
      return;
    }

    createMutation.mutate({
      jaStatusEntryId: '',
      eventName: formData.jaEvent.eventName || 'Event',
      eventType: formData.jaEvent.eventType,
      eventDate: new Date(formData.jaEvent.eventDate).toISOString(),
      isWholeDay: formData.jaEvent.isWholeDay,
      note: formData.jaEvent.note || undefined,
    });
  };

  const handleNestedChange = (
      parent: 'jaEvent',
      field: string,
      value: string | number | boolean
  ) => {
    setFormData((prev) => ({
      ...prev,
      [parent]: prev[parent]
          ? {
            ...prev[parent],
            [field]: value,
          }
          : null,
    }));
  };

  return (
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent className="sm:max-w-[720px] max-h-[90vh] overflow-y-auto">
          <form onSubmit={handleSubmit}>
            <DialogHeader>
              <DialogTitle>Create Event</DialogTitle>

              <DialogDescription>
                Schedule an interview, follow-up, reminder, or deadline.
              </DialogDescription>
            </DialogHeader>

            <div className="space-y-6 py-4">
              <section className="space-y-4">
                <div>
                  <h3 className="font-semibold">Event Details</h3>

                  <p className="text-sm text-muted-foreground">
                    Information about the scheduled event.
                  </p>
                </div>

                <div className="grid gap-4 md:grid-cols-2">
                  <div className="space-y-2">
                    <Label>Event Type *</Label>

                    <Select
                        value={formData.jaEvent.eventType.toString()}
                        onValueChange={(value) =>
                            handleNestedChange(
                                'jaEvent',
                                'eventType',
                                Number(value)
                            )
                        }
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Select event type" />
                      </SelectTrigger>

                      <SelectContent>
                        {Object.entries(JAEventType)
                            .filter(([key]) => isNaN(Number(key)))
                            .map(([key, value]) => (
                                <SelectItem
                                    key={value}
                                    value={value.toString()}
                                >
                                  {key.replace(/([A-Z])/g, ' $1').trim()}
                                </SelectItem>
                            ))}
                      </SelectContent>
                    </Select>
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="eventName">Event Name *</Label>

                    <Input
                        id="eventName"
                        value={formData.jaEvent.eventName}
                        onChange={(e) =>
                            handleNestedChange(
                                'jaEvent',
                                'eventName',
                                e.target.value
                            )
                        }
                        placeholder="e.g., In-person interview"
                    />
                  </div>
                </div>

                <div className="flex items-center gap-2">
                  <Checkbox
                      id="isWholeDay"
                      checked={formData.jaEvent.isWholeDay}
                      onCheckedChange={(checked) =>
                          handleNestedChange(
                              'jaEvent',
                              'isWholeDay',
                              Boolean(checked)
                          )
                      }
                  />

                  <Label
                      htmlFor="isWholeDay"
                      className="cursor-pointer"
                  >
                    All-day event
                  </Label>
                </div>

                <div className="grid gap-4 md:grid-cols-2">
                  <div className="space-y-2">
                    <Label htmlFor="eventDateOnly">Date *</Label>

                    <Input
                        id="eventDateOnly"
                        type="date"
                        value={
                          formData.jaEvent.eventDate
                              ? formData.jaEvent.eventDate.split('T')[0]
                              : ''
                        }
                        onChange={(e) => {
                          const time = formData.jaEvent.eventDate
                              ? formData.jaEvent.eventDate.split('T')[1] ||
                              '00:00'
                              : '00:00';

                          handleNestedChange(
                              'jaEvent',
                              'eventDate',
                              `${e.target.value}T${time}`
                          );
                        }}
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="eventTime">
                      Time {!formData.jaEvent.isWholeDay && '*'}
                    </Label>

                    <Input
                        id="eventTime"
                        type="time"
                        step="60"
                        disabled={formData.jaEvent.isWholeDay}
                        value={
                          formData.jaEvent.eventDate
                              ? formData.jaEvent.eventDate.split('T')[1] || ''
                              : ''
                        }
                        onChange={(e) => {
                          const date = formData.jaEvent.eventDate
                              ? formData.jaEvent.eventDate.split('T')[0]
                              : '';

                          handleNestedChange(
                              'jaEvent',
                              'eventDate',
                              `${date}T${e.target.value}`
                          );
                        }}
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="eventNote">Event Notes</Label>

                  <Textarea
                      id="eventNote"
                      value={formData.jaEvent.note ?? ''}
                      onChange={(e) =>
                          handleNestedChange(
                              'jaEvent',
                              'note',
                              e.target.value
                          )
                      }
                      placeholder="Meeting link, interviewer names, preparation notes..."
                      rows={3}
                  />
                </div>
              </section>

              <Separator />
            </div>

            <DialogFooter>
              <Button
                  type="button"
                  variant="outline"
                  onClick={() => onOpenChange(false)}
              >
                Cancel
              </Button>

              <Button
                  type="submit"
                  disabled={createMutation.isPending}
              >
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