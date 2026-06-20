'use client';

import { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { applicationService } from '@/services/application-service';
import { CreateJobApplicationDto } from '@/types/JAObjects/JobApplications/CreateJobApplicationDto';
import { CreateJAEventDto } from '@/types/JAObjects/JAEvents/CreateJAEventDto';
import { JAStatusType } from '@/types/Enums/JAStatusType';
import { JAEventType } from '@/types/Enums/JAEventType';
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

interface CreateApplicationModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSuccess: () => void;
  prefillData?: Partial<CreateJobApplicationDto>;
}

export function CreateApplicationModal({
  open,
  onOpenChange,
  onSuccess,
  prefillData,
}: CreateApplicationModalProps) {
  const [formData, setFormData] = useState<CreateJobApplicationDto>({
    company: prefillData?.company || '',
    position: prefillData?.position || '',
    note: prefillData?.note || '',
    jobDescription: prefillData?.jobDescription || '',
    initialStatus: {
      jobApplicationId: prefillData?.initialStatus?.jobApplicationId || '1',
      statusType: prefillData?.initialStatus?.statusType ?? JAStatusType.Wishlist,
      note: prefillData?.initialStatus?.note || '',
    },
    jaEvent: prefillData?.jaEvent || null,
  });

  const emptyJAEvent: CreateJAEventDto = {
    jaStatusEntryId: '1',
    eventName: '',
    eventType: JAEventType.Call,
    eventDate: '',
    isWholeDay: false,
    note: '',
  };
  
  const handleAddEventChange = (checked: boolean) => {
    setFormData((prev) => ({
      ...prev,
      jaEvent: checked ? emptyJAEvent : null,
    }));
  };
  
  const createMutation = useMutation({
    mutationFn: (data: CreateJobApplicationDto) => applicationService.create(data),
    onSuccess: () => {
      toast.success('Application added successfully');
      onSuccess();
      resetForm();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to create application';
      toast.error(errorMessage);
    },
  });

  const resetForm = () => {
    setFormData({
      company: '',
      position: '',
      note: '',
      jobDescription: '',
      initialStatus: {
        jobApplicationId: '',
        statusType: JAStatusType.Wishlist,
        note: '',
      },
      jaEvent: null,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.company || !formData.position) {
      toast.error('Company name and position are required');
      return;
    }

    createMutation.mutate(formData);
  };

  const handleChange = (
    field: keyof CreateJobApplicationDto,
    value: string | number | undefined
  ) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  const handleNestedChange = (
    parent: 'initialStatus' | 'jaEvent',
    field: string,
    value: string | number | boolean
  ) => {
    setFormData(prev => ({
      ...prev,
      [parent]: prev[parent] ? { ...prev[parent], [field]: value } : null,
    }));
  };

  return (
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogContent className="sm:max-w-[720px] max-h-[90vh] overflow-y-auto">
          <form onSubmit={handleSubmit}>
            <DialogHeader>
              <DialogTitle>Create Job Application</DialogTitle>
              <DialogDescription>
                Track a new opportunity and optionally schedule an event.
              </DialogDescription>
            </DialogHeader>

            <div className="space-y-6 py-4">
              {/* Basic Information */}
              <section className="space-y-4">
                <div className="grid gap-4 md:grid-cols-2">
                  <div className="space-y-2">
                    <Label htmlFor="company">Company *</Label>
                    <Input
                        id="company"
                        value={formData.company}
                        onChange={(e) => handleChange("company", e.target.value)}
                        placeholder="Company name"
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="position">Position *</Label>
                    <Input
                        id="position"
                        value={formData.position}
                        onChange={(e) => handleChange("position", e.target.value)}
                        placeholder="Position title"
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="note">Application Notes</Label>
                  <Textarea
                      id="note"
                      value={formData.note ?? ""}
                      onChange={(e) => handleChange("note", e.target.value)}
                      placeholder="Recruiter contact, special expectations, salary expectations..."
                      rows={3}
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="jobDescription">Job Description</Label>
                  <Textarea
                      id="jobDescription"
                      value={formData.jobDescription ?? ""}
                      onChange={(e) =>
                          handleChange("jobDescription", e.target.value)
                      }
                      placeholder="Paste the job description..."
                      rows={6}
                  />
                </div>
              </section>

              <Separator />

              {/* Status */}
              <section className="space-y-4">
                <div>
                  <h3 className="font-semibold">Application Status</h3>
                  <p className="text-sm text-muted-foreground">
                    Track the current stage of this application.
                  </p>
                </div>

                <div className="space-y-2">
                  <Label>Status Type *</Label>
                  <Select
                      value={formData.initialStatus.statusType.toString()}
                      onValueChange={(value) =>
                          handleNestedChange(
                              "initialStatus",
                              "statusType",
                              Number(value)
                          )
                      }
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select status" />
                    </SelectTrigger>

                    <SelectContent>
                      {Object.entries(JAStatusType)
                          .filter(([key, value]) => isNaN(Number(key)) && (value as number) < JAStatusType.Accepted)
                          .map(([key, value]) => (
                              <SelectItem
                                  key={value}
                                  value={value.toString()}
                              >
                                {key.replace(/([A-Z])/g, " $1").trim()}
                              </SelectItem>
                          ))}
                    </SelectContent>
                  </Select>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="initialStatusNote">Status Notes</Label>
                  <Textarea
                      id="initialStatusNote"
                      value={formData.initialStatus.note ?? ""}
                      onChange={(e) =>
                          handleNestedChange(
                              "initialStatus",
                              "note",
                              e.target.value
                          )
                      }
                      placeholder="Optional notes about this status..."
                      rows={3}
                  />
                </div>
              </section>

              <Separator />

              {/* Event Toggle */}
              <section className="space-y-4">
                <div className="flex items-center justify-between rounded-lg border p-4">
                  <div>
                    <p className="font-medium">Schedule an Event</p>
                    <p className="text-sm text-muted-foreground">
                      Add an interview, follow-up, reminder, or deadline.
                    </p>
                  </div>

                  <Checkbox
                      checked={formData.jaEvent !== null}
                      onCheckedChange={(checked) =>
                          handleAddEventChange(Boolean(checked))
                      }
                  />
                </div>

                {formData.jaEvent && (
                    <div className="space-y-4 rounded-lg border p-4">
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
                                      "jaEvent",
                                      "eventType",
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
                                        {key.replace(/([A-Z])/g, " $1").trim()}
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
                                      "jaEvent",
                                      "eventName",
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
                                    "jaEvent",
                                    "isWholeDay",
                                    Boolean(checked)
                                )
                            }
                        />
                        <Label htmlFor="isWholeDay" className="cursor-pointer">
                          All-day event
                        </Label>
                      </div>

                      <div className="grid gap-4 md:grid-cols-2">
                        <div className="space-y-2">
                          <Label htmlFor="eventDateOnly">Date *</Label>
                          <Input
                              id="eventDateOnly"
                              type="date"
                              value={formData.jaEvent.eventDate ? formData.jaEvent.eventDate.split('T')[0] : ''}
                              onChange={(e) => {
                                const time = formData.jaEvent.eventDate ? formData.jaEvent.eventDate.split('T')[1] || '00:00' : '00:00';
                                handleNestedChange(
                                    "jaEvent",
                                    "eventDate",
                                    `${e.target.value}T${time}`
                                );
                              }}
                          />
                        </div>

                        <div className="space-y-2">
                          <Label htmlFor="eventTime">Time {!formData.jaEvent.isWholeDay && '*'}</Label>
                          <Input
                              id="eventTime"
                              type="time"
                              step="60"
                              disabled={formData.jaEvent.isWholeDay}
                              value={formData.jaEvent.eventDate ? formData.jaEvent.eventDate.split('T')[1] || '' : ''}
                              onChange={(e) => {
                                const date = formData.jaEvent.eventDate ? formData.jaEvent.eventDate.split('T')[0] : '';
                                handleNestedChange(
                                    "jaEvent",
                                    "eventDate",
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
                            value={formData.jaEvent.note ?? ""}
                            onChange={(e) =>
                                handleNestedChange(
                                    "jaEvent",
                                    "note",
                                    e.target.value
                                )
                            }
                            placeholder="Meeting link, interviewer names, preparation notes..."
                            rows={3}
                        />
                      </div>
                    </div>
                )}
              </section>
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
                    "Create Application"
                )}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog> 
  );
}
