export interface ApplicationFilterParams {
  search?: string;
  statuses?: number[];
  dateFrom?: string;
  dateTo?: string;
  sortBy?: 'appliedDate' | 'company' | 'status' | 'updatedAt';
  sortDirection?: 'asc' | 'desc';
  pageNumber?: number;
  pageSize?: number;
}
