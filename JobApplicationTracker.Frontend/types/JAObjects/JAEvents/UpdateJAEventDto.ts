export interface UpdateJAEventDto {
  eventName?: string | null;
  eventType?: number | null;
  eventDate?: string | null;
  isWholeDay?: boolean | null;
  note?: string | null;
}