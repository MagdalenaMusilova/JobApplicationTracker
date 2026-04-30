import { apiRequest } from './client'

export const ApplicationsAPI = {
    getAll: () => apiRequest('/api/applications'),

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