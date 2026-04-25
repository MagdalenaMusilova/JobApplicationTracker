import { getStatusIndex, statusToProgress } from '../utils/matchScore'

function ApplicationCard({ app, onClick, onDelete, statusSteps }) {
    const stepIndex = getStatusIndex(app.status)
    const progress = statusToProgress[app.status] ?? 0
    const closestEvent = app.closestEvent

    return (
        <article className="card clickable application-card" onClick={onClick}>
            <div className="application-card-header">
                <div className="application-card-heading">
                    <p className="eyebrow application-card-company">{app.company}</p>
                    <h2 className="application-card-title">{app.title}</h2>
                </div>
            </div>

            <div className="application-card-grid">
                <div className="application-card-meta-block">
                    <span className="info-label">Current status</span>
                    <span className="application-status-pill">{app.status}</span>
                </div>

                {closestEvent ? (
                    <div className="application-card-meta-block">
                        <span className="info-label">Upcoming event</span>
                        <strong className="application-card-event">
                            {closestEvent.eventName ?? 'Event'}
                            {closestEvent.eventDate ? ` · ${new Date(closestEvent.eventDate).toLocaleDateString()}` : ''}
                        </strong>
                    </div>
                ) : null}
            </div>

            <div className="application-progress-block">
                <div className="application-progress-top">
                    <span>Status progress</span>
                    <strong>
                        {stepIndex + 1}/{statusSteps.length}
                    </strong>
                </div>

                <div className="progress application-status-progress" title={`Status progress: ${app.status}`}>
                    <div
                        className="progress-bar application-status-progress-bar"
                        style={{ width: `${progress}%` }}
                    />
                </div>
            </div>

            <div className="button-row" style={{ marginTop: '0.75rem' }}>
                <button
                    type="button"
                    className="secondary-btn"
                    onClick={onClick}
                >
                    View
                </button>
                <button
                    type="button"
                    className="secondary-btn danger"
                    onClick={(e) => {
                        e.stopPropagation()
                        onDelete()
                    }}
                >
                    Delete
                </button>
            </div>
        </article>
    )
}

export default ApplicationCard