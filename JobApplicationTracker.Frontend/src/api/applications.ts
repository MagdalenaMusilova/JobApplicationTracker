import { apiRequest } from './client'
import {JAEventMinimal} from "../models/JAEventMinimal";
import {JobApplicationMinimal} from "../models/JobApplicationMinimal";

export const ApplicationsAPI = {
    getAll: () : Promise<JobApplicationMinimal[]> => apiRequest('/api/applications'),

    create: (payload) =>
        apiRequest('/api/applications', {
            method: 'POST',
            body: JSON.stringify(payload),
        }),

    update: (id, payload) =>
        apiRequest(`/api/applications/${id}`, {
            method: 'PUT',
            body: JSON.stringify(payload),
        }),

    remove: (id) =>
        apiRequest(`/api/applications/${id}`, {
            method: 'DELETE',
        }),

    deny: (id) =>
        apiRequest(`/api/applications/${id}/deny`, {
            method: 'PUT',
        }),
}