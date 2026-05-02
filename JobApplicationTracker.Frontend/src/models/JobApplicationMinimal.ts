import {JAStatusType} from "./enums/JAStatusType";
import {JAEventType} from "./enums/JAEventType";

export interface JobApplicationMinimal {
    id: string,
    company: string,
    position: string,
    note: string | null,
    jaStatus: JAStatusType,
    jaEvent: JAEventType | null,
}