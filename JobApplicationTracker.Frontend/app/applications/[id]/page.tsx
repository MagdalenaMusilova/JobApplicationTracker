'use client';

import { useState, useEffect, use } from 'react';
import { useRouter } from 'next/navigation';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { format, formatDistanceToNow } from 'date-fns';
import { ArrowLeft, Pencil, X, Check, Plus, Trash2, Calendar, Clock } from 'lucide-react';

import { cn } from '@/lib/utils';
import { applicationService } from '@/services/application-service';
import { eventService } from '@/services/event-service';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Label } from '@/components/ui/label';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Skeleton } from '@/components/ui/skeleton';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from '@/components/ui/dialog';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';

import { JobApplicationDto } from '@/types/JAObjects/JobApplications/JobApplicationDto';
import { JAStatusEntryDto } from '@/types/JAObjects/JAStatuses/JAStatusEntryDto';
import { UpdateJobApplicationDto } from '@/types/JAObjects/JobApplications/UpdateJobApplicationDto';
import { JobApplicationMinimalDto } from '@/types/JAObjects/JobApplications/JobApplicationMinimalDto';
import { JAStatusType, applicationStatusColors, jaStatusLabels } from '@/types/Enums/JAStatusType';
import { JAEventType, eventTypeLabels, eventTypeColors } from '@/types/Enums/JAEventType';

interface PageProps {
  params: Promise<{ id: string }>;
}

