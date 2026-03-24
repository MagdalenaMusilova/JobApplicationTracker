function Modal({ title, subtitle, children, onClose }) {
    return (
        <div className="modal-backdrop" onClick={onClose}>
            <div className="modal card" onClick={(e) => e.stopPropagation()}>
                <div className="page-header compact">
                    <div>
                        <h2>{title}</h2>
                        <p>{subtitle}</p>
                    </div>
                </div>
                {children}
            </div>
        </div>
    )
}

export default Modal