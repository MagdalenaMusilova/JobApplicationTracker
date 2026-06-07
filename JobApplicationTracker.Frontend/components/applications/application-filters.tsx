'use client';

import { Input } from '@/components/ui/input';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Button } from '@/components/ui/button';
import { Search, X } from 'lucide-react';
import { ApplicationFilterParams } from '@/types/JAObjects/JobApplications/ApplicationFilterParams';
import { JAStatusType, jaStatusLabels, applicationStatusColors } from '@/types/Enums/JAStatusType';
import { cn } from '@/lib/utils';

interface ApplicationFiltersProps {
  filters: ApplicationFilterParams;
  onFilterChange: (filters: Partial<ApplicationFilterParams>) => void;
}

export function ApplicationFilters({ filters, onFilterChange }: ApplicationFiltersProps) {
  // Track multiple selected statuses
  const selectedStatuses: JAStatusType[] = filters.statuses || [];
  
  const hasActiveFilters = filters.search || selectedStatuses.length > 0;

  const clearFilters = () => {
    onFilterChange({
      search: undefined,
      statuses: undefined,
    });
  };

  const toggleStatus = (status: JAStatusType) => {
    const current = selectedStatuses;
    const isSelected = current.includes(status);
    
    if (isSelected) {
      // Remove status
      const newStatuses = current.filter(s => s !== status);
      onFilterChange({ statuses: newStatuses.length > 0 ? newStatuses : undefined });
    } else {
      // Add status
      onFilterChange({ statuses: [...current, status] });
    }
  };

  return (
    <div className="space-y-4">
      {/* Search and Sort Row */}
      <div className="flex flex-col sm:flex-row gap-4">
        {/* Search */}
        <div className="relative flex-1 max-w-md">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search by company, position..."
            value={filters.search || ''}
            onChange={(e) => onFilterChange({ search: e.target.value || undefined })}
            className="pl-10 bg-input"
          />
        </div>

        {/* Sort By */}
        <Select
          value={filters.sortBy || 'updatedAt'}
          onValueChange={(value) =>
            onFilterChange({ sortBy: value as ApplicationFilterParams['sortBy'] })
          }
        >
          <SelectTrigger className="w-full sm:w-[180px] bg-input">
            <SelectValue placeholder="Sort by" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="updatedAt">Last Updated</SelectItem>
            <SelectItem value="appliedDate">Date Applied</SelectItem>
            <SelectItem value="company">Company</SelectItem>
            <SelectItem value="status">Status</SelectItem>
          </SelectContent>
        </Select>

        {/* Clear Filters */}
        {hasActiveFilters && (
          <Button variant="ghost" onClick={clearFilters} className="px-3">
            <X className="h-4 w-4 mr-1" />
            Clear
          </Button>
        )}
      </div>

      {/* Status Filter Buttons */}
      <div className="flex flex-wrap gap-2">
        <span className="text-sm text-muted-foreground py-1 pr-2">Filter by status:</span>
        {Object.entries(jaStatusLabels).map(([value, label]) => {
          const status = Number(value) as JAStatusType;
          const isSelected = selectedStatuses.includes(status);
          
          return (
            <Button
              key={value}
              variant={isSelected ? "default" : "outline"}
              size="sm"
              onClick={() => toggleStatus(status)}
              className={cn(
                "h-7 text-xs",
                isSelected && "ring-2 ring-offset-2 ring-primary",
                !isSelected && applicationStatusColors[status]
              )}
            >
              {label}
            </Button>
          );
        })}
      </div>
    </div>
  );
}
