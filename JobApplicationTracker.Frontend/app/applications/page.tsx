'use client';

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { applicationService } from '@/services/application-service';
import { ProtectedLayout } from '@/components/layout/protected-layout';
import { ApplicationsTable } from '@/components/applications/applications-table';
import { ApplicationFilters } from '@/components/applications/application-filters';
import { CreateApplicationModal } from '@/components/applications/create-application-modal';
import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';
import { Plus } from 'lucide-react';
import { ApplicationFilterParams } from '@/types';

export default function ApplicationsPage() {
  const [filters, setFilters] = useState<ApplicationFilterParams>({
    pageNumber: 1,
    pageSize: 10,
    sortBy: 'appliedDate',
    sortDirection: 'desc',
  });
  const [showCreateModal, setShowCreateModal] = useState(false);

  const { data, isLoading, refetch } = useQuery({
    queryKey: ['applications', filters],
    queryFn: () => applicationService.getAll(filters),
  });

  const handleFilterChange = (newFilters: Partial<ApplicationFilterParams>) => {
    setFilters(prev => ({
      ...prev,
      ...newFilters,
      pageNumber: 1, // Reset to first page on filter change
    }));
  };

  const handlePageChange = (page: number) => {
    setFilters(prev => ({
      ...prev,
      pageNumber: page,
    }));
  };

  return (
    <ProtectedLayout>
      <div className="space-y-6">
        {/* Header */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div>
            <h1 className="text-3xl font-bold tracking-tight text-foreground">Applications</h1>
            <p className="text-muted-foreground mt-1">
              Manage and track all your job applications
            </p>
          </div>
          <Button onClick={() => setShowCreateModal(true)}>
            <Plus className="h-4 w-4 mr-2" />
            Add Application
          </Button>
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
              <Skeleton key={i} className="h-16 w-full" />
            ))}
          </div>
        ) : data ? (
          <ApplicationsTable
            applications={data.items}
            totalCount={data.totalCount}
            pageNumber={data.pageNumber}
            pageSize={data.pageSize}
            totalPages={data.totalPages}
            onPageChange={handlePageChange}
            onRefresh={refetch}
          />
        ) : null}

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
