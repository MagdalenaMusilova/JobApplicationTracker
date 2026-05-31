'use client';

import { JobApplicationDto, workModeLabels } from '@/types';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import {
  DollarSign,
  Mail,
  User,
  Link as LinkIcon,
  MapPin,
  Briefcase,
  FileText,
} from 'lucide-react';

interface ApplicationDetailsProps {
  application: JobApplicationDto;
}

function formatSalary(amount?: number): string {
  if (!amount) return 'Not specified';
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
    maximumFractionDigits: 0,
  }).format(amount);
}

export function ApplicationDetails({ application }: ApplicationDetailsProps) {
  const details = [
    {
      icon: Briefcase,
      label: 'Work Mode',
      value: workModeLabels[application.workMode],
    },
    {
      icon: MapPin,
      label: 'Location',
      value: application.location || 'Not specified',
    },
    {
      icon: DollarSign,
      label: 'Salary Range',
      value:
        application.salaryMin || application.salaryMax
          ? `${formatSalary(application.salaryMin)} - ${formatSalary(application.salaryMax)}`
          : 'Not specified',
    },
    {
      icon: User,
      label: 'Contact',
      value: application.contactName || 'Not specified',
    },
    {
      icon: Mail,
      label: 'Contact Email',
      value: application.contactEmail || 'Not specified',
      isLink: !!application.contactEmail,
      href: application.contactEmail ? `mailto:${application.contactEmail}` : undefined,
    },
    {
      icon: LinkIcon,
      label: 'Job URL',
      value: application.jobUrl ? 'View Posting' : 'Not available',
      isLink: !!application.jobUrl,
      href: application.jobUrl,
    },
  ];

  return (
    <Card className="bg-card border-border">
      <CardHeader>
        <CardTitle className="text-lg font-semibold">Details</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          {details.map((detail) => (
            <div key={detail.label} className="flex items-start gap-3">
              <div className="p-2 rounded-lg bg-muted">
                <detail.icon className="h-4 w-4 text-muted-foreground" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">{detail.label}</p>
                {detail.isLink && detail.href ? (
                  <a
                    href={detail.href}
                    target={detail.href.startsWith('mailto:') ? undefined : '_blank'}
                    rel="noopener noreferrer"
                    className="text-sm font-medium text-primary hover:underline"
                  >
                    {detail.value}
                  </a>
                ) : (
                  <p className="text-sm font-medium text-foreground">{detail.value}</p>
                )}
              </div>
            </div>
          ))}
        </div>

        {/* Notes Section */}
        {application.notes && (
          <div className="pt-4 border-t border-border">
            <div className="flex items-start gap-3">
              <div className="p-2 rounded-lg bg-muted">
                <FileText className="h-4 w-4 text-muted-foreground" />
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Notes</p>
                <p className="text-sm text-foreground whitespace-pre-wrap mt-1">
                  {application.notes}
                </p>
              </div>
            </div>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
