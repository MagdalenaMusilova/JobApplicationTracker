'use client';

import { useRouter } from 'next/navigation';
import { format, formatDistanceToNow } from 'date-fns';
import { useMutation } from '@tanstack/react-query';
import { applicationService } from '@/services/application-service';
import { JobApplicationMinimalDto } from '@/types/JAObjects/JobApplications/JobApplicationMinimalDto'
import { JAStatusType, applicationStatusColors, jaStatusLabels  } from '@/types/Enums/JAStatusType'
import { eventTypeLabels } from '@/types/Enums/JAEventType'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { Trash2, XCircle, Building2, Calendar } from 'lucide-react';
import { cn } from '@/lib/utils';
import { toast } from 'sonner';

interface ApplicationsTableProps {
  applications: JobApplicationMinimalDto[];
  onRefresh: () => void;
}

export function ApplicationsTable({
                                    applications,
                                    onRefresh,
                                  }: ApplicationsTableProps) {
  const router = useRouter();

  console.log('applications', applications);
  console.log('first status', applications[0]?.jaStatus);
  console.log('enum', JAStatusType);


  const deleteMutation = useMutation({
    mutationFn: (id: string) => applicationService.delete(id),
    onSuccess: () => {
      toast.success('Application deleted');
      onRefresh();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to delete application';
      toast.error(errorMessage);
    },
  });

  const updateStatusMutation = useMutation({
    mutationFn: ({ id, status }: { id: string; status: JAStatusType }) =>
        applicationService.updateStatus({
          jobApplicationId: id,
          statusType: status,
          note: ''
        }),
    onSuccess: () => {
      toast.success('Application marked as rejected');
      onRefresh();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to update application status';
      toast.error(errorMessage);
    },
  });

  const handleDelete = (e: React.MouseEvent, id: string) => {
    e.stopPropagation();
    deleteMutation.mutate(id);
  };

  const handleMarkRejected = (e: React.MouseEvent, id: string) => {
    e.stopPropagation();
    updateStatusMutation.mutate({
      id,
      status: JAStatusType.Rejected,
    });
  };

  const handleRowClick = (id: string) => {
    router.push(`/applications/${id}`);
  };

  if (applications.length === 0) {
    return (
        <div className="flex flex-col items-center justify-center py-16 text-center border border-border rounded-lg bg-card">
          <Building2 className="h-12 w-12 text-muted-foreground/50 mb-4" />
          <h3 className="text-lg font-medium text-foreground">
            No applications found
          </h3>
          <p className="text-sm text-muted-foreground mt-1">
            Try adjusting your filters or add a new application
          </p>
        </div>
    );
  }

  return (
      <div className="space-y-4">
        <div className="border border-border rounded-lg overflow-hidden bg-card">
          <Table>
            <TableHeader>
              <TableRow className="hover:bg-transparent">
                <TableHead className="w-[180px]">Company</TableHead>
                <TableHead>Position</TableHead>
                <TableHead>Status</TableHead>
                <TableHead>Event</TableHead>
                <TableHead>Last Updated</TableHead>
                <TableHead className="w-[120px] text-right">
                  Actions
                </TableHead>
              </TableRow>
            </TableHeader>

            <TableBody>
              {applications.map((app) => (
                  <TableRow
                      key={app.jaId}
                      className="group cursor-pointer hover:bg-accent/50"
                      onClick={() => handleRowClick(app.jaId)}
                  >
                    <TableCell>
                      <div className="flex items-center gap-3">
                        <div className="w-8 h-8 rounded-lg bg-primary/10 flex items-center justify-center flex-shrink-0">
                      <span className="text-xs font-semibold text-primary">
                        {app.company.charAt(0).toUpperCase()}
                      </span>
                        </div>

                        <span className="font-medium text-foreground">
                      {app.company}
                    </span>
                      </div>
                    </TableCell>

                    <TableCell>
                      <p className="text-foreground">{app.position}</p>
                    </TableCell>

                    <TableCell>
                      <Badge
                          variant="outline"
                          className={cn(
                              'text-xs',
                              applicationStatusColors[app.jaStatus]
                          )}
                      >
                        {jaStatusLabels[app.jaStatus]}
                      </Badge>
                    </TableCell>

                    <TableCell>
                      {app.eventDate && app.eventType !== undefined ? (
                          <div className="flex items-center gap-2 text-sm">
                            <Calendar className="h-3.5 w-3.5 text-muted-foreground" />
                            <div>
                              <p className="text-foreground">
                                {eventTypeLabels[app.eventType]}
                              </p>
                              <p className="text-xs text-muted-foreground">
                                {format(
                                    new Date(app.eventDate),
                                    'MMM d, h:mm a'
                                )}
                              </p>
                            </div>
                          </div>
                      ) : (
                          <span className="text-muted-foreground text-sm">-</span>
                      )}
                    </TableCell>

                    <TableCell>
                      <div className="text-sm">
                        <p className="text-foreground">
                          {formatDistanceToNow(new Date(app.updatedAt), {
                            addSuffix: true,
                          })}
                        </p>
                        <p className="text-xs text-muted-foreground">
                          {format(
                              new Date(app.updatedAt),
                              'MMM d, yyyy'
                          )}
                        </p>
                      </div>
                    </TableCell>

                    <TableCell>
                      <div
                          className="flex items-center justify-end gap-1"
                          onClick={(e) => e.stopPropagation()}
                      >
                        {app.jaStatus !== JAStatusType.Rejected && (
                            <AlertDialog>
                              <AlertDialogTrigger asChild>
                                <Button
                                    variant="ghost"
                                    size="icon"
                                    className="h-8 w-8 text-yellow-500 hover:text-yellow-500 hover:bg-yellow-500/10"
                                    disabled={updateStatusMutation.isPending}
                                >
                                  <XCircle className="h-4 w-4" />
                                </Button>
                              </AlertDialogTrigger>

                              <AlertDialogContent
                                  onClick={(e) => e.stopPropagation()}
                              >
                                <AlertDialogHeader>
                                  <AlertDialogTitle>
                                    Mark as Rejected?
                                  </AlertDialogTitle>
                                  <AlertDialogDescription>
                                    This will mark your application at{' '}
                                    {app.company} for {app.position} as rejected.
                                  </AlertDialogDescription>
                                </AlertDialogHeader>

                                <AlertDialogFooter>
                                  <AlertDialogCancel>
                                    Cancel
                                  </AlertDialogCancel>

                                  <AlertDialogAction
                                      onClick={(e) =>
                                          handleMarkRejected(e, app.jaId)
                                      }
                                      className="bg-yellow-500 text-white hover:bg-yellow-600"
                                  >
                                    Mark Rejected
                                  </AlertDialogAction>
                                </AlertDialogFooter>
                              </AlertDialogContent>
                            </AlertDialog>
                        )}

                        <AlertDialog>
                          <AlertDialogTrigger asChild>
                            <Button
                                variant="ghost"
                                size="icon"
                                className="h-8 w-8 text-destructive hover:text-destructive hover:bg-destructive/10"
                                disabled={deleteMutation.isPending}
                            >
                              <Trash2 className="h-4 w-4" />
                            </Button>
                          </AlertDialogTrigger>

                          <AlertDialogContent
                              onClick={(e) => e.stopPropagation()}
                          >
                            <AlertDialogHeader>
                              <AlertDialogTitle>
                                Delete Application?
                              </AlertDialogTitle>
                              <AlertDialogDescription>
                                This will permanently delete your application at{' '}
                                {app.company}. This action cannot be undone.
                              </AlertDialogDescription>
                            </AlertDialogHeader>

                            <AlertDialogFooter>
                              <AlertDialogCancel>
                                Cancel
                              </AlertDialogCancel>

                              <AlertDialogAction
                                  onClick={(e) => handleDelete(e, app.jaId)}
                                  className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                              >
                                Delete
                              </AlertDialogAction>
                            </AlertDialogFooter>
                          </AlertDialogContent>
                        </AlertDialog>
                      </div>
                    </TableCell>
                  </TableRow>
              ))}
            </TableBody>
          </Table>
        </div>

        <p className="text-sm text-muted-foreground">
          {applications.length} application
          {applications.length !== 1 ? 's' : ''}
        </p>
      </div>
  );
}