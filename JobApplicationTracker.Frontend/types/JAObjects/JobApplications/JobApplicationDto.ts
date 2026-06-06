import type { JAStatusEntryDto } from '../JAStatuses/JAStatusEntryDto';
import type { JobListingDto } from '../JobListing/JobListingDto';

export interface JobApplicationDto {
  id: string;
  userId: string;
  company: string;
  position: string;
  note?: string | null;
  jobListing: JobListingDto;
  statusHistory: JAStatusEntryDto[];
}