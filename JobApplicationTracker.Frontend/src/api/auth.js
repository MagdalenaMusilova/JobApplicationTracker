import { apiRequest } from './client'

export const AuthAPI = {
    signup: (data) =>
        apiRequest('/api/users', {
            method: 'POST',
            body: JSON.stringify(data),
        }),

    signin: (data) =>
        apiRequest('/api/auth/signin', {
            method: 'POST',
            body: JSON.stringify(data),
        }),
}