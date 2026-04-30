import { apiRequest } from './client'

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
}