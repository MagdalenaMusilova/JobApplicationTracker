'use client';

import Link from 'next/link';
import { format } from 'date-fns';
import {
  JobApplicationListDto,
  applicationStatusLabels,
  applicationStatusColors,
  workModeLabels,
} from '@/types';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { ArrowRight, Building2, MapPin } from 'lucide-react';
import { cn } from '@/lib/utils';

interface RecentApplicationsProps {
  applications: JobApplicationListDto[];
}

export function RecentApplications({ applications }: RecentApplicationsProps) {
  return (
    <Card className="bg-card border-border h-full">
      <CardHeader className="flex flex-row items-center justify-between pb-4">
        <CardTitle className="text-lg font-semibold">Recent Applications</CardTitle>
        <Link href="/applications">
          <Button variant="ghost" size="sm" className="text-muted-foreground hover:text-foreground">
            View all
            <ArrowRight className="ml-1 h-4 w-4" />
          </Button>
        </Link>
      </CardHeader>
      <CardContent>
        {applications.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-8 text-center">
            <Building2 className="h-10 w-10 text-muted-foreground/50 mb-3" />
            <p className="text-sm text-muted-foreground">No applications yet</p>
            <p className="text-xs text-muted-foreground/70 mt-1">
              Start tracking your job applications
            </p>
          </div>
        ) : (
          <div className="space-y-1">
            {applications.map((app) => (
              <Link
                key={app.id}
                href={`/applications/${app.id}`}
                className="block"
              >
                <div className="flex items-center gap-4 p-3 rounded-lg hover:bg-accent/50 transition-colors">
                  {/* Company Initial */}
                  <div className="w-10 h-10 rounded-lg bg-primary/10 flex items-center justify-center flex-shrink-0">
                    <span className="text-sm font-semibold text-primary">
                      {app.companyName.charAt(0).toUpperCase()}
                    </span>
                  </div>
                  
                  {/* Details */}
                  <div className="flex-1 min-w-0">
                    <div className="flex items-center gap-2">
                      <p className="font-medium text-foreground truncate">
                        {app.companyName}
                      </p>
                      <Badge
                        variant="outline"
                        className={cn('text-xs', applicationStatusColors[app.currentStatus])}
                      >
                        {applicationStatusLabels[app.currentStatus]}
                      </Badge>
                    </div>
                    <p className="text-sm text-muted-foreground truncate">
                      {app.jobTitle}
                    </p>
                    <div className="flex items-center gap-3 mt-1 text-xs text-muted-foreground">
                      {app.location && (
                        <span className="flex items-center gap-1">
                          <MapPin className="h-3 w-3" />
                          {app.location}
                        </span>
                      )}
                      <span>{workModeLabels[app.workMode]}</span>
                    </div>
                  </div>
                  
                  {/* Date */}
                  <div className="text-right flex-shrink-0">
                    <p className="text-xs text-muted-foreground">
                      {format(new Date(app.appliedDate), 'MMM d')}
                    </p>
                  </div>
                </div>
              </Link>
            ))}
          </div>
        )}
      </CardContent>
    </Card>
  );
}
