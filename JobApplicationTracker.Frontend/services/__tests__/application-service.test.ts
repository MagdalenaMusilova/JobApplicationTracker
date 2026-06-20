import { applicationService } from '../application-service'
import { CreateJobApplicationDto } from '@/types/JAObjects/JobApplications/CreateJobApplicationDto'
import httpClient from '@/lib/http-client'

// Mock the http client
jest.mock('@/lib/http-client')
const mockedHttpClient = httpClient as jest.Mocked<typeof httpClient>

describe('applicationService', () => {
  afterEach(() => {
    jest.clearAllMocks()
  })

  describe('getAll', () => {
    it('should fetch all applications', async () => {
      const mockData = [
        {
          id: 'app-1',
          company: 'Test Company',
          position: 'Software Engineer',
          status: 'Applied',
        },
      ]

      mockedHttpClient.get.mockResolvedValueOnce({ data: mockData })

      const applications = await applicationService.getAll()

      expect(mockedHttpClient.get).toHaveBeenCalled()
      expect(applications).toEqual(mockData)
      expect(Array.isArray(applications)).toBe(true)
      expect(applications[0]).toHaveProperty('id')
      expect(applications[0]).toHaveProperty('company')
    })
  })

  describe('create', () => {
    it('should create a new application', async () => {
      const newApplication: CreateJobApplicationDto = {
        company: 'New Company',
        position: 'Developer',
        note: 'Test note',
        jobDescription: 'Test description',
        initialStatus: {
          jobApplicationId: '1',
          statusType: 0,
          note: '',
        },
        jaEvent: null,
      }

      const mockResponse = {
        id: 'new-app-id',
        company: 'New Company',
        position: 'Developer',
      }

      mockedHttpClient.post.mockResolvedValueOnce({ data: mockResponse })

      const result = await applicationService.create(newApplication)

      expect(mockedHttpClient.post).toHaveBeenCalledWith(
        expect.anything(),
        newApplication
      )
      expect(result).toEqual(mockResponse)
      expect(result.company).toBe('New Company')
    })
  })

  describe('getById', () => {
    it('should fetch a single application by id', async () => {
      const applicationId = 'app-1'
      const mockData = {
        id: applicationId,
        company: 'Test Company',
        position: 'Software Engineer',
      }

      mockedHttpClient.get.mockResolvedValueOnce({ data: mockData })

      const application = await applicationService.getById(applicationId)

      expect(mockedHttpClient.get).toHaveBeenCalledWith(
        expect.stringContaining(applicationId)
      )
      expect(application).toEqual(mockData)
      expect(application.id).toBe(applicationId)
    })
  })

  describe('update', () => {
    it('should update an application', async () => {
      const applicationId = 'app-1'
      const updateData = {
        company: 'Updated Company',
        position: 'Senior Developer',
      }

      const mockResponse = {
        id: applicationId,
        ...updateData,
      }

      mockedHttpClient.put.mockResolvedValueOnce({ data: mockResponse })

      const result = await applicationService.update(applicationId, updateData)

      expect(mockedHttpClient.put).toHaveBeenCalledWith(
        expect.stringContaining(applicationId),
        updateData
      )
      expect(result).toEqual(mockResponse)
    })
  })

  describe('delete', () => {
    it('should delete an application', async () => {
      const applicationId = 'app-1'

      mockedHttpClient.delete.mockResolvedValueOnce({ data: null })

      await applicationService.delete(applicationId)

      expect(mockedHttpClient.delete).toHaveBeenCalledWith(
        expect.stringContaining(applicationId)
      )
    })
  })

  describe('deny', () => {
    it('should deny an application', async () => {
      const applicationId = 'app-1'
      const mockResponse = {
        id: applicationId,
        status: 'Denied',
      }

      mockedHttpClient.put.mockResolvedValueOnce({ data: mockResponse })

      const result = await applicationService.deny(applicationId)

      expect(mockedHttpClient.put).toHaveBeenCalledWith(
        expect.stringContaining(applicationId)
      )
      expect(result).toEqual(mockResponse)
    })
  })
})
