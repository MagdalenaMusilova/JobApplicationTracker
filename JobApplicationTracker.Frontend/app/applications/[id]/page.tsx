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
import { CreateEventModal } from '@/components/applications/create-event-modal';
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
  DialogDescription,
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
  const [currentStatusForModal, setCurrentStatusForModal] = useState<JAStatusType | null>(null);
  const [editingStatusType, setEditingStatusType] = useState<JAStatusType | null>(null);

  // Event modal state
  const [showEventModal, setShowEventModal] = useState(false);
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
  const deleteEventMutation = useMutation({
    mutationFn: () => eventService.delete(lastStatus?.jaEvent?.id || ''),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      setShowDeleteEventDialog(false);
    },
  });

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

  const isStatusAllowed = (currentStatus: JAStatusType, newStatus: JAStatusType): boolean => {
    // Task and Interview can go back and forth
    if (
      (currentStatus === JAStatusType.Task && newStatus === JAStatusType.Interview) ||
      (currentStatus === JAStatusType.Interview && newStatus === JAStatusType.Task)
    ) {
      return true;
    }

    // Offer can repeat
    if (currentStatus === JAStatusType.Offer && newStatus === JAStatusType.Offer) {
      return true;
    }

    // Otherwise, only allow strictly higher statuses
    return newStatus > currentStatus;
  };

  const getLowestAllowedStatus = (currentStatus: JAStatusType): JAStatusType => {
    const allowedStatuses = Object.keys(jaStatusLabels)
      .map(Number)
      .filter((status) => isStatusAllowed(currentStatus, status as JAStatusType))
      .sort((a, b) => a - b);

    return (allowedStatuses[0] as JAStatusType) || currentStatus;
  };

  const openAddStatusModal = () => {
    setStatusModalMode('add');
    const sortedHistory = [...(application?.statusHistory || [])].sort((a, b) => a.orderIndex - b.orderIndex);
    const currentStatus = sortedHistory[sortedHistory.length - 1]?.jaStatusType ?? JAStatusType.Applied;
    setCurrentStatusForModal(currentStatus);
    const lowestAllowed = getLowestAllowedStatus(currentStatus);
    setStatusForm({ jaStatusType: lowestAllowed, note: '' });
    setShowStatusModal(true);
  };

  const openEditStatusModal = (status: JAStatusEntryDto) => {
    setStatusModalMode('edit');
    setEditingStatusId(status.id);

    // Find the previous status to determine allowed changes
    const sortedHistory = [...(application?.statusHistory || [])].sort((a, b) => a.orderIndex - b.orderIndex);
    const currentIndex = sortedHistory.findIndex(s => s.id === status.id);
    const previousStatus = currentIndex > 0 ? sortedHistory[currentIndex - 1]?.jaStatusType : JAStatusType.Wishlist;

    setCurrentStatusForModal(previousStatus);
    setEditingStatusType(status.jaStatusType); // Store the status being edited
    // Set the form with the current status type as default
    setStatusForm({ jaStatusType: status.jaStatusType, note: status.note || '' });
    setShowStatusModal(true);
  };

  const handleSaveStatus = () => {
    if (statusModalMode === 'add') {
      addStatusMutation.mutate({ statusType: statusForm.jaStatusType, note: statusForm.note || undefined });
    } else {
      editStatusMutation.mutate({ id: editingStatusId!, jaStatusType: statusForm.jaStatusType, note: statusForm.note || undefined });
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

  const sortedStatusHistoryAsc = [...application.statusHistory].sort(
      (a, b) => a.orderIndex - b.orderIndex
  );
  const sortedStatusHistory = [...sortedStatusHistoryAsc].reverse(); // Newest first for display
  const lastStatus = sortedStatusHistoryAsc[sortedStatusHistoryAsc.length - 1];
  const canDeleteStatus = sortedStatusHistoryAsc.length > 1;

  // Check if there are any available statuses to add
  const hasAvailableStatuses = Object.keys(jaStatusLabels)
    .map(Number)
    .some((status) => isStatusAllowed(lastStatus.jaStatusType, status as JAStatusType));

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
                  {sortedStatusHistoryAsc.length > 0 ? (
                    <>
                      {format(new Date(sortedStatusHistoryAsc[0].createdAt), 'PPP')}
                      <span className="text-muted-foreground ml-2">
                        ({formatDistanceToNow(new Date(sortedStatusHistoryAsc[0].createdAt), { addSuffix: true })})
                      </span>
                    </>
                  ) : 'N/A'}
                </p>
              </div>
              <div className="space-y-2">
                <Label className="text-muted-foreground">Last Updated</Label>
                <p className="text-sm">
                  {sortedStatusHistoryAsc.length > 0 ? (
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
            <Button size="sm" onClick={openAddStatusModal} disabled={!hasAvailableStatuses}>
              <Plus className="h-4 w-4 mr-2" />
              Add Status
            </Button>
          </CardHeader>
          <CardContent>
            <ol className="relative">
              {sortedStatusHistory.map((status, index) => {
                const isFirst = index === 0;
                const isLast = index === sortedStatusHistory.length - 1;
                const isCurrent = isFirst; // First in reversed array = most recent
                const isRejected = status.jaStatusType >= JAStatusType.Rejected;
                const hasEvent = status.jaEvent !== null;
                const eventInPast = hasEvent && status.jaEvent && new Date(status.jaEvent.eventDate) < new Date();

                let iconColor = 'bg-muted text-muted-foreground';
                let icon = <span className="h-2 w-2 rounded-full bg-current" />;

                if (isCurrent) {
                  if (isRejected) {
                    iconColor = 'bg-destructive text-destructive-foreground';
                    icon = <X className="h-3.5 w-3.5" />;
                  } else if (hasEvent && !eventInPast) {
                    iconColor = 'bg-amber-500 text-white';
                    icon = <Clock className="h-3.5 w-3.5" />;
                  } else {
                    // Use default primary color for both: no event, or event in past
                    iconColor = 'bg-primary text-primary-foreground';
                    icon = <Check className="h-3.5 w-3.5" />;
                  }
                }

                return (
                    <li
                        key={status.id}
                        className="relative flex gap-4 pb-6 last:pb-0 group"
                    >
                      {/* Connecting line */}
                      {index < sortedStatusHistory.length - 1 && (
                          <span
                              className="absolute left-[11px] top-7 bottom-0 w-px bg-border"
                              aria-hidden="true"
                          />
                      )}

                      {/* Timeline marker */}
                      <div className="relative z-10 flex-shrink-0">
                        <span className={`flex h-6 w-6 items-center justify-center rounded-full ring-4 ring-background ${iconColor}`}>
                          {icon}
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
                          {isFirst && (
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
              <Button size="sm" onClick={() => setShowEventModal(true)}>
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
            <DialogDescription>
              {statusModalMode === 'add' ? 'Add a new status update to this application.' : 'Edit the most recent status update.'}
            </DialogDescription>
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
                  {Object.entries(jaStatusLabels)
                    .filter(([value]) => {
                      const statusValue = Number(value) as JAStatusType;
                      // When editing, always include the current status being edited
                      if (statusModalMode === 'edit' && editingStatusType !== null && statusValue === editingStatusType) {
                        return true;
                      }
                      // Otherwise filter normally
                      return currentStatusForModal !== null && isStatusAllowed(currentStatusForModal, statusValue);
                    })
                    .map(([value, label]) => (
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
      <CreateEventModal
        open={showEventModal}
        onOpenChange={setShowEventModal}
        applicationId={applicationId}
      />

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
