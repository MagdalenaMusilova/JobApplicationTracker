import { apiRequest } from './client'
import {JAEventType} from "../models/enums/JAEventType";
import {JAStatusType} from "../models/enums/JAStatusType";

export const StatusAPI = {
    getAll: () => apiRequest('/api/statuses'),

    addEntry: (payload) =>
        apiRequest('/api/applications/entry', {
            method: 'POST',
            body: JSON.stringify(payload),
        }),

    updateEntry: (id, payload) =>
        apiRequest(`/api/applications/entry/${id}`, {
            method: 'PUT',
            body: JSON.stringify(payload),
        }),

    deleteEntry: (id) =>
        apiRequest(`/api/applications/entry/${id}`, {
            method: 'DELETE',
        }),

    getTypes: ():Promise<JAStatusType[]> => apiRequest('/api/statuses/types'),
}