'use client';

import { format } from 'date-fns';
import {
  JobApplicationDto,
  applicationStatusLabels,
  applicationStatusColors,
  workModeLabels,
} from '@/types';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  ExternalLink,
  MoreHorizontal,
  RefreshCw,
  Trash2,
  Loader2,
} from 'lucide-react';
import { cn } from '@/lib/utils';

interface ApplicationHeaderProps {
  application: JobApplicationDto;
  onUpdateStatus: () => void;
  onDelete: () => void;
  isDeleting: boolean;
}

export function ApplicationHeader({
  application,
  onUpdateStatus,
  onDelete,
  isDeleting,
}: ApplicationHeaderProps) {
  return (
    <div className="bg-card border border-border rounded-lg p-6">
      <div className="flex flex-col md:flex-row md:items-start md:justify-between gap-4">
        {/* Left Side - Company Info */}
        <div className="flex items-start gap-4">
          <div className="w-14 h-14 rounded-xl bg-primary/10 flex items-center justify-center flex-shrink-0">
            <span className="text-xl font-bold text-primary">
              {application.companyName.charAt(0).toUpperCase()}
            </span>
          </div>
          <div>
            <div className="flex items-center gap-3 flex-wrap">
              <h1 className="text-2xl font-bold text-foreground">
                {application.companyName}
              </h1>
              <Badge
                variant="outline"
                className={cn('text-sm', applicationStatusColors[application.currentStatus])}
              >
                {applicationStatusLabels[application.currentStatus]}
              </Badge>
            </div>
            <p className="text-lg text-muted-foreground mt-1">
              {application.jobTitle}
            </p>
            <div className="flex items-center gap-4 mt-2 text-sm text-muted-foreground">
              {application.location && <span>{application.location}</span>}
              <span>{workModeLabels[application.workMode]}</span>
              <span>Applied {format(new Date(application.appliedDate), 'MMM d, yyyy')}</span>
            </div>
          </div>
        </div>

        {/* Right Side - Actions */}
        <div className="flex items-center gap-2">
          <Button onClick={onUpdateStatus}>
            <RefreshCw className="h-4 w-4 mr-2" />
            Update Status
          </Button>
          
          {application.jobUrl && (
            <Button variant="outline" asChild>
              <a href={application.jobUrl} target="_blank" rel="noopener noreferrer">
                <ExternalLink className="h-4 w-4 mr-2" />
                View Job
              </a>
            </Button>
          )}

          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="outline" size="icon">
                <MoreHorizontal className="h-4 w-4" />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end">
              <DropdownMenuItem onClick={onUpdateStatus}>
                <RefreshCw className="h-4 w-4 mr-2" />
                Update Status
              </DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem
                onClick={onDelete}
                disabled={isDeleting}
                className="text-destructive focus:text-destructive"
              >
                {isDeleting ? (
                  <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                ) : (
                  <Trash2 className="h-4 w-4 mr-2" />
                )}
                Delete Application
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      </div>
    </div>
  );
}
