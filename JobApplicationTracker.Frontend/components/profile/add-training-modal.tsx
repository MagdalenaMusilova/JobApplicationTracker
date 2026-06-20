'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { profileService } from '@/services/profile-service';
import { TrainingDto } from '@/types/User/Resume/UserResumeDto';
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
import { Loader2, Plus, X } from 'lucide-react';
import { toast } from 'sonner';

interface AddTrainingModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  resumeId: string;
}

export function AddTrainingModal({
  open,
  onOpenChange,
  resumeId,
}: AddTrainingModalProps) {
  const [formData, setFormData] = useState<TrainingDto>({
    name: '',
    type: '',
    startDate: '',
    endDate: '',
    certification: [],
    skills: [],
    notes: '',
  });
  const [currentCertification, setCurrentCertification] = useState('');

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: TrainingDto) =>
      profileService.addTraining(resumeId, data),
    onSuccess: () => {
      toast.success('Training added successfully');
      queryClient.invalidateQueries({ queryKey: ['account'] });
      onOpenChange(false);
      resetForm();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to add training';
      toast.error(errorMessage);
    },
  });

  const resetForm = () => {
    setFormData({
      name: '',
      type: '',
      startDate: '',
      endDate: '',
      certification: [],
      skills: [],
      notes: '',
    });
    setCurrentCertification('');
  };

  const handleAddCertification = () => {
    if (currentCertification.trim()) {
      setFormData((prev) => ({
        ...prev,
        certification: [...(prev.certification || []), currentCertification.trim()],
      }));
      setCurrentCertification('');
    }
  };

  const handleRemoveCertification = (index: number) => {
    setFormData((prev) => ({
      ...prev,
      certification: prev.certification?.filter((_, i) => i !== index) || [],
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.name?.trim()) {
      toast.error('Training name is required');
      return;
    }

    createMutation.mutate(formData);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add Training</DialogTitle>
            <DialogDescription>
              Add certifications, courses, or training programs
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="name">Training Name *</Label>
              <Input
                id="name"
                value={formData.name || ''}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, name: e.target.value }))
                }
                placeholder="e.g., AWS Certified Solutions Architect"
                className="bg-input"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="type">Type</Label>
              <Input
                id="type"
                value={formData.type || ''}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, type: e.target.value }))
                }
                placeholder="e.g., Certification, Course, Workshop"
                className="bg-input"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="startDate">Start Date</Label>
                <Input
                  id="startDate"
                  type="date"
                  value={formData.startDate || ''}
                  onChange={(e) =>
                    setFormData((prev) => ({ ...prev, startDate: e.target.value }))
                  }
                  className="bg-input"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="endDate">End Date</Label>
                <Input
                  id="endDate"
                  type="date"
                  value={formData.endDate || ''}
                  onChange={(e) =>
                    setFormData((prev) => ({ ...prev, endDate: e.target.value }))
                  }
                  className="bg-input"
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label>Certifications</Label>
              <div className="flex gap-2">
                <Input
                  value={currentCertification}
                  onChange={(e) => setCurrentCertification(e.target.value)}
                  placeholder="Add a certification..."
                  className="bg-input"
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      handleAddCertification();
                    }
                  }}
                />
                <Button
                  type="button"
                  variant="outline"
                  size="icon"
                  onClick={handleAddCertification}
                >
                  <Plus className="h-4 w-4" />
                </Button>
              </div>
              {formData.certification && formData.certification.length > 0 && (
                <div className="flex flex-wrap gap-2 mt-2">
                  {formData.certification.map((cert, index) => (
                    <div
                      key={index}
                      className="flex items-center gap-1 bg-muted px-2 py-1 rounded text-sm"
                    >
                      <span>{cert}</span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="h-4 w-4 p-0"
                        onClick={() => handleRemoveCertification(index)}
                      >
                        <X className="h-3 w-3" />
                      </Button>
                    </div>
                  ))}
                </div>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="notes">Notes</Label>
              <Textarea
                id="notes"
                value={formData.notes || ''}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, notes: e.target.value }))
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
                  Adding...
                </>
              ) : (
                'Add Training'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
