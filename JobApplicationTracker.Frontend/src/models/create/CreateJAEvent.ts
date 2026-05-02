export interface CreateJAEvent {
    JAStatusEntryId: string,
    eventName: string;
    eventType: number;
    eventDate: Date;
    isWholeDay?: boolean;
    note: string | null;
}