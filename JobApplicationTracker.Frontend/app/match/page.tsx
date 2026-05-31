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
  CheckCircle2,
  XCircle,
  Lightbulb,
  Target,
  AlertCircle,
  Loader2,
} from 'lucide-react';
import { applicationService } from '@/services/application-service';
import { WorkMode, workModeLabels } from '@/types';
import type { CreateJobApplicationDto, JobMatchResultDto } from '@/types';
import { useRouter } from 'next/navigation';

// Mock match analysis function (would be backend API call)
async function analyzeJobMatch(jobDescription: string): Promise<JobMatchResultDto> {
  // Simulate API delay
  await new Promise(resolve => setTimeout(resolve, 2000));
  
  // Mock analysis result based on job description content
  const hasReact = jobDescription.toLowerCase().includes('react');
  const hasTypeScript = jobDescription.toLowerCase().includes('typescript');
  const hasNext = jobDescription.toLowerCase().includes('next');
  const hasNode = jobDescription.toLowerCase().includes('node');
  const hasPython = jobDescription.toLowerCase().includes('python');
  const hasAWS = jobDescription.toLowerCase().includes('aws');
  const hasDocker = jobDescription.toLowerCase().includes('docker');
  const hasGraphQL = jobDescription.toLowerCase().includes('graphql');
  
  let score = 60; // Base score
  const matchReasons: string[] = [];
  const missingSkills: string[] = [];
  const recommendations: string[] = [];
  
  if (hasReact) { score += 10; matchReasons.push('Strong React experience matches requirement'); }
  if (hasTypeScript) { score += 10; matchReasons.push('TypeScript proficiency aligns well'); }
  if (hasNext) { score += 8; matchReasons.push('Next.js expertise is a strong match'); }
  if (hasNode) { score += 5; matchReasons.push('Node.js backend experience is relevant'); }
  if (hasGraphQL) { score += 5; matchReasons.push('GraphQL knowledge matches requirements'); }
  
  if (hasPython && !hasNode) { missingSkills.push('Python'); recommendations.push('Consider taking a Python course to expand backend skills'); }
  if (hasAWS) { missingSkills.push('AWS'); recommendations.push('AWS certifications could strengthen your application'); }
  if (hasDocker) { missingSkills.push('Docker/Kubernetes'); recommendations.push('Container experience would be beneficial'); }
  
  if (matchReasons.length === 0) {
    matchReasons.push('Your frontend skills provide a foundation for this role');
  }
  
  if (missingSkills.length === 0) {
    missingSkills.push('No major gaps identified');
  }
  
  score = Math.min(98, Math.max(40, score));
  
  let overallAssessment = '';
  if (score >= 85) {
    overallAssessment = 'Excellent match! Your skills and experience align very well with this role. You should strongly consider applying.';
  } else if (score >= 70) {
    overallAssessment = 'Good match. You have many of the required skills. Consider highlighting your relevant experience in your application.';
  } else if (score >= 55) {
    overallAssessment = 'Moderate match. While you have some relevant skills, there are areas for growth. This could be a stretch role.';
  } else {
    overallAssessment = 'This role may require skills outside your current expertise. Consider if this aligns with your career goals.';
  }
  
  return {
    matchScore: score,
    matchReasons,
    missingSkills,
    recommendations,
    overallAssessment,
  };
}

