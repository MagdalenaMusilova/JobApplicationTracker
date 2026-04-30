import { createContext, useContext, useState } from 'react'
import Modal from '../components/Modal'

const ConfirmContext = createContext()

export function ConfirmProvider({ children }) {
    const [state, setState] = useState({
        open: false,
        message: '',
        resolve: null,
    })

    const requestConfirm = (message) => {
        return new Promise((resolve) => {
            setState({
                open: true,
                message,
                resolve,
            })
        })
    }

    const handleConfirm = () => {
        state.resolve(true)
        setState({ open: false, message: '', resolve: null })
    }

    const handleCancel = () => {
        state.resolve(false)
        setState({ open: false, message: '', resolve: null })
    }

    return (
        <ConfirmContext.Provider value={{ confirm: requestConfirm }}>
            {children}

            {state.open && (
                <Modal
                    title="Confirm action"
                    subtitle={state.message}
                    onClose={handleCancel}
                >
                    <div className="button-row modal-actions">
                        <button className="secondary-btn" onClick={handleCancel}>
                            Cancel
                        </button>
                        <button className="primary-btn danger" onClick={handleConfirm}>
                            Confirm
                        </button>
                    </div>
                </Modal>
            )}
        </ConfirmContext.Provider>
    )
}

export function useConfirm() {
    return useContext(ConfirmContext)
}