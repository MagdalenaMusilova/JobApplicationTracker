import type { JAEventDto } from '../JAEvents/JAEventDto';

export interface JAStatusEntryDto {
  id: string;
  jobApplicationId: string;
  orderIndex: number;
  jaStatusType: number;
  createdAt: string;
  note?: string | null;
  jaEvent?: JAEventDto;
}