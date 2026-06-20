import { http, HttpResponse } from 'msw'
import { mockLoginResponse, mockUser } from './auth-mock'

const BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000'

export const handlers = [
  // Auth endpoints
  http.post(`${BASE_URL}/api/auth/login`, () => {
    return HttpResponse.json(mockLoginResponse)
  }),

  http.post(`${BASE_URL}/api/auth/register`, () => {
    return HttpResponse.json(mockLoginResponse)
  }),

  http.post(`${BASE_URL}/api/auth/refresh`, () => {
    return HttpResponse.json(mockLoginResponse)
  }),

  http.post(`${BASE_URL}/api/auth/logout`, () => {
    return new HttpResponse(null, { status: 200 })
  }),

  // Application endpoints
  http.get(`${BASE_URL}/api/applications`, () => {
    return HttpResponse.json([
      {
        id: 'app-1',
        company: 'Test Company',
        position: 'Software Engineer',
        status: 'Applied',
        appliedDate: '2024-01-01',
      },
    ])
  }),

  http.post(`${BASE_URL}/api/applications`, async ({ request }) => {
    const body = await request.json()
    return HttpResponse.json({
      id: 'new-app-id',
      ...body,
      appliedDate: new Date().toISOString(),
    })
  }),

  http.get(`${BASE_URL}/api/applications/:id`, ({ params }) => {
    return HttpResponse.json({
      id: params.id,
      company: 'Test Company',
      position: 'Software Engineer',
      status: 'Applied',
      appliedDate: '2024-01-01',
    })
  }),

  http.put(`${BASE_URL}/api/applications/:id`, async ({ request, params }) => {
    const body = await request.json()
    return HttpResponse.json({
      id: params.id,
      ...body,
    })
  }),

  http.delete(`${BASE_URL}/api/applications/:id`, () => {
    return new HttpResponse(null, { status: 204 })
  }),

  // Dashboard endpoints
  http.get(`${BASE_URL}/api/dashboard/stats`, () => {
    return HttpResponse.json({
      totalApplications: 10,
      activeApplications: 5,
      interviews: 3,
      offers: 1,
    })
  }),
]
