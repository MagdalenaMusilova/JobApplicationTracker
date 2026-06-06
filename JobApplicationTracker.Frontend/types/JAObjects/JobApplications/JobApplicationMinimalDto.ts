import type { JAEventMinimalDto } from '../JAEvents/JAEventMinimalDto';

export interface JobApplicationMinimalDto {
  jAId: string;
  company: string;
  position: string;
  jaStatus: JAStatusType;
  eventType: JAEventType;
  eventDate: Date;
  isWholeDay: boolean;
  updatedAt: Date;
}