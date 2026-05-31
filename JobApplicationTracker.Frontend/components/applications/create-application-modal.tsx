'use client';

import { useState } from 'react';
import { useMutation } from '@tanstack/react-query';
import { applicationService } from '@/services/application-service';
import { CreateJobApplicationDto, WorkMode, workModeLabels } from '@/types';
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
    companyName: prefillData?.companyName || '',
    jobTitle: prefillData?.jobTitle || '',
    jobUrl: prefillData?.jobUrl || '',
    location: prefillData?.location || '',
    workMode: prefillData?.workMode ?? WorkMode.Hybrid,
    salaryMin: prefillData?.salaryMin,
    salaryMax: prefillData?.salaryMax,
    notes: prefillData?.notes || '',
    contactName: prefillData?.contactName || '',
    contactEmail: prefillData?.contactEmail || '',
  });

  const createMutation = useMutation({
    mutationFn: (data: CreateJobApplicationDto) => applicationService.create(data),
    onSuccess: () => {
      toast.success('Application added successfully');
      onSuccess();
      resetForm();
    },
    onError: () => {
      toast.error('Failed to create application');
    },
  });

  const resetForm = () => {
    setFormData({
      companyName: '',
      jobTitle: '',
      jobUrl: '',
      location: '',
      workMode: WorkMode.Hybrid,
      salaryMin: undefined,
      salaryMax: undefined,
      notes: '',
      contactName: '',
      contactEmail: '',
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.companyName || !formData.jobTitle) {
      toast.error('Company name and job title are required');
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

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle>Add New Application</DialogTitle>
            <DialogDescription>
              Track a new job application. Fill in the details below.
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            {/* Company & Job Title */}
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="companyName">Company Name *</Label>
                <Input
                  id="companyName"
                  value={formData.companyName}
                  onChange={(e) => handleChange('companyName', e.target.value)}
                  placeholder="e.g., Stripe"
                  className="bg-input"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="jobTitle">Job Title *</Label>
                <Input
                  id="jobTitle"
                  value={formData.jobTitle}
                  onChange={(e) => handleChange('jobTitle', e.target.value)}
                  placeholder="e.g., Senior Frontend Engineer"
                  className="bg-input"
                />
              </div>
            </div>

            {/* Job URL */}
            <div className="space-y-2">
              <Label htmlFor="jobUrl">Job URL</Label>
              <Input
                id="jobUrl"
                type="url"
                value={formData.jobUrl || ''}
                onChange={(e) => handleChange('jobUrl', e.target.value)}
                placeholder="https://..."
                className="bg-input"
              />
            </div>

            {/* Location & Work Mode */}
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="location">Location</Label>
                <Input
                  id="location"
                  value={formData.location || ''}
                  onChange={(e) => handleChange('location', e.target.value)}
                  placeholder="e.g., San Francisco, CA"
                  className="bg-input"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="workMode">Work Mode</Label>
                <Select
                  value={formData.workMode.toString()}
                  onValueChange={(value) => handleChange('workMode', Number(value))}
                >
                  <SelectTrigger className="bg-input">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    {Object.entries(workModeLabels).map(([value, label]) => (
                      <SelectItem key={value} value={value}>
                        {label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
            </div>

            {/* Salary Range */}
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="salaryMin">Min Salary</Label>
                <Input
                  id="salaryMin"
                  type="number"
                  value={formData.salaryMin || ''}
                  onChange={(e) =>
                    handleChange('salaryMin', e.target.value ? Number(e.target.value) : undefined)
                  }
                  placeholder="e.g., 150000"
                  className="bg-input"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="salaryMax">Max Salary</Label>
                <Input
                  id="salaryMax"
                  type="number"
                  value={formData.salaryMax || ''}
                  onChange={(e) =>
                    handleChange('salaryMax', e.target.value ? Number(e.target.value) : undefined)
                  }
                  placeholder="e.g., 200000"
                  className="bg-input"
                />
              </div>
            </div>

            {/* Contact Info */}
            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="contactName">Contact Name</Label>
                <Input
                  id="contactName"
                  value={formData.contactName || ''}
                  onChange={(e) => handleChange('contactName', e.target.value)}
                  placeholder="e.g., John Smith"
                  className="bg-input"
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="contactEmail">Contact Email</Label>
                <Input
                  id="contactEmail"
                  type="email"
                  value={formData.contactEmail || ''}
                  onChange={(e) => handleChange('contactEmail', e.target.value)}
                  placeholder="e.g., recruiter@company.com"
                  className="bg-input"
                />
              </div>
            </div>

            {/* Notes */}
            <div className="space-y-2">
              <Label htmlFor="notes">Notes</Label>
              <Textarea
                id="notes"
                value={formData.notes || ''}
                onChange={(e) => handleChange('notes', e.target.value)}
                placeholder="Any additional notes about this application..."
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
            <Button type="submit" disabled={createMutation.isPending}>
              {createMutation.isPending ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Adding...
                </>
              ) : (
                'Add Application'
              )}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
