'use client';

import { useState, useEffect, use } from 'react';
import { useRouter } from 'next/navigation';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { format, formatDistanceToNow } from 'date-fns';
import { ArrowLeft, Pencil, X, Check, Plus, Trash2, Calendar, Clock } from 'lucide-react';

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

import {
  ApplicationStatus,
  ApplicationEventType,
  applicationStatusLabels,
  applicationStatusColors,
  eventTypeLabels,
  eventTypeColors,
} from '@/types';
import type {
  ApplicationStatusHistoryDto,
  UpdateJobApplicationDto,
} from '@/types';

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
  const [statusForm, setStatusForm] = useState({ status: ApplicationStatus.Applied, notes: '' });
  const [showDeleteStatusDialog, setShowDeleteStatusDialog] = useState(false);

  // Event modal state
  const [showEventModal, setShowEventModal] = useState(false);
  const [eventModalMode, setEventModalMode] = useState<'add' | 'edit'>('add');
  const [eventForm, setEventForm] = useState({
    eventType: ApplicationEventType.Interview,
    scheduledAt: '',
    description: '',
    location: '',
    notes: '',
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
        companyName: application.companyName,
        jobTitle: application.jobTitle,
        jobDescription: application.jobDescription || '',
        notes: application.notes || '',
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
    mutationFn: (data: { newStatus: ApplicationStatus; notes?: string }) =>
      applicationService.updateStatus(applicationId, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['applications'] });
      setShowStatusModal(false);
      setStatusForm({ status: ApplicationStatus.Applied, notes: '' });
    },
  });

  // Edit last status mutation
  const editStatusMutation = useMutation({
    mutationFn: async (data: { id: string; status: ApplicationStatus; notes?: string }) => {
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
        applicationId,
        eventType: data.eventType,
        scheduledAt: new Date(data.scheduledAt).toISOString(),
        description: data.description || undefined,
        location: data.location || undefined,
        notes: data.notes || undefined,
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
      eventService.update(application?.event?.id || '', {
        eventType: data.eventType,
        scheduledAt: new Date(data.scheduledAt).toISOString(),
        description: data.description || undefined,
        location: data.location || undefined,
        notes: data.notes || undefined,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      setShowEventModal(false);
      resetEventForm();
    },
  });

  const deleteEventMutation = useMutation({
    mutationFn: () => eventService.delete(application?.event?.id || ''),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      setShowDeleteEventDialog(false);
    },
  });

  const resetEventForm = () => {
    setEventForm({
      eventType: ApplicationEventType.Interview,
      scheduledAt: '',
      description: '',
      location: '',
      notes: '',
    });
  };

  const handleSaveApplication = () => {
    updateMutation.mutate(editForm);
  };

  const handleDiscardChanges = () => {
    if (application) {
      setEditForm({
        companyName: application.companyName,
        jobTitle: application.jobTitle,
        jobDescription: application.jobDescription || '',
        notes: application.notes || '',
      });
    }
    setIsEditing(false);
  };

  const openAddStatusModal = () => {
    setStatusModalMode('add');
    const nextStatus = application ? application.currentStatus + 1 : ApplicationStatus.Applied;
    setStatusForm({ status: Math.min(nextStatus, ApplicationStatus.Withdrawn), notes: '' });
    setShowStatusModal(true);
  };

  const openEditStatusModal = (status: ApplicationStatusHistoryDto) => {
    setStatusModalMode('edit');
    setEditingStatusId(status.id);
    setStatusForm({ status: status.status, notes: status.notes || '' });
    setShowStatusModal(true);
  };

  const openAddEventModal = () => {
    setEventModalMode('add');
    resetEventForm();
    setShowEventModal(true);
  };

  const openEditEventModal = () => {
    if (application?.event) {
      setEventModalMode('edit');
      const eventDate = new Date(application.event.scheduledAt);
      setEventForm({
        eventType: application.event.eventType,
        scheduledAt: eventDate.toISOString().slice(0, 16),
        description: application.event.description || '',
        location: application.event.location || '',
        notes: application.event.notes || '',
      });
      setShowEventModal(true);
    }
  };

  const handleSaveStatus = () => {
    if (statusModalMode === 'add') {
      addStatusMutation.mutate({ newStatus: statusForm.status, notes: statusForm.notes || undefined });
    } else {
      editStatusMutation.mutate({ id: editingStatusId!, status: statusForm.status, notes: statusForm.notes || undefined });
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
                {isEditing ? 'Edit Application' : application.companyName}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? 'Make changes and save or discard' : application.jobTitle}
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
              <Badge className={applicationStatusColors[application.currentStatus]}>
                {applicationStatusLabels[application.currentStatus]}
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
                    value={editForm.companyName || ''}
                    onChange={(e) => setEditForm({ ...editForm, companyName: e.target.value })}
                  />
                ) : (
                  <p className="text-lg font-medium">{application.companyName}</p>
                )}
              </div>
              <div className="space-y-2">
                <Label>Position</Label>
                {isEditing ? (
                  <Input
                    value={editForm.jobTitle || ''}
                    onChange={(e) => setEditForm({ ...editForm, jobTitle: e.target.value })}
                  />
                ) : (
                  <p className="text-lg font-medium">{application.jobTitle}</p>
                )}
              </div>
            </div>

            {/* Timestamps (always readonly) */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <Label className="text-muted-foreground">Created At</Label>
                <p className="text-sm">
                  {format(new Date(application.createdAt), 'PPP')}
                  <span className="text-muted-foreground ml-2">
                    ({formatDistanceToNow(new Date(application.createdAt), { addSuffix: true })})
                  </span>
                </p>
              </div>
              <div className="space-y-2">
                <Label className="text-muted-foreground">Last Updated</Label>
                <p className="text-sm">
                  {format(new Date(application.updatedAt), 'PPP')}
                  <span className="text-muted-foreground ml-2">
                    ({formatDistanceToNow(new Date(application.updatedAt), { addSuffix: true })})
                  </span>
                </p>
              </div>
            </div>

            {/* Notes */}
            <div className="space-y-2">
              <Label>Notes</Label>
              {isEditing ? (
                <Textarea
                  rows={3}
                  value={editForm.notes || ''}
                  onChange={(e) => setEditForm({ ...editForm, notes: e.target.value })}
                  placeholder="Add notes about this application..."
                />
              ) : (
                <p className="text-sm text-muted-foreground whitespace-pre-wrap">
                  {application.notes || 'No notes'}
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
                  {application.jobDescription || 'No job description'}
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
            <div className="space-y-3">
              {application.statusHistory.map((status, index) => {
                const isLast = index === application.statusHistory.length - 1;
                return (
                  <div
                    key={status.id}
                    className="flex items-start justify-between p-4 border border-border rounded-lg"
                  >
                    <div className="flex items-start gap-4">
                      <div className="flex flex-col items-center">
                        <div className={`w-3 h-3 rounded-full ${
                          status.status === ApplicationStatus.Rejected ? 'bg-red-500' :
                          status.status === ApplicationStatus.Accepted ? 'bg-green-500' :
                          status.status === ApplicationStatus.OfferReceived ? 'bg-emerald-500' :
                          'bg-primary'
                        }`} />
                        {index < application.statusHistory.length - 1 && (
                          <div className="w-px h-full min-h-[24px] bg-border mt-1" />
                        )}
                      </div>
                      <div>
                        <Badge className={applicationStatusColors[status.status]}>
                          {applicationStatusLabels[status.status]}
                        </Badge>
                        <p className="text-sm text-muted-foreground mt-1">
                          {format(new Date(status.changedAt), 'PPP p')}
                        </p>
                        {status.notes && (
                          <p className="text-sm mt-2 text-foreground">{status.notes}</p>
                        )}
                      </div>
                    </div>
                    {isLast && (
                      <div className="flex items-center gap-1">
                        <Button variant="ghost" size="sm" onClick={() => openEditStatusModal(status)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        {canDeleteStatus && (
                          <Button
                            variant="ghost"
                            size="sm"
                            className="text-destructive hover:text-destructive"
                            onClick={() => setShowDeleteStatusDialog(true)}
                          >
                            <Trash2 className="h-4 w-4" />
                          </Button>
                        )}
                      </div>
                    )}
                  </div>
                );
              })}
            </div>
          </CardContent>
        </Card>

        {/* Event Card (freely editable, single event) */}
        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle>Scheduled Event</CardTitle>
            {!application.event && (
              <Button size="sm" onClick={openAddEventModal}>
                <Plus className="h-4 w-4 mr-2" />
                Add Event
              </Button>
            )}
          </CardHeader>
          <CardContent>
            {application.event ? (
              <div className="p-4 border border-border rounded-lg">
                <div className="flex items-start justify-between">
                  <div className="space-y-3">
                    <div className="flex items-center gap-3">
                      <Badge className={eventTypeColors[application.event.eventType]}>
                        {eventTypeLabels[application.event.eventType]}
                      </Badge>
                      {application.event.isCompleted && (
                        <Badge variant="outline" className="text-green-600 border-green-600">
                          Completed
                        </Badge>
                      )}
                    </div>
                    <div className="flex items-center gap-4 text-sm text-muted-foreground">
                      <span className="flex items-center gap-1">
                        <Calendar className="h-4 w-4" />
                        {format(new Date(application.event.scheduledAt), 'PPP')}
                      </span>
                      <span className="flex items-center gap-1">
                        <Clock className="h-4 w-4" />
                        {format(new Date(application.event.scheduledAt), 'p')}
                      </span>
                    </div>
                    {application.event.description && (
                      <p className="text-sm">{application.event.description}</p>
                    )}
                    {application.event.location && (
                      <p className="text-sm text-muted-foreground">Location: {application.event.location}</p>
                    )}
                    {application.event.notes && (
                      <p className="text-sm text-muted-foreground italic">{application.event.notes}</p>
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
                value={statusForm.status.toString()}
                onValueChange={(value) => setStatusForm({ ...statusForm, status: parseInt(value) })}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {Object.entries(applicationStatusLabels).map(([value, label]) => (
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
                value={statusForm.notes}
                onChange={(e) => setStatusForm({ ...statusForm, notes: e.target.value })}
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
                value={eventForm.scheduledAt}
                onChange={(e) => setEventForm({ ...eventForm, scheduledAt: e.target.value })}
              />
            </div>
            <div className="space-y-2">
              <Label>Description (optional)</Label>
              <Input
                value={eventForm.description}
                onChange={(e) => setEventForm({ ...eventForm, description: e.target.value })}
                placeholder="e.g., Technical interview with engineering team"
              />
            </div>
            <div className="space-y-2">
              <Label>Location (optional)</Label>
              <Input
                value={eventForm.location}
                onChange={(e) => setEventForm({ ...eventForm, location: e.target.value })}
                placeholder="e.g., Zoom, Google Meet, Office Address"
              />
            </div>
            <div className="space-y-2">
              <Label>Notes (optional)</Label>
              <Textarea
                rows={2}
                value={eventForm.notes}
                onChange={(e) => setEventForm({ ...eventForm, notes: e.target.value })}
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
              disabled={!eventForm.scheduledAt || addEventMutation.isPending || updateEventMutation.isPending}
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
