import { renderHook, act } from '@testing-library/react'
import { useToast } from '../use-toast'

describe('useToast', () => {
  it('should initialize with empty toasts', () => {
    const { result } = renderHook(() => useToast())

    expect(result.current.toasts).toEqual([])
  })

  it('should add a toast', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.toast({
        title: 'Test Toast',
        description: 'This is a test',
      })
    })

    expect(result.current.toasts).toHaveLength(1)
    expect(result.current.toasts[0].title).toBe('Test Toast')
    expect(result.current.toasts[0].description).toBe('This is a test')
  })

  it('should dismiss a specific toast', () => {
    const { result } = renderHook(() => useToast())

    let toastId: string

    act(() => {
      const toast = result.current.toast({
        title: 'Test Toast',
      })
      toastId = toast.id
    })

    expect(result.current.toasts).toHaveLength(1)

    act(() => {
      result.current.dismiss(toastId!)
    })

    expect(result.current.toasts[0].open).toBe(false)
  })

  it('should respect toast limit', () => {
    const { result } = renderHook(() => useToast())

    act(() => {
      result.current.toast({ title: 'Toast 1' })
      result.current.toast({ title: 'Toast 2' })
    })

    // Toast limit is 1, so only the most recent toast should be present
    expect(result.current.toasts).toHaveLength(1)
    expect(result.current.toasts[0].title).toBe('Toast 2')
  })

  it('should update a toast', () => {
    const { result } = renderHook(() => useToast())

    let updateFn: (props: any) => void

    act(() => {
      const toast = result.current.toast({
        title: 'Original Title',
      })
      updateFn = toast.update
    })

    act(() => {
      updateFn!({
        title: 'Updated Title',
      })
    })

    expect(result.current.toasts[0].title).toBe('Updated Title')
  })
})
