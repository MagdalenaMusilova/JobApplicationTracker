'use client';

import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Badge } from '@/components/ui/badge';
import { Progress } from '@/components/ui/progress';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import {
  Sparkles,
  Target,
  Loader2,
} from 'lucide-react';
import { applicationService } from '@/services/application-service';
import { matchService } from '@/services/match-service';
import { WorkMode, workModeLabels } from '@/types/Enums/WorkMode';
import type { CreateJobApplicationDto } from '@/types/JAObjects/JobApplications/CreateJobApplicationDto';
import { useRouter } from 'next/navigation';

export default function MatchPage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [jobDescription, setJobDescription] = useState('');
  const [matchResult, setMatchResult] = useState<string | null>(null);
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

  const handleAnalyze = async () => {
    if (!jobDescription.trim()) return;
    
    setIsAnalyzing(true);
    try {
      const result = await matchService.analyzeMatch(jobDescription);
      setMatchResult(result);
    } catch (error) {
      console.error('Failed to analyze match:', error);
    } finally {
      setIsAnalyzing(false);
    }
  };

  const handleClear = () => {
    setJobDescription('');
    setMatchResult(null);
  };

  return (
    <ProtectedLayout>
      <div className="p-6 space-y-6">
        <div>
          <h1 className="text-3xl font-bold flex items-center gap-3">
            <Target className="h-8 w-8 text-primary" />
            Match Job
          </h1>
          <p className="text-muted-foreground mt-1">
            Paste a job description to see how well you match
          </p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          {/* Job Description Input */}
          <Card>
            <CardHeader>
              <CardTitle>Job Description</CardTitle>
              <CardDescription>
                Paste the full job description or key requirements
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <Textarea
                placeholder="Paste job description here..."
                value={jobDescription}
                onChange={(e) => setJobDescription(e.target.value)}
                rows={16}
                className="resize-none"
              />
              <div className="flex gap-3">
                <Button 
                  onClick={handleAnalyze} 
                  disabled={!jobDescription.trim() || isAnalyzing}
                  className="flex-1"
                >
                  {isAnalyzing ? (
                    <>
                      <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                      Analyzing...
                    </>
                  ) : (
                    <>
                      <Sparkles className="h-4 w-4 mr-2" />
                      Analyze Match
                    </>
                  )}
                </Button>
                {(jobDescription || matchResult) && (
                  <Button variant="outline" onClick={handleClear}>
                    Clear
                  </Button>
                )}
              </div>
            </CardContent>
          </Card>

          {/* Match Result */}
          <Card>
            <CardHeader>
              <CardTitle>Match Analysis</CardTitle>
              <CardDescription>
                How well your profile matches this job
              </CardDescription>
            </CardHeader>
            <CardContent>
              {!matchResult && !isAnalyzing && (
                <div className="flex flex-col items-center justify-center py-12 text-center">
                  <Sparkles className="h-12 w-12 text-muted-foreground/30 mb-4" />
                  <p className="text-muted-foreground">
                    Paste a job description and click &quot;Analyze Match&quot; to see your compatibility
                  </p>
                </div>
              )}

              {isAnalyzing && (
                <div className="flex flex-col items-center justify-center py-12 text-center">
                  <Loader2 className="h-12 w-12 text-primary animate-spin mb-4" />
                  <p className="text-muted-foreground">
                    Analyzing your profile against the job requirements...
                  </p>
                </div>
              )}

              {matchResult && !isAnalyzing && (
                <div className="space-y-6">
                  {/* AI Plaintext Result */}
                  <div className="p-4 rounded-lg bg-muted/30 border whitespace-pre-wrap font-sans text-sm leading-relaxed overflow-auto max-h-[500px]">
                    {matchResult}
                  </div>

                  {/* Create Application Button */}
                  <Button 
                    className="w-full" 
                    onClick={() => setIsCreateModalOpen(true)}
                  >
                    Create Application for This Job
                  </Button>
                </div>
              )}
            </CardContent>
          </Card>
        </div>
      </div>

      {/* Create Application Modal */}
      <CreateApplicationModal
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
      />
    </ProtectedLayout>
  );
}

function CreateApplicationModal({
  open,
  onOpenChange,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [formData, setFormData] = useState<CreateJobApplicationDto>({
    company: '',
    position: '',
    jobUrl: '',
    location: '',
    workMode: WorkMode.Remote,
    notes: '',
  });

  const createMutation = useMutation({
    mutationFn: (data: CreateJobApplicationDto) => applicationService.create(data),
    onSuccess: (newApp) => {
      queryClient.invalidateQueries({ queryKey: ['applications'] });
      onOpenChange(false);
      router.push(`/applications/${newApp.id}`);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    createMutation.mutate(formData);
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg">
        <DialogHeader>
          <DialogTitle>Create Job Application</DialogTitle>
          <DialogDescription>
            Add the details for this job application
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="company">Company Name *</Label>
            <Input
              id="company"
              value={formData.company}
              onChange={e => setFormData(prev => ({ ...prev, company: e.target.value }))}
              placeholder="e.g., Acme Inc."
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="position">Position *</Label>
            <Input
              id="position"
              value={formData.position}
              onChange={e => setFormData(prev => ({ ...prev, position: e.target.value }))}
              placeholder="e.g., Senior Frontend Engineer"
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="jobUrl">Job URL</Label>
            <Input
              id="jobUrl"
              type="url"
              value={formData.jobUrl}
              onChange={e => setFormData(prev => ({ ...prev, jobUrl: e.target.value }))}
              placeholder="https://..."
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="location">Location</Label>
              <Input
                id="location"
                value={formData.location}
                onChange={e => setFormData(prev => ({ ...prev, location: e.target.value }))}
                placeholder="e.g., San Francisco, CA"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="workMode">Work Type</Label>
              <Select
                value={formData.workMode?.toString()}
                onValueChange={value => setFormData(prev => ({ ...prev, workMode: parseInt(value) as WorkMode }))}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {Object.entries(workModeLabels).map(([value, label]) => (
                    <SelectItem key={value} value={value}>{label}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="notes">Notes</Label>
            <Textarea
              id="notes"
              rows={3}
              value={formData.notes}
              onChange={e => setFormData(prev => ({ ...prev, notes: e.target.value }))}
              placeholder="Any additional notes..."
            />
          </div>

          <div className="flex justify-end gap-3 pt-4">
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancel
            </Button>
            <Button type="submit" disabled={createMutation.isPending}>
              {createMutation.isPending ? 'Creating...' : 'Create Application'}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
