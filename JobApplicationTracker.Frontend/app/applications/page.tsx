'use client';

import { useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';

import { applicationService } from '@/services/application-service';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { ApplicationsTable } from '@/components/applications/applications-table';
import { ApplicationFilters } from '@/components/applications/application-filters';
import { CreateApplicationModal } from '@/components/applications/create-application-modal';

import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';

import { Plus } from 'lucide-react';
import { ApplicationFilterParams } from '@/types/JAObjects/JobApplications/ApplicationFilterParams';
import { JAStatusType } from '@/types/Enums/JAStatusType';

// "Opened" statuses (default filter)
const OPENED_STATUSES = [
  JAStatusType.Wishlist,
  JAStatusType.Applied,
  JAStatusType.Task,
  JAStatusType.Interview,
  JAStatusType.Offer
];

export default function ApplicationsPage() {
  const [showCreateModal, setShowCreateModal] = useState(false);

  const [filters, setFilters] = useState<ApplicationFilterParams>({
    search: '',
    statuses: OPENED_STATUSES,
    sortBy: 'updatedAt',
    sortDirection: 'desc',
  });

  /**
   * Always fetch active applications
   * Fetch archived only when needed (when Accepted/Rejected statuses are selected)
   */
  const activeQuery = useQuery({
    queryKey: ['applications', 'active'],
    queryFn: () => applicationService.getAllMinimal('active'),
    staleTime: 1000 * 60 * 10,
  });

  // Check if we need archived data (if Accepted or Rejected are selected)
  const needsArchivedData = filters.statuses?.some(
    status => status === JAStatusType.Accepted || status === JAStatusType.Rejected
  ) ?? false;

  const archivedQuery = useQuery({
    queryKey: ['applications', 'archived'],
    queryFn: () => applicationService.getAllMinimal('archived'),
    staleTime: 1000 * 60 * 10,
    enabled: needsArchivedData, // Only fetch when we need archived statuses
  });

  // Combine data from both sources
  const sourceApplications = useMemo(() => {
    const active = activeQuery.data ?? [];
    const archived = archivedQuery.data ?? [];
    return [...active, ...archived];
  }, [activeQuery.data, archivedQuery.data]);

  const isLoading = activeQuery.isLoading || (needsArchivedData && archivedQuery.isLoading);

  const refetch = () => {
    activeQuery.refetch();
    if (needsArchivedData) {
      archivedQuery.refetch();
    }
  };

  const filteredApplications = useMemo(() => {
    let items = [...sourceApplications];

    // Filter by search word (company or position)
    if (filters.search?.trim()) {
      const search = filters.search.toLowerCase();

      items = items.filter(app => {
        const company = app.company?.toLowerCase() ?? '';
        const position = app.position?.toLowerCase() ?? '';

        return company.includes(search) || position.includes(search);
      });
    }

    // Filter by statuses (multiselect)
    if (filters.statuses?.length) {
      items = items.filter(app =>
          filters.statuses!.includes(app.jaStatus)
      );
    }

    // Multi-level sorting
    items.sort((a, b) => {
      // 1. Sort by UpdatedAt (most recent first)
      const aUpdatedAt = new Date(a.updatedAt).getTime();
      const bUpdatedAt = new Date(b.updatedAt).getTime();
      if (aUpdatedAt !== bUpdatedAt) {
        return bUpdatedAt - aUpdatedAt; // descending (most recent first)
      }

      // 2. Sort by event type (no event JAs at the bottom)
      const aHasEvent = a.eventType !== null && a.eventType !== undefined;
      const bHasEvent = b.eventType !== null && b.eventType !== undefined;
      if (aHasEvent !== bHasEvent) {
        return aHasEvent ? -1 : 1; // items with events first
      }

      // 3. Sort by event date (if both have events, no event JAs at the bottom)
      if (aHasEvent && bHasEvent) {
        const aEventDate = a.eventDate ? new Date(a.eventDate).getTime() : Number.MAX_SAFE_INTEGER;
        const bEventDate = b.eventDate ? new Date(b.eventDate).getTime() : Number.MAX_SAFE_INTEGER;
        if (aEventDate !== bEventDate) {
          return aEventDate - bEventDate; // ascending (soonest first)
        }
      }

      // 4. Sort by status
      if (a.jaStatus !== b.jaStatus) {
        return a.jaStatus - b.jaStatus;
      }

      // 5. Sort by position
      const positionCompare = a.position.localeCompare(b.position);
      if (positionCompare !== 0) {
        return positionCompare;
      }

      // 6. Sort by company
      return a.company.localeCompare(b.company);
    });

    return items;
  }, [sourceApplications, filters]);

  const handleFilterChange = (
      newFilters: Partial<ApplicationFilterParams>
  ) => {
    setFilters(prev => ({
      ...prev,
      ...newFilters,
    }));
  };

  return (
      <ProtectedLayout>
        <div className="space-y-6">
          {/* Header */}
          <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
            <div>
              <h1 className="text-3xl font-bold tracking-tight text-foreground">
                Applications
              </h1>

              <p className="text-muted-foreground mt-1">
                Manage and track all your job applications
              </p>
            </div>

            <div className="flex gap-2">
{/*              <Button
                  variant={
                    showArchived ? 'outline' : 'default'
                  }
                  onClick={() => setShowArchived(false)}
              >
                Active
              </Button>

              <Button
                  variant={
                    showArchived ? 'default' : 'outline'
                  }
                  onClick={() => setShowArchived(true)}
              >
                Archived
              </Button>*/}

              <Button
                  onClick={() =>
                      setShowCreateModal(true)
                  }
              >
                <Plus className="h-4 w-4 mr-2" />
                Add Application
              </Button>
            </div>
          </div>

          {/* Filters */}
          <ApplicationFilters
              filters={filters}
              onFilterChange={handleFilterChange}
          />

          {/* Table */}
          {isLoading ? (
              <div className="space-y-4">
                <Skeleton className="h-12 w-full" />

                {[...Array(5)].map((_, i) => (
                    <Skeleton
                        key={i}
                        className="h-16 w-full"
                    />
                ))}
              </div>
          ) : (
              <ApplicationsTable
                  applications={filteredApplications}
                  totalCount={filteredApplications.length}
                  pageNumber={1}
                  pageSize={filteredApplications.length}
                  totalPages={1}
                  onPageChange={() => {}}
                  onRefresh={refetch}
              />
          )}

          {/* Create Modal */}
          <CreateApplicationModal
              open={showCreateModal}
              onOpenChange={setShowCreateModal}
              onSuccess={() => {
                refetch();
                setShowCreateModal(false);
              }}
          />
        </div>
      </ProtectedLayout>
  );
}