import type { JAStatusType } from '../../Enums/JAStatusType';
import type { JAEventType } from '../../Enums/JAEventType';

export interface JobApplicationMinimalDto {
  jaId: string;
  company: string;
  position: string;
  jaStatus: JAStatusType;
  eventType: JAEventType | null;
  eventDate: string | null;
  isWholeDay: boolean | null;
  updatedAt: string;
}