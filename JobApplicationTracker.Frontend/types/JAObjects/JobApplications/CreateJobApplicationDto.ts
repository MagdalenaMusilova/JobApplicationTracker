import type { CreateJAEventDto } from '../JAEvents/CreateJAEventDto';
import type { CreateJAStatusEntryDto } from '../JAStatuses/CreateJAStatusEntryDto';

export interface CreateJobApplicationDto {
  company: string;
  position: string;
  note?: string | null;
  initialStatus: CreateInitJAStatusDto;
  jobDescription?: string | null;
  jaEvent?: CreateJAEventDto | null;
}