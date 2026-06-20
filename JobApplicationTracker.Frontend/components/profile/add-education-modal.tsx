'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { profileService } from '@/services/profile-service';
import { EducationDto } from '@/types/User/Resume/UserResumeDto';
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

interface AddEducationModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  resumeId: string;
}

export function AddEducationModal({
  open,
  onOpenChange,
  resumeId,
}: AddEducationModalProps) {
  const [formData, setFormData] = useState<EducationDto>({
    school: '',
    degree: '',
    majors: [],
    isFinished: false,
    skills: [],
    notes: '',
  });
  const [currentMajor, setCurrentMajor] = useState('');

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: EducationDto) =>
      profileService.addEducation(resumeId, data),
    onSuccess: () => {
      toast.success('Education added successfully');
      queryClient.invalidateQueries({ queryKey: ['account'] });
      onOpenChange(false);
      resetForm();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to add education';
      toast.error(errorMessage);
    },
  });

  const resetForm = () => {
    setFormData({
      school: '',
      degree: '',
      majors: [],
      isFinished: false,
      skills: [],
      notes: '',
    });
    setCurrentMajor('');
  };

  const handleAddMajor = () => {
    if (currentMajor.trim()) {
      setFormData((prev) => ({
        ...prev,
        majors: [...prev.majors, currentMajor.trim()],
      }));
      setCurrentMajor('');
    }
  };

  const handleRemoveMajor = (index: number) => {
    setFormData((prev) => ({
      ...prev,
      majors: prev.majors.filter((_, i) => i !== index),
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.school?.trim() || !formData.degree?.trim()) {
      toast.error('School and degree are required');
      return;
    }

    createMutation.mutate(formData);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add Education</DialogTitle>
            <DialogDescription>
              Add your educational background
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="school">School *</Label>
              <Input
                id="school"
                value={formData.school || ''}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, school: e.target.value }))
                }
                placeholder="e.g., MIT"
                className="bg-input"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="degree">Degree *</Label>
              <Input
                id="degree"
                value={formData.degree || ''}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, degree: e.target.value }))
                }
                placeholder="e.g., Bachelor of Science"
                className="bg-input"
                required
              />
            </div>

            <div className="space-y-2">
              <Label>Major(s)</Label>
              <div className="flex gap-2">
                <Input
                  value={currentMajor}
                  onChange={(e) => setCurrentMajor(e.target.value)}
                  placeholder="Add a major..."
                  className="bg-input"
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      handleAddMajor();
                    }
                  }}
                />
                <Button
                  type="button"
                  variant="outline"
                  size="icon"
                  onClick={handleAddMajor}
                >
                  <Plus className="h-4 w-4" />
                </Button>
              </div>
              {formData.majors.length > 0 && (
                <div className="flex flex-wrap gap-2 mt-2">
                  {formData.majors.map((major, index) => (
                    <div
                      key={index}
                      className="flex items-center gap-1 bg-muted px-2 py-1 rounded text-sm"
                    >
                      <span>{major}</span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="h-4 w-4 p-0"
                        onClick={() => handleRemoveMajor(index)}
                      >
                        <X className="h-3 w-3" />
                      </Button>
                    </div>
                  ))}
                </div>
              )}
            </div>

            <div className="flex items-center space-x-2">
              <input
                id="isFinished"
                type="checkbox"
                checked={formData.isFinished}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, isFinished: e.target.checked }))
                }
              />
              <Label htmlFor="isFinished">Completed</Label>
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
                'Add Education'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
