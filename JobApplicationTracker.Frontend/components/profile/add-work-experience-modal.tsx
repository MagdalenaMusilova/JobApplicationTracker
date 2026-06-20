'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { profileService } from '@/services/profile-service';
import { WorkExperienceDto } from '@/types/User/Resume/UserResumeDto';
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

interface AddWorkExperienceModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  resumeId: string;
}

export function AddWorkExperienceModal({
  open,
  onOpenChange,
  resumeId,
}: AddWorkExperienceModalProps) {
  const [formData, setFormData] = useState<WorkExperienceDto>({
    company: '',
    position: '',
    startDate: '',
    endDate: '',
    jobDescription: [],
    skills: [],
    notes: '',
  });
  const [currentDescription, setCurrentDescription] = useState('');

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: WorkExperienceDto) =>
      profileService.addWorkExperience(resumeId, data),
    onSuccess: () => {
      toast.success('Work experience added successfully');
      queryClient.invalidateQueries({ queryKey: ['account'] });
      onOpenChange(false);
      resetForm();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to add work experience';
      toast.error(errorMessage);
    },
  });

  const resetForm = () => {
    setFormData({
      company: '',
      position: '',
      startDate: '',
      endDate: '',
      jobDescription: [],
      skills: [],
      notes: '',
    });
    setCurrentDescription('');
  };

  const handleAddDescription = () => {
    if (currentDescription.trim()) {
      setFormData((prev) => ({
        ...prev,
        jobDescription: [...prev.jobDescription, currentDescription.trim()],
      }));
      setCurrentDescription('');
    }
  };

  const handleRemoveDescription = (index: number) => {
    setFormData((prev) => ({
      ...prev,
      jobDescription: prev.jobDescription.filter((_, i) => i !== index),
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.company?.trim() || !formData.position?.trim()) {
      toast.error('Company and position are required');
      return;
    }

    createMutation.mutate(formData);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add Work Experience</DialogTitle>
            <DialogDescription>
              Add your professional work experience
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="company">Company *</Label>
                <Input
                  id="company"
                  value={formData.company || ''}
                  onChange={(e) =>
                    setFormData((prev) => ({ ...prev, company: e.target.value }))
                  }
                  placeholder="e.g., Google"
                  className="bg-input"
                  required
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="position">Position *</Label>
                <Input
                  id="position"
                  value={formData.position || ''}
                  onChange={(e) =>
                    setFormData((prev) => ({ ...prev, position: e.target.value }))
                  }
                  placeholder="e.g., Software Engineer"
                  className="bg-input"
                  required
                />
              </div>
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
              <Label>Job Description</Label>
              <div className="flex gap-2">
                <Input
                  value={currentDescription}
                  onChange={(e) => setCurrentDescription(e.target.value)}
                  placeholder="Add a bullet point..."
                  className="bg-input"
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      handleAddDescription();
                    }
                  }}
                />
                <Button
                  type="button"
                  variant="outline"
                  size="icon"
                  onClick={handleAddDescription}
                >
                  <Plus className="h-4 w-4" />
                </Button>
              </div>
              {formData.jobDescription.length > 0 && (
                <ul className="mt-2 space-y-1">
                  {formData.jobDescription.map((desc, index) => (
                    <li
                      key={index}
                      className="flex items-start gap-2 text-sm bg-muted p-2 rounded"
                    >
                      <span className="flex-1">• {desc}</span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="h-6 w-6"
                        onClick={() => handleRemoveDescription(index)}
                      >
                        <X className="h-3 w-3" />
                      </Button>
                    </li>
                  ))}
                </ul>
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
                'Add Experience'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
