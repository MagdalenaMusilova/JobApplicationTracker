import { statusToProgress } from '../utils/matchScore'

function ApplicationDetailPage({
                                   profile,
                                   selectedApplication,
                                   selectedMatchScore,
                                   navigateBack,
                                   openCreate,
                                   openEdit,
                                   onDeleteApplication,
                                   onOpenStatusHistory,
                                   onAddStatus,
                                   onEditLastStatus,
                                   onDeleteLastStatus,
                               }) {
    const statusHistory = Array.isArray(selectedApplication.statusHistory)
        ? selectedApplication.statusHistory
        : []

    const latestStatus = statusHistory.length > 0 ? statusHistory[statusHistory.length - 1] : null
    const scheduledEvents = Array.isArray(selectedApplication.scheduledEvents)
        ? selectedApplication.scheduledEvents
        : []

    return (
        <section className="page">
            <div className="page-header">
                <div>
                    <h1>{selectedApplication.title}</h1>
                    <p>{selectedApplication.company}</p>
                </div>
                <div className="button-row">
                    <button type="button" className="secondary-btn" onClick={navigateBack}>
                        Back
                    </button>
                    <button type="button" className="secondary-btn danger" onClick={() => onDeleteApplication(selectedApplication.id)}>
                        Delete
                    </button>
                    <button type="button" className="secondary-btn" onClick={openEdit}>
                        Edit
                    </button>
                    <button type="button" className="primary-btn" onClick={openCreate}>
                        New Application
                    </button>
                </div>
            </div>

            <div className="detail-layout">
                <article className="card">
                    <div className="detail-hero">
                        <div>
                            <span className="badge">{selectedApplication.status}</span>
                            <h2>{selectedApplication.title}</h2>
                            <p className="muted">
                                {selectedApplication.company} · {selectedApplication.location}
                            </p>
                        </div>

                        <div className="application-progress">
                            <span>Progress</span>
                            <strong>{statusToProgress[selectedApplication.status] ?? 0}%</strong>
                            <div className="progress">
                                <div
                                    className="progress-bar"
                                    style={{ width: `${statusToProgress[selectedApplication.status] ?? 0}%` }}
                                />
                            </div>
                        </div>
                    </div>

                    <div className="info-list">
                        <div>
                            <span>Application ID</span>
                            <strong>{selectedApplication.id}</strong>
                        </div>
                        <div>
                            <span>Updated</span>
                            <strong>{selectedApplication.updatedAt}</strong>
                        </div>
                        <div>
                            <span>Current stage</span>
                            <strong>{selectedApplication.status}</strong>
                        </div>
                        <div>
                            <span>Next step</span>
                            <strong>{selectedApplication.nextStep}</strong>
                        </div>
                    </div>
                </article>

                <article className="card">
                    <h2>Automatic match</h2>
                    <p className="muted">
                        This score is computed automatically from the role description and your profile skills.
                    </p>

                    <div className="match-score">
                        <span>Candidate fit</span>
                        <strong>{selectedMatchScore}%</strong>
                    </div>

                    <div className="progress">
                        <div className="progress-bar match-bar" style={{ width: `${selectedMatchScore}%` }} />
                    </div>

                    <div className="tag-list">
                        {profile.skills.map((skill) => (
                            <span key={skill.id ?? skill.name} className="tag">
                {skill.name ?? String(skill)}
              </span>
                        ))}
                    </div>
                </article>
            </div>

            <div className="detail-layout">
                <article className="card">
                    <h2>Job description</h2>
                    <p>{selectedApplication.description || 'No job description provided.'}</p>

                    <h3>Notes</h3>
                    <p>{selectedApplication.notes || 'No notes provided.'}</p>
                </article>

                <article className="card">
                    <h2>Status</h2>
                    {latestStatus ? (
                        <button type="button" className="status-summary-btn" onClick={onOpenStatusHistory}>
                            <div>
                                <strong>{latestStatus.jaStatus ?? 'Applied'}</strong>
                                <p className="muted">{latestStatus.note || 'No note.'}</p>
                            </div>
                            <span className="badge">View history</span>
                        </button>
                    ) : (
                        <p className="muted">No status history yet.</p>
                    )}

                    <div className="button-row" style={{ marginTop: '1rem' }}>
                        <button type="button" className="secondary-btn" onClick={onAddStatus}>
                            Add status
                        </button>
                        {latestStatus && (
                            <>
                                <button type="button" className="secondary-btn" onClick={onEditLastStatus}>
                                    Edit last
                                </button>
                                <button type="button" className="secondary-btn danger" onClick={onDeleteLastStatus}>
                                    Remove last
                                </button>
                            </>
                        )}
                    </div>
                </article>
            </div>

            <div className="detail-layout">
                <article className="card">
                    <h2>Scheduled events</h2>
                    {scheduledEvents.length > 0 ? (
                        <div className="stack-list">
                            {scheduledEvents.map((event, index) => (
                                <div key={event.id ?? `${event.eventName}-${index}`} className="editable-item">
                                    <div className="editable-item-header">
                                        <div>
                                            <h3>{event.eventName ?? 'Event'}</h3>
                                            <p className="muted">
                                                {event.eventType ?? 'Other'} ·{' '}
                                                {event.eventDate ? new Date(event.eventDate).toLocaleString() : 'No date'}
                                            </p>
                                        </div>
                                    </div>
                                    <p>{event.note || 'No note.'}</p>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="muted">No scheduled events yet.</p>
                    )}
                </article>

                <article className="card">
                    <h2>Application actions</h2>
                    <div className="button-row">
                        <button type="button" className="secondary-btn" onClick={openEdit}>
                            Edit application
                        </button>
                        <button type="button" className="secondary-btn danger" onClick={() => onDeleteApplication(selectedApplication.id)}>
                            Delete application
                        </button>
                    </div>
                </article>
            </div>
        </section>
    )
}

export default ApplicationDetailPage