export default function MatchPage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [jobDescription, setJobDescription] = useState('');
  const [matchResult, setMatchResult] = useState<JobMatchResultDto | null>(null);
  const [isAnalyzing, setIsAnalyzing] = useState(false);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

  const handleAnalyze = async () => {
    if (!jobDescription.trim()) return;
    
    setIsAnalyzing(true);
    try {
      const result = await analyzeJobMatch(jobDescription);
      setMatchResult(result);
    } finally {
      setIsAnalyzing(false);
    }
  };

  const handleClear = () => {
    setJobDescription('');
    setMatchResult(null);
  };

  const getScoreColor = (score: number) => {
    if (score >= 80) return 'text-green-500';
    if (score >= 60) return 'text-yellow-500';
    return 'text-orange-500';
  };

  const getScoreBgColor = (score: number) => {
    if (score >= 80) return 'bg-green-500';
    if (score >= 60) return 'bg-yellow-500';
    return 'bg-orange-500';
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
                placeholder="Paste job description here...

Example:
We're looking for a Senior Frontend Engineer with experience in:
- React and TypeScript
- Next.js or similar frameworks
- RESTful APIs and GraphQL
- Modern CSS (Tailwind, CSS-in-JS)
- Testing frameworks (Jest, Playwright)"
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
                  {/* Score */}
                  <div className="text-center">
                    <div className={`text-5xl font-bold ${getScoreColor(matchResult.matchScore)}`}>
                      {matchResult.matchScore}%
                    </div>
                    <p className="text-muted-foreground mt-1">Match Score</p>
                    <Progress 
                      value={matchResult.matchScore} 
                      className={`mt-3 h-2 ${getScoreBgColor(matchResult.matchScore)}`} 
                    />
                  </div>

                  {/* Overall Assessment */}
                  <div className="p-4 rounded-lg bg-muted/50 border">
                    <p className="text-sm">{matchResult.overallAssessment}</p>
                  </div>

                  {/* Matching Skills */}
                  <div className="space-y-2">
                    <h4 className="text-sm font-medium flex items-center gap-2">
                      <CheckCircle2 className="h-4 w-4 text-green-500" />
                      Why You Match
                    </h4>
                    <ul className="space-y-1">
                      {matchResult.matchReasons.map((reason, i) => (
                        <li key={i} className="text-sm text-muted-foreground flex items-start gap-2">
                          <span className="text-green-500 mt-1">+</span>
                          {reason}
                        </li>
                      ))}
                    </ul>
                  </div>

                  {/* Missing Skills */}
                  <div className="space-y-2">
                    <h4 className="text-sm font-medium flex items-center gap-2">
                      <AlertCircle className="h-4 w-4 text-yellow-500" />
                      Areas to Develop
                    </h4>
                    <div className="flex flex-wrap gap-2">
                      {matchResult.missingSkills.map((skill, i) => (
                        <Badge key={i} variant="outline" className="text-yellow-600 border-yellow-500/30">
                          {skill}
                        </Badge>
                      ))}
                    </div>
                  </div>

                  {/* Recommendations */}
                  {matchResult.recommendations.length > 0 && (
                    <div className="space-y-2">
                      <h4 className="text-sm font-medium flex items-center gap-2">
                        <Lightbulb className="h-4 w-4 text-blue-500" />
                        Recommendations
                      </h4>
                      <ul className="space-y-1">
                        {matchResult.recommendations.map((rec, i) => (
                          <li key={i} className="text-sm text-muted-foreground flex items-start gap-2">
                            <span className="text-blue-500 mt-1">*</span>
                            {rec}
                          </li>
                        ))}
                      </ul>
                    </div>
                  )}

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
        matchScore={matchResult?.matchScore}
      />
    </ProtectedLayout>
  );
}

function CreateApplicationModal({
  open,
  onOpenChange,
  matchScore,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  matchScore?: number;
}) {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [formData, setFormData] = useState<CreateJobApplicationDto>({
    companyName: '',
    jobTitle: '',
    jobUrl: '',
    location: '',
    workMode: WorkMode.Remote,
    notes: matchScore ? `AI Match Score: ${matchScore}%` : '',
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
            <Label htmlFor="companyName">Company Name *</Label>
            <Input
              id="companyName"
              value={formData.companyName}
              onChange={e => setFormData(prev => ({ ...prev, companyName: e.target.value }))}
              placeholder="e.g., Acme Inc."
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="jobTitle">Position *</Label>
            <Input
              id="jobTitle"
              value={formData.jobTitle}
              onChange={e => setFormData(prev => ({ ...prev, jobTitle: e.target.value }))}
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
