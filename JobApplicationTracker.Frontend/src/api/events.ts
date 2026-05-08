import { apiRequest } from './client'
import {JAEventType} from "../models/enums/JAEventType";

export const EventAPI = {
    getTypes: ():Promise<JAEventType[]> => apiRequest('/api/events/types'),
}