'use client';

import { useState } from 'react';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import {
  Sparkles,
  Target,
  Loader2,
} from 'lucide-react';
import { matchService } from '@/services/match-service';
import { toast } from 'sonner';

export default function MatchPage() {
  const [jobDescription, setJobDescription] = useState('');
  const [matchResult, setMatchResult] = useState<string | null>(null);
  const [isAnalyzing, setIsAnalyzing] = useState(false);

  const handleAnalyze = async () => {
    if (!jobDescription.trim()) return;

    setIsAnalyzing(true);
    try {
      const result = await matchService.analyzeMatch(jobDescription);
      setMatchResult(result);
    } catch (error: any) {
      const errorMessage = error?.response?.data?.error || 'Failed to analyze match';
      toast.error(errorMessage);
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
                </div>
              )}
            </CardContent>
          </Card>
        </div>
      </div>
    </ProtectedLayout>
  );
}
