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

const ARCHIVE_TAGS = ['Rejected', 'Withdrawn', 'Ghosted'];

export interface ApplicationFilterParams {
  search?: string;
  statuses?: ApplicationStatus[];
  sortBy?: 'appliedDate' | 'company' | 'role' | 'status' | 'updatedAt';
  sortDirection?: 'asc' | 'desc';
}

/**
 * Hardcoded custom filters.
 * Add whatever business logic you want here.
 */
const CUSTOM_TAG_FILTERS: Record<
    string,
    (application: any) => boolean
> = {
  activeProcess: app =>
      app.tags?.includes('Interview') ||
      app.tags?.includes('Assessment'),

  offer: app =>
      app.tags?.includes('Offer'),

  waiting: app =>
      !app.tags?.includes('Offer') &&
      !app.tags?.includes('Rejected'),
};

export default function ApplicationsPage() {
  const [showCreateModal, setShowCreateModal] = useState(false);

  const [showArchived, setShowArchived] = useState(false);

  const [filters, setFilters] = useState<
      ApplicationFilterParams & {
    customTags?: string[];
  }
  >({
    searchWord: '',
    tags: [],
    customTags: [],
    sortBy: 'appliedDate',
    sortDirection: 'desc',
  });

  /**
   * Single fetch.
   * React Query caches the result.
   */
  const { data, isLoading, refetch } = useQuery({
    queryKey: ['applications'],
    queryFn: () => applicationService.getAllMinimal(),
    staleTime: 1000 * 60 * 10,
  });

  const activeApplications = useMemo(() => {
    return (
        data ?? []
    );
  }, [data]);

  const archivedApplications = useMemo(() => {
    return (
        data ?? []
    );
  }, [data]);

  const sourceApplications = showArchived
      ? archivedApplications
      : activeApplications;

  const filteredApplications = useMemo(() => {
    let items = [...sourceApplications];

    if (filters.searchWord?.trim()) {
      const search = filters.searchWord.toLowerCase();

      items = items.filter(app => {
        const company =
            app.companyName?.toLowerCase() ?? '';

        const role =
            app.roleTitle?.toLowerCase() ?? '';

        return (
            company.includes(search) ||
            role.includes(search)
        );
      });
    }

    if (filters.tags?.length) {
      items = items.filter(app =>
          filters.tags.every(tag =>
              app.tags?.includes(tag)
          )
      );
    }

    if (filters.customTags?.length) {
      items = items.filter(app =>
          filters.customTags!.every(customTag => {
            const filterFn =
                CUSTOM_TAG_FILTERS[customTag];

            return filterFn ? filterFn(app) : true;
          })
      );
    }

    items.sort((a, b) => {
      const sortBy = filters.sortBy ?? 'appliedDate';
      const direction =
          filters.sortDirection === 'asc' ? 1 : -1;

      const aValue = a[sortBy];
      const bValue = b[sortBy];

      if (aValue == null) return 1;
      if (bValue == null) return -1;

      if (aValue < bValue) return -1 * direction;
      if (aValue > bValue) return 1 * direction;

      return 0;
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