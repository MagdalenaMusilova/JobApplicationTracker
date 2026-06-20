'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { profileService } from '@/services/profile-service';
import { JobSkillDto } from '@/types/User/Resume/UserResumeDto';
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

interface AddSkillModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  resumeId: string;
}

export function AddSkillModal({
  open,
  onOpenChange,
  resumeId,
}: AddSkillModalProps) {
  const [formData, setFormData] = useState<JobSkillDto>({
    name: '',
    aliases: [],
    skills: [],
    notes: '',
  });
  const [currentAlias, setCurrentAlias] = useState('');

  const queryClient = useQueryClient();

  const createMutation = useMutation({
    mutationFn: (data: JobSkillDto) =>
      profileService.addSkill(resumeId, data),
    onSuccess: () => {
      toast.success('Skill added successfully');
      queryClient.invalidateQueries({ queryKey: ['account'] });
      onOpenChange(false);
      resetForm();
    },
    onError: (error: any) => {
      const errorMessage = error?.response?.data?.error || 'Failed to add skill';
      toast.error(errorMessage);
    },
  });

  const resetForm = () => {
    setFormData({
      name: '',
      aliases: [],
      skills: [],
      notes: '',
    });
    setCurrentAlias('');
  };

  const handleAddAlias = () => {
    if (currentAlias.trim()) {
      setFormData((prev) => ({
        ...prev,
        aliases: [...(prev.aliases || []), currentAlias.trim()],
      }));
      setCurrentAlias('');
    }
  };

  const handleRemoveAlias = (index: number) => {
    setFormData((prev) => ({
      ...prev,
      aliases: prev.aliases?.filter((_, i) => i !== index) || [],
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!formData.name?.trim()) {
      toast.error('Skill name is required');
      return;
    }

    createMutation.mutate(formData);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add Skill</DialogTitle>
            <DialogDescription>
              Add a technical or professional skill to your profile
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="name">Skill Name *</Label>
              <Input
                id="name"
                value={formData.name || ''}
                onChange={(e) =>
                  setFormData((prev) => ({ ...prev, name: e.target.value }))
                }
                placeholder="e.g., JavaScript, React, Python"
                className="bg-input"
                required
              />
            </div>

            <div className="space-y-2">
              <Label>Aliases</Label>
              <div className="flex gap-2">
                <Input
                  value={currentAlias}
                  onChange={(e) => setCurrentAlias(e.target.value)}
                  placeholder="Add an alias..."
                  className="bg-input"
                  onKeyDown={(e) => {
                    if (e.key === 'Enter') {
                      e.preventDefault();
                      handleAddAlias();
                    }
                  }}
                />
                <Button
                  type="button"
                  variant="outline"
                  size="icon"
                  onClick={handleAddAlias}
                >
                  <Plus className="h-4 w-4" />
                </Button>
              </div>
              {formData.aliases && formData.aliases.length > 0 && (
                <div className="flex flex-wrap gap-2 mt-2">
                  {formData.aliases.map((alias, index) => (
                    <div
                      key={index}
                      className="flex items-center gap-1 bg-muted px-2 py-1 rounded text-sm"
                    >
                      <span>{alias}</span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="h-4 w-4 p-0"
                        onClick={() => handleRemoveAlias(index)}
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
                placeholder="Any additional notes about this skill..."
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
                'Add Skill'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
