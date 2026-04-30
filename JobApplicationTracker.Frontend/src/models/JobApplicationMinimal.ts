import {JAEvent} from "./JAEvent";
import {JAStatusType} from "./enums/JAStatusType";

export interface JobApplicationMinimal {
    id: string;
    company: string;
    position: string;
    status : JAStatusType;
    event: JAEvent | null;
}