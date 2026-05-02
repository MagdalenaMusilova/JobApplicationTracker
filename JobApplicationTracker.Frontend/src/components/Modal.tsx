import '../styles/Modal.css'

function Modal({ title, subtitle, children, onClose, footer }) {
    return (
        <div className="modal-backdrop" onClick={onClose}>
            <div className="modal card modal-shell" onClick={(e) => e.stopPropagation()}>

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