export default function ApplicationDetailPage({ params }: PageProps) {
  const { id: applicationId } = use(params);
  const router = useRouter();
  const queryClient = useQueryClient();
  
  // Edit mode state for batch editing
  const [isEditing, setIsEditing] = useState(false);
  const [editForm, setEditForm] = useState<UpdateJobApplicationDto>({});

  // Status history modal state
  const [showStatusModal, setShowStatusModal] = useState(false);
  const [statusModalMode, setStatusModalMode] = useState<'add' | 'edit'>('add');
  const [editingStatusId, setEditingStatusId] = useState<string | null>(null);
  const [statusForm, setStatusForm] = useState({ jaStatusType: JAStatusType.Applied, note: '' });
  const [showDeleteStatusDialog, setShowDeleteStatusDialog] = useState(false);

  // Event modal state
  const [showEventModal, setShowEventModal] = useState(false);
  const [eventModalMode, setEventModalMode] = useState<'add' | 'edit'>('add');
  const [eventForm, setEventForm] = useState({
    eventType: JAEventType.Interview,
    eventDate: '',
    eventName: '',
    note: '',
  });
  const [showDeleteEventDialog, setShowDeleteEventDialog] = useState(false);

  // Fetch application data
  const { data: application, isLoading, error } = useQuery({
    queryKey: ['application', applicationId],
    queryFn: () => applicationService.getById(applicationId),
  });

  // Initialize edit form when application loads
  useEffect(() => {
    if (application) {
      setEditForm({
        company: application.company,
        position: application.position,
        jobDescription: application.jobListing?.jobDescription || '',
        note: application.note || '',
      });
    }
  }, [application]);

  // Update application mutation (batch edit)
  const updateMutation = useMutation({
    mutationFn: (data: UpdateJobApplicationDto) => applicationService.update(applicationId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['applications'] });
      setIsEditing(false);
    },
  });

  // Add status mutation
  const addStatusMutation = useMutation({
    mutationFn: (data: { statusType: JAStatusType; note?: string }) =>
      applicationService.updateStatus({
        jobApplicationId: applicationId,
        statusType: data.statusType,
        note: data.note,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['applications'] });
      setShowStatusModal(false);
      setStatusForm({ jaStatusType: JAStatusType.Applied, note: '' });
    },
  });

  // Edit last status mutation
  const editStatusMutation = useMutation({
    mutationFn: async (data: { id: string; jaStatusType: JAStatusType; note?: string }) => {
      // Simulated - in real app this would call API
      return Promise.resolve();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      setShowStatusModal(false);
    },
  });

  // Delete last status mutation
  const deleteStatusMutation = useMutation({
    mutationFn: async () => {
      // Simulated - in real app this would call API
      return Promise.resolve();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      setShowDeleteStatusDialog(false);
    },
  });

  // Event mutations
  const addEventMutation = useMutation({
    mutationFn: (data: typeof eventForm) =>
      eventService.create({
        jaStatusEntryId: application?.statusHistory[application.statusHistory.length - 1]?.id || '',
        eventType: data.eventType,
        eventDate: new Date(data.eventDate).toISOString(),
        eventName: data.eventName || 'Event',
        isWholeDay: false,
        note: data.note || undefined,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      setShowEventModal(false);
      resetEventForm();
    },
  });

  const updateEventMutation = useMutation({
    mutationFn: (data: typeof eventForm) =>
      eventService.update(lastStatus?.jaEvent?.id || '', {
        eventType: data.eventType,
        eventDate: new Date(data.eventDate).toISOString(),
        eventName: data.eventName || 'Event',
        isWholeDay: false,
        note: data.note || undefined,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      setShowEventModal(false);
      resetEventForm();
    },
  });

  const deleteEventMutation = useMutation({
    mutationFn: () => eventService.delete(lastStatus?.jaEvent?.id || ''),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      setShowDeleteEventDialog(false);
    },
  });

  const resetEventForm = () => {
    setEventForm({
      eventType: JAEventType.Interview,
      eventDate: '',
      eventName: '',
      note: '',
    });
  };

  const handleSaveApplication = () => {
    updateMutation.mutate(editForm);
  };

  const handleDiscardChanges = () => {
    if (application) {
      setEditForm({
        company: application.company,
        position: application.position,
        jobDescription: application.jobListing?.jobDescription || '',
        note: application.note || '',
      });
    }
    setIsEditing(false);
  };

  const openAddStatusModal = () => {
    setStatusModalMode('add');
    const currentStatus = application?.statusHistory[application.statusHistory.length - 1]?.jaStatusType ?? JAStatusType.Applied;
    const nextStatus = currentStatus + 1;
    setStatusForm({ jaStatusType: Math.min(nextStatus, JAStatusType.Withdrawn), note: '' });
    setShowStatusModal(true);
  };

  const openEditStatusModal = (status: JAStatusEntryDto) => {
    setStatusModalMode('edit');
    setEditingStatusId(status.id);
    setStatusForm({ jaStatusType: status.jaStatusType, note: status.note || '' });
    setShowStatusModal(true);
  };

  const openAddEventModal = () => {
    setEventModalMode('add');
    resetEventForm();
    setShowEventModal(true);
  };

  const openEditEventModal = () => {
    const event = lastStatus?.jaEvent;
    if (event) {
      setEventModalMode('edit');
      setEventForm({
        eventType: event.eventType,
        eventDate: new Date(event.eventDate).toISOString().slice(0, 16),
        eventName: event.eventName || '',
        note: event.note || '',
      });
      setShowEventModal(true);
    }
  };

  const handleSaveStatus = () => {
    if (statusModalMode === 'add') {
      addStatusMutation.mutate({ statusType: statusForm.jaStatusType, note: statusForm.note || undefined });
    } else {
      editStatusMutation.mutate({ id: editingStatusId!, jaStatusType: statusForm.jaStatusType, note: statusForm.note || undefined });
    }
  };

  const handleSaveEvent = () => {
    if (eventModalMode === 'add') {
      addEventMutation.mutate(eventForm);
    } else {
      updateEventMutation.mutate(eventForm);
    }
  };

  if (isLoading) {
    return (
      <ProtectedLayout>
        <div className="space-y-6">
          <Skeleton className="h-8 w-64" />
          <Skeleton className="h-64 w-full" />
          <Skeleton className="h-48 w-full" />
          <Skeleton className="h-32 w-full" />
        </div>
      </ProtectedLayout>
    );
  }

  if (error || !application) {
    return (
      <ProtectedLayout>
        <div className="text-center py-12">
          <p className="text-muted-foreground">Application not found</p>
          <Button variant="link" onClick={() => router.push('/applications')}>
            Back to Applications
          </Button>
        </div>
      </ProtectedLayout>
    );
  }

  const sortedStatusHistory = [...application.statusHistory].sort(
      (a, b) => b.orderIndex - a.orderIndex
  );
  const lastStatus = application.statusHistory[application.statusHistory.length - 1];
  const canDeleteStatus = application.statusHistory.length > 1;

  return (
    <ProtectedLayout>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button variant="ghost" size="icon" onClick={() => router.push('/applications')}>
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold">
                {isEditing ? 'Edit Application' : application.company}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? 'Make changes and save or discard' : application.position}
              </p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            {isEditing ? (
              <>
                <Button variant="outline" onClick={handleDiscardChanges}>
                  <X className="h-4 w-4 mr-2" />
                  Discard
                </Button>
                <Button onClick={handleSaveApplication} disabled={updateMutation.isPending}>
                  <Check className="h-4 w-4 mr-2" />
                  {updateMutation.isPending ? 'Saving...' : 'Save Changes'}
                </Button>
              </>
            ) : (
              <Button onClick={() => setIsEditing(true)}>
                <Pencil className="h-4 w-4 mr-2" />
                Edit Application
              </Button>
            )}
          </div>
        </div>

        {/* Application Details Card (batch editable) */}
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <CardTitle>Application Details</CardTitle>
              <Badge className={applicationStatusColors[lastStatus.jaStatusType] as unknown as JAStatusType}>
            {jaStatusLabels[lastStatus.jaStatusType as unknown as JAStatusType]}
          </Badge>
            </div>
          </CardHeader>
          <CardContent className="space-y-6">
            {/* Company & Position */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <Label>Company</Label>
                {isEditing ? (
                  <Input
                    value={editForm.company || ''}
                    onChange={(e) => setEditForm({ ...editForm, company: e.target.value })}
                  />
                ) : (
                  <p className="text-lg font-medium">{application.company}</p>
                )}
              </div>
              <div className="space-y-2">
                <Label>Position</Label>
                {isEditing ? (
                  <Input
                    value={editForm.position || ''}
                    onChange={(e) => setEditForm({ ...editForm, position: e.target.value })}
                  />
                ) : (
                  <p className="text-lg font-medium">{application.position}</p>
                )}
              </div>
            </div>

            {/* Timestamps (always readonly) */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <Label className="text-muted-foreground">Created At</Label>
                <p className="text-sm">
                  {application.statusHistory.length > 0 ? (
                    <>
                      {format(new Date(application.statusHistory[0].createdAt), 'PPP')}
                      <span className="text-muted-foreground ml-2">
                        ({formatDistanceToNow(new Date(application.statusHistory[0].createdAt), { addSuffix: true })})
                      </span>
                    </>
                  ) : 'N/A'}
                </p>
              </div>
              <div className="space-y-2">
                <Label className="text-muted-foreground">Last Updated</Label>
                <p className="text-sm">
                  {application.statusHistory.length > 0 ? (
                    <>
                      {format(new Date(lastStatus.createdAt), 'PPP')}
                      <span className="text-muted-foreground ml-2">
                        ({formatDistanceToNow(new Date(lastStatus.createdAt), { addSuffix: true })})
                      </span>
                    </>
                  ) : 'N/A'}
                </p>
              </div>
            </div>

            {/* Notes */}
            <div className="space-y-2">
              <Label>Notes</Label>
              {isEditing ? (
                <Textarea
                  rows={3}
                  value={editForm.note || ''}
                  onChange={(e) => setEditForm({ ...editForm, note: e.target.value })}
                  placeholder="Add notes about this application..."
                />
              ) : (
                <p className="text-sm text-muted-foreground whitespace-pre-wrap">
                  {application.note || 'No notes'}
                </p>
              )}
            </div>

            {/* Job Description */}
            <div className="space-y-2">
              <Label>Job Description</Label>
              {isEditing ? (
                <Textarea
                  rows={8}
                  value={editForm.jobDescription || ''}
                  onChange={(e) => setEditForm({ ...editForm, jobDescription: e.target.value })}
                  placeholder="Paste the job description here..."
                />
              ) : (
                <div className="text-sm text-muted-foreground whitespace-pre-wrap max-h-64 overflow-y-auto p-3 bg-muted/50 rounded-md">
                  {application.jobListing?.jobDescription || 'No job description'}
                </div>
              )}
            </div>
          </CardContent>
        </Card>

        {/* Status History Card (freely editable) */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle>Status History</CardTitle>
            <Button size="sm" onClick={openAddStatusModal}>
              <Plus className="h-4 w-4 mr-2" />
              Add Status
            </Button>
          </CardHeader>
          <CardContent>
            <ol className="relative">
              {application.statusHistory.map((status, index) => {
                const isLast = index === application.statusHistory.length - 1;
                const isCurrent = index === application.statusHistory.length - 1;
                return (
                    <li
                        key={status.id}
                        className="relative flex gap-4 pb-6 last:pb-0 group"
                    >
                      {/* Connecting line */}
                      {index < application.statusHistory.length - 1 && (
                          <span
                              className="absolute left-[11px] top-7 bottom-0 w-px bg-border"
                              aria-hidden="true"
                          />
                      )}

                      {/* Timeline marker */}
                      <div className="relative z-10 flex-shrink-0">
                      <span
                          className={`flex h-6 w-6 items-center justify-center rounded-full ring-4 ring-background ${
                              isCurrent
                                  ? 'bg-primary text-primary-foreground'
                                  : 'bg-muted text-muted-foreground'
                          }`}
                      >
                        {isCurrent ? (
                            <Check className="h-3.5 w-3.5" />
                        ) : (
                            <span className="h-2 w-2 rounded-full bg-current" />
                        )}
                      </span>
                      </div>

                      {/* Content */}
                      <div className="flex-1 min-w-0 -mt-0.5">
                        <div className="flex items-start justify-between gap-2">
                          <div className="flex flex-wrap items-center gap-2">
                            <Badge className={applicationStatusColors[status.jaStatusType]}>
                              {jaStatusLabels[status.jaStatusType]}
                            </Badge>
                            {isCurrent && (
                                <span className="text-xs font-medium text-primary">Current</span>
                            )}
                          </div>
                          {isLast && (
                              <div className="flex items-center gap-1 opacity-0 group-hover:opacity-100 transition-opacity">
                                <Button variant="ghost" size="icon" className="h-7 w-7" onClick={() => openEditStatusModal(status)}>
                                  <Pencil className="h-3.5 w-3.5" />
                                  <span className="sr-only">Edit status</span>
                                </Button>
                                {canDeleteStatus && (
                                    <Button
                                        variant="ghost"
                                        size="icon"
                                        className="h-7 w-7 text-destructive hover:text-destructive"
                                        onClick={() => setShowDeleteStatusDialog(true)}
                                    >
                                      <Trash2 className="h-3.5 w-3.5" />
                                      <span className="sr-only">Delete status</span>
                                    </Button>
                                )}
                              </div>
                          )}
                        </div>
                        <p className="text-xs text-muted-foreground mt-1.5">
                          {format(new Date(status.createdAt), 'PPP')} at {format(new Date(status.createdAt), 'p')}
                        </p>
                        {status.note && (
                            <p className="text-sm mt-2 text-foreground leading-relaxed">{status.note}</p>
                        )}
                      </div>
                    </li>
                );
              })}
            </ol>
          </CardContent>
        </Card>

        {/* Event Card (freely editable, single event) */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle>Scheduled Event</CardTitle>
            {!lastStatus.jaEvent && (
              <Button size="sm" onClick={openAddEventModal}>
                <Plus className="h-4 w-4 mr-2" />
                Add Event
              </Button>
            )}
          </CardHeader>
          <CardContent>
            {lastStatus.jaEvent ? (
              <div className="p-4 border border-border rounded-lg">
                <div className="flex items-start justify-between">
                  <div className="space-y-3">
                    <div className="flex items-center gap-3">
                      <Badge className={eventTypeColors[lastStatus.jaEvent.eventType]}>
                        {eventTypeLabels[lastStatus.jaEvent.eventType]}
                      </Badge>
                      {/*{lastStatus.jaEvent.isCompleted && (
                        <Badge variant="outline" className="text-green-600 border-green-600">
                          Completed
                        </Badge>
                      )}*/}
                    </div>
                    <div className="flex items-center gap-4 text-sm text-muted-foreground">
                      <span className="flex items-center gap-1">
                        <Calendar className="h-4 w-4" />
                        {format(new Date(lastStatus.jaEvent.eventDate), 'PPP')}
                      </span>
                      {!lastStatus.jaEvent.isWholeDay && (
                        <span className="flex items-center gap-1">
                          <Clock className="h-4 w-4" />
                          {format(new Date(lastStatus.jaEvent.eventDate), 'p')}
                        </span>
                      )}
                    </div>
                    {lastStatus.jaEvent.eventName && (
                      <p className="text-sm">{lastStatus.jaEvent.eventName}</p>
                    )}
                    {lastStatus.jaEvent.note && (
                      <p className="text-sm text-muted-foreground italic">{lastStatus.jaEvent.note}</p>
                    )}
                  </div>
                  <div className="flex items-center gap-1">
                    <Button variant="ghost" size="sm" onClick={openEditEventModal}>
                      <Pencil className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="sm"
                      className="text-destructive hover:text-destructive"
                      onClick={() => setShowDeleteEventDialog(true)}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
              </div>
            ) : (
              <p className="text-muted-foreground text-center py-8">
                No event scheduled. Click &quot;Add Event&quot; to schedule one.
              </p>
            )}
          </CardContent>
        </Card>
      </div>

      {/* Status Modal */}
      <Dialog open={showStatusModal} onOpenChange={setShowStatusModal}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>
              {statusModalMode === 'add' ? 'Add Status' : 'Edit Status'}
            </DialogTitle>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label>Status</Label>
              <Select
                value={statusForm.jaStatusType.toString()}
                onValueChange={(value) => setStatusForm({ ...statusForm, jaStatusType: parseInt(value) })}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {Object.entries(jaStatusLabels).map(([value, label]) => (
                    <SelectItem key={value} value={value}>
                      {label}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label>Notes (optional)</Label>
              <Textarea
                rows={3}
                value={statusForm.note}
                onChange={(e) => setStatusForm({ ...statusForm, note: e.target.value })}
                placeholder="Add notes about this status change..."
              />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setShowStatusModal(false)}>
              Cancel
            </Button>
            <Button onClick={handleSaveStatus} disabled={addStatusMutation.isPending || editStatusMutation.isPending}>
              {statusModalMode === 'add' ? 'Add Status' : 'Save Changes'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Delete Status Dialog */}
      <AlertDialog open={showDeleteStatusDialog} onOpenChange={setShowDeleteStatusDialog}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Last Status?</AlertDialogTitle>
            <AlertDialogDescription>
              This will remove the most recent status from the history. This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => deleteStatusMutation.mutate()}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>

      {/* Event Modal */}
      <Dialog open={showEventModal} onOpenChange={setShowEventModal}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>
              {eventModalMode === 'add' ? 'Add Event' : 'Edit Event'}
            </DialogTitle>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label>Event Type</Label>
              <Select
                value={eventForm.eventType.toString()}
                onValueChange={(value) => setEventForm({ ...eventForm, eventType: parseInt(value) })}
              >
                <SelectTrigger>
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
              <Label>Date & Time</Label>
              <Input
                type="datetime-local"
                value={eventForm.eventDate}
                onChange={(e) => setEventForm({ ...eventForm, eventDate: e.target.value })}
              />
            </div>
            <div className="space-y-2">
              <Label>Event Name</Label>
              <Input
                value={eventForm.eventName}
                onChange={(e) => setEventForm({ ...eventForm, eventName: e.target.value })}
                placeholder="e.g., Technical interview with engineering team"
              />
            </div>
            <div className="space-y-2">
              <Label>Notes (optional)</Label>
              <Textarea
                rows={2}
                value={eventForm.note}
                onChange={(e) => setEventForm({ ...eventForm, note: e.target.value })}
                placeholder="Any additional notes..."
              />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setShowEventModal(false)}>
              Cancel
            </Button>
            <Button
              onClick={handleSaveEvent}
              disabled={!eventForm.eventDate || addEventMutation.isPending || updateEventMutation.isPending}
            >
              {eventModalMode === 'add' ? 'Add Event' : 'Save Changes'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Delete Event Dialog */}
      <AlertDialog open={showDeleteEventDialog} onOpenChange={setShowDeleteEventDialog}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Event?</AlertDialogTitle>
            <AlertDialogDescription>
              This will remove the scheduled event. This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => deleteEventMutation.mutate()}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </ProtectedLayout>
  );
}
