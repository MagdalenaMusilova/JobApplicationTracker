export interface CreateJAEvent {
    JAStatusEntryId: string,
    eventName: string;
    eventType: number;
    eventDate: string;
    isWholeDay?: boolean;
    note: string | null;
}