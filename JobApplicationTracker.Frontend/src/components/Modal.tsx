import '../styles/Modal.css'
import { ReactNode, MouseEvent } from 'react'

interface ModalProps {
    title: string
    subtitle?: string
    children: ReactNode
    onClose: () => void
        footer?: ReactNode
}

function Modal({ title, subtitle, children, onClose, footer }: ModalProps) {
    const handleBackdropClick = () => {
        onClose()
    }

    const handleModalClick = (e: MouseEvent<HTMLDivElement>) => {
        e.stopPropagation()
    }

    return (
        <div className="modal-backdrop" onClick={handleBackdropClick}>
            <div className="modal card modal-shell" onClick={handleModalClick}>

                <div className="modal-header">
                    <div>
                        <h2>{title}</h2>
                        {subtitle && <p>{subtitle}</p>}
                    </div>
                </div>

                <div className="modal-body">
                    {children}
                </div>

                {footer && (
                    <div className="modal-actions">
                        {footer}
                    </div>
                )}

            </div>
        </div>
    )
}

export default Modal