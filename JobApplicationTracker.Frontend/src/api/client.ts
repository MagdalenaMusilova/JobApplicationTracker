import { storage } from '../utils/storage'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL as string

type ApiRequestOptions = Omit<RequestInit, 'headers'> & {
    headers?: HeadersInit
}

export async function apiRequest<T = unknown>(
    path: string,
    options: ApiRequestOptions = {}
): Promise<T | null> {
    const token = storage.getToken()

    const headers: Record<string, string> = {
        'Content-Type': 'application/json',
        ...(options.headers as Record<string, string>),
    }

    // only attach token if it exists AND is not empty
    if (token && token !== 'null' && token !== 'undefined') {
        headers.Authorization = `Bearer ${token}`
    }

    const response = await fetch(`${API_BASE_URL}${path}`, {
        ...options,
        headers,
    })

    if (!response.ok) {
        const message = await response.text().catch(() => '')
        throw new Error(message || `Request failed (${response.status})`)
    }

    if (response.status === 204) return null

    return response.json() as Promise<T>
}