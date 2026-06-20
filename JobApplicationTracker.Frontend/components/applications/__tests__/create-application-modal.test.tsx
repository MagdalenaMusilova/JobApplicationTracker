import React from 'react'
import { render, screen, waitFor } from '@/__tests__/test-utils'
import { CreateApplicationModal } from '../create-application-modal'
import userEvent from '@testing-library/user-event'
import { applicationService } from '@/services/application-service'

// Mock the application service instead of toast
jest.mock('@/services/application-service')
const mockedApplicationService = applicationService as jest.Mocked<typeof applicationService>

// Mock sonner toast
jest.mock('sonner', () => ({
  toast: {
    success: jest.fn(),
    error: jest.fn(),
  },
}))

describe('CreateApplicationModal', () => {
  const defaultProps = {
    open: true,
    onOpenChange: jest.fn(),
    onSuccess: jest.fn(),
  }

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it('should render modal when open is true', () => {
    render(<CreateApplicationModal {...defaultProps} />)

    expect(screen.getByText('Create Job Application')).toBeInTheDocument()
    expect(
      screen.getByText('Track a new opportunity and optionally schedule an event.')
    ).toBeInTheDocument()
  })

  it('should not render modal when open is false', () => {
    render(<CreateApplicationModal {...defaultProps} open={false} />)

    expect(screen.queryByText('Create Job Application')).not.toBeInTheDocument()
  })

  it('should have all required form fields', () => {
    render(<CreateApplicationModal {...defaultProps} />)

    expect(screen.getByLabelText(/company/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/^position/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/application notes/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/job description/i)).toBeInTheDocument()
    expect(screen.getByText(/status type/i)).toBeInTheDocument()
  })

  it('should fill out form and submit successfully', async () => {
    const user = userEvent.setup()

    // Mock successful creation
    mockedApplicationService.create.mockResolvedValueOnce({
      id: 'new-id',
      company: 'Test Company',
      position: 'Software Engineer',
    } as any)

    render(<CreateApplicationModal {...defaultProps} />)

    // Fill in required fields
    const companyInput = screen.getByLabelText(/company/i)
    const positionInput = screen.getByLabelText(/^position/i)

    await user.clear(companyInput)
    await user.type(companyInput, 'Test Company')
    await user.clear(positionInput)
    await user.type(positionInput, 'Software Engineer')

    // Submit form
    const submitButton = screen.getByRole('button', {
      name: /create application/i,
    })
    await user.click(submitButton)

    // Wait for the mutation to complete
    await waitFor(() => {
      expect(mockedApplicationService.create).toHaveBeenCalled()
      expect(defaultProps.onSuccess).toHaveBeenCalled()
    }, { timeout: 3000 })
  })

  it('should show event form when schedule event checkbox is checked', async () => {
    const user = userEvent.setup()
    render(<CreateApplicationModal {...defaultProps} />)

    // Initially event fields should not be visible
    expect(screen.queryByLabelText(/event name/i)).not.toBeInTheDocument()

    // Check the schedule event checkbox
    const scheduleEventCheckbox = screen.getByRole('checkbox')
    await user.click(scheduleEventCheckbox)

    // Now event fields should be visible
    await waitFor(() => {
      expect(screen.getByLabelText(/event name/i)).toBeInTheDocument()
    })

    // Check for date input instead of label (Radix Select doesn't use standard labels)
    expect(screen.getByLabelText(/date & time/i)).toBeInTheDocument()

    // Check that "Event Type" text exists (label for Radix Select)
    expect(screen.getByText(/event type/i)).toBeInTheDocument()
  })

  it('should hide event form when schedule event checkbox is unchecked', async () => {
    const user = userEvent.setup()
    render(<CreateApplicationModal {...defaultProps} />)

    // Check the checkbox first
    const scheduleEventCheckbox = screen.getByRole('checkbox')
    await user.click(scheduleEventCheckbox)

    await waitFor(() => {
      expect(screen.getByLabelText(/event name/i)).toBeInTheDocument()
    })

    // Uncheck the checkbox
    await user.click(scheduleEventCheckbox)

    await waitFor(() => {
      expect(screen.queryByLabelText(/event name/i)).not.toBeInTheDocument()
    })
  })

  it('should call onOpenChange when cancel button is clicked', async () => {
    const user = userEvent.setup()
    render(<CreateApplicationModal {...defaultProps} />)

    const cancelButton = screen.getByRole('button', { name: /cancel/i })
    await user.click(cancelButton)

    expect(defaultProps.onOpenChange).toHaveBeenCalledWith(false)
  })

  it('should prefill form data when provided', () => {
    const prefillData = {
      company: 'Prefilled Company',
      position: 'Prefilled Position',
      note: 'Prefilled note',
    }

    render(<CreateApplicationModal {...defaultProps} prefillData={prefillData} />)

    expect(screen.getByDisplayValue('Prefilled Company')).toBeInTheDocument()
    expect(screen.getByDisplayValue('Prefilled Position')).toBeInTheDocument()
    expect(screen.getByDisplayValue('Prefilled note')).toBeInTheDocument()
  })

  it('should disable submit button while submitting', async () => {
    const user = userEvent.setup()

    // Mock a delayed response
    mockedApplicationService.create.mockImplementation(
      () => new Promise((resolve) => setTimeout(() => resolve({
        id: 'new-id',
        company: 'Test',
        position: 'Dev',
      } as any), 100))
    )

    render(<CreateApplicationModal {...defaultProps} />)

    const companyInput = screen.getByLabelText(/company/i)
    const positionInput = screen.getByLabelText(/^position/i)

    await user.type(companyInput, 'Test Company')
    await user.type(positionInput, 'Software Engineer')

    const submitButton = screen.getByRole('button', {
      name: /create application/i,
    })

    await user.click(submitButton)

    // Button should be disabled during submission
    await waitFor(() => {
      expect(submitButton).toBeDisabled()
    })
  })
})
