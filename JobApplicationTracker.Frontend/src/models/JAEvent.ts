import {JAEventType} from "./enums/JAEventType";

export interface JAEvent {
    id: string;
    eventName: string;
    eventType: JAEventType;
    eventDate: Date;
    isWholeDay: boolean;
}