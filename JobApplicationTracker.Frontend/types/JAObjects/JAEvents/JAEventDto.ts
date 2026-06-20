export interface JAEventDto {
  id: string;
  jaStatusEntryId: string;
  jobApplicationId: string;
  eventName: string;
  eventType: number;
  eventDate: string;
  isWholeDay: boolean;
  note?: string | null;
}