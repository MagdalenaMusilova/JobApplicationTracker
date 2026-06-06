export interface CreateJAEventDto {
  jaStatusEntryId: string;
  eventName: string;
  eventType: number;
  eventDate: string;
  isWholeDay: boolean;
  note?: string | null;
}