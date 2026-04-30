import { storage } from '../utils/storage'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

export async function apiRequest(path, options = {}) {
    const token = storage.getToken()

    const headers = {
        'Content-Type': 'application/json',
        ...(options.headers ?? {}),
    }

    // ✅ only attach token if it exists AND is not empty
    if (token && token !== 'null' && token !== 'undefined') {
        headers.Authorization = `Bearer ${token}`
    }

    const response = await fetch(`${API_BASE_URL}${path}`, {
        ...options,
        headers,
    })

    // 🚨 GLOBAL 401 HANDLING (IMPORTANT FIX BELOW)
    if (response.status === 401) {
        storage.clearToken()
        window.location.href = '/login'
        return
    }

    if (!response.ok) {
        const message = await response.text().catch(() => '')
        throw new Error(message || `Request failed (${response.status})`)
    }

    if (response.status === 204) return null

    return response.json()
}