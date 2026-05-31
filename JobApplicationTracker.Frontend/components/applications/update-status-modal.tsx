'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { applicationService } from '@/services/application-service';
import {
  ApplicationStatus,
  applicationStatusLabels,
  UpdateStatusDto,
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

interface UpdateStatusModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  applicationId: string;
  currentStatus: ApplicationStatus;
}

export function UpdateStatusModal({
  open,
  onOpenChange,
  applicationId,
  currentStatus,
}: UpdateStatusModalProps) {
  const [newStatus, setNewStatus] = useState<ApplicationStatus>(currentStatus);
  const [notes, setNotes] = useState('');
  const queryClient = useQueryClient();

  const updateMutation = useMutation({
    mutationFn: (data: UpdateStatusDto) =>
      applicationService.updateStatus(applicationId, data),
    onSuccess: () => {
      toast.success('Status updated successfully');
      queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
      queryClient.invalidateQueries({ queryKey: ['applications'] });
      queryClient.invalidateQueries({ queryKey: ['dashboard'] });
      onOpenChange(false);
      setNotes('');
    },
    onError: () => {
      toast.error('Failed to update status');
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (newStatus === currentStatus) {
      toast.error('Please select a different status');
      return;
    }

    updateMutation.mutate({
      newStatus,
      notes: notes || undefined,
    });
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Update Application Status</DialogTitle>
            <DialogDescription>
              Change the status of this application and add optional notes.
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="status">New Status</Label>
              <Select
                value={newStatus.toString()}
                onValueChange={(value) => setNewStatus(Number(value))}
              >
                <SelectTrigger className="bg-input">
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
              <Label htmlFor="notes">Notes (optional)</Label>
              <Textarea
                id="notes"
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                placeholder="Add any notes about this status change..."
                rows={3}
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
            <Button type="submit" disabled={updateMutation.isPending}>
              {updateMutation.isPending ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Updating...
                </>
              ) : (
                'Update Status'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
