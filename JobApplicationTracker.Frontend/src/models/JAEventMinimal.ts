import {JAEventType} from "./enums/JAEventType";

export interface JAEventMinimal {
    id: string,
    eventName: string;
    eventType: JAEventType,
    eventDate: Date,
    isWholeDay: boolean,
}