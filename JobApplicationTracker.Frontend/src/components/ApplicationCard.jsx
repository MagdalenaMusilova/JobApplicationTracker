import '../styles/ApplicationCard.css'

function ApplicationCard({ app, onClick, onDelete, onRejected }) {
    const closestEvent = app.event
    const lastStatus = app.statusHistory.at(-1)
    
    return (
        <article className="app-card compact" onClick={onClick}>
            <div className="app-card-header">
                <p className="app-company">{app.company}</p>
                <h2 className="app-title">{app.position}</h2>
            </div>

            <div className="app-card-meta">
                <div className="meta-row">
                    <span className="meta-label">Status:</span>
                    <span className={`status-text status-${lastStatus.jaStatus}`}>
                        {lastStatus.jaStatus}
                    </span>
                </div>

                {closestEvent && (
                    <div className="meta-row">
                        <span className="meta-label">Event:</span>
                        <span className="meta-value">
                            {closestEvent.name ?? 'Event'}
                            {closestEvent.eventDate
                                ? ` · ${new Date(closestEvent.eventDate).toLocaleDateString()}`
                                : ''}
                        </span>
                    </div>
                )}
            </div>

            <div className="app-card-actions compact-actions">
                <button
                    type="button"
                    className="secondary-btn"
                    onClick={(e) => {
                        e.stopPropagation()
                        onClick?.()
                    }}
                >
                    View
                </button>

                <button
                    type="button"
                    className="secondary-btn"
                    onClick={(e) => {
                        e.stopPropagation()
                        onRejected?.(app.id)
                    }}
                >
                    Mark as rejected
                </button>

                <button
                    type="button"
                    className="secondary-btn danger"
                    onClick={(e) => {
                        e.stopPropagation()
                        onDelete?.(app.id)
                    }}
                >
                    Delete
                </button>
            </div>
        </article>
    )
}

export default ApplicationCard