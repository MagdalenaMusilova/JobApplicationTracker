import type { JAStatusEntryDto } from '../JAStatuses/JAStatusEntryDto';
import type { JobListingDto } from '../JobListing/JobListingDto';
import type { JAEventDto } from '../JAEvents/JAEventDto';

export interface JobApplicationDto {
  id: string;
  userId: string;
  company: string;
  position: string;
  note: string | null;
  jobListing: JobListingDto;
  statusHistory: JAStatusEntryDto[];
  event?: JAEventDto | null;
}