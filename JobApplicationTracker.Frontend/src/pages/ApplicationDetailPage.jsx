import { useEffect, useState } from 'react'
import CreateEventModal from '../components/CreateEventModal'
import '../styles/ApplicationDetailPage.css'

function ApplicationDetailPage({
                                   selectedApplication,
                                   openCreate,
                                   onDeleteApplication,
                                   onOpenStatusHistory,
                                   onAddStatus,
                                   onEditLastStatus,
                                   onDeleteLastStatus,
                                   onSaveApplication,
                               }) {
    const [isCreateEventModalOpen, setIsCreateEventModalOpen] = useState(false)
    const [isEditingApplication, setIsEditingApplication] = useState(false)
    const [draft, setDraft] = useState(selectedApplication)

    useEffect(() => {
        setDraft(selectedApplication)
        setIsEditingApplication(false)
    }, [selectedApplication])

    const statusHistory = Array.isArray(selectedApplication.statusHistory)
        ? selectedApplication.statusHistory
        : []

    const latestStatus =
        statusHistory.length > 0
            ? statusHistory[statusHistory.length - 1]
            : null

    const scheduledEvents = Array.isArray(selectedApplication.scheduledEvents)
        ? selectedApplication.scheduledEvents
        : []

    const handleSave = () => {
        onSaveApplication?.(draft)
        setIsEditingApplication(false)
    }

    const handleCancel = () => {
        setDraft(selectedApplication)
        setIsEditingApplication(false)
    }

    return (
        <section className="page">

            {/* HEADER */}
            <div className="page-header">
                <div>
                    <h1>{selectedApplication.company}</h1>
                    <p>{selectedApplication.title}</p>
                </div>

                <div className="button-row">
                    <button
                        className="secondary-btn danger"
                        onClick={() => onDeleteApplication(selectedApplication.id)}
                    >
                        Delete
                    </button>

                    <button className="primary-btn" onClick={openCreate}>
                        New Application
                    </button>
                </div>
            </div>

            {/* APPLICATION CARD */}
            <div className="detail-layout">
                <article className="card">

                    {/* HEADER */}
                    <div className="section-header">
                        <h2>Application</h2>

                        {!isEditingApplication ? (
                            <button
                                className="secondary-btn"
                                onClick={() => setIsEditingApplication(true)}
                            >
                                Edit
                            </button>
                        ) : (
                            <div className="button-row">
                                <button className="secondary-btn" onClick={handleCancel}>
                                    Cancel
                                </button>
                                <button className="primary-btn" onClick={handleSave}>
                                    Save
                                </button>
                            </div>
                        )}
                    </div>

                    {/* TITLE + COMPANY */}
                    <div style={{ marginTop: 12 }}>
                        {!isEditingApplication ? (
                            <>
                                <p className="muted">{selectedApplication.company}</p>
                                <h2>{selectedApplication.title}</h2>
                            </>
                        ) : (
                            <>
                                <label className="field" style={{ marginTop: 10 }}>
                                    <span>Company</span>
                                    <input
                                        value={draft.company || ''}
                                        onChange={(e) =>
                                            setDraft({ ...draft, company: e.target.value })
                                        }
                                    />
                                </label>
                                
                                <label className="field">
                                    <span>Position</span>
                                    <input
                                        value={draft.title || ''}
                                        onChange={(e) =>
                                            setDraft({ ...draft, title: e.target.value })
                                        }
                                    />
                                </label>
                            </>
                        )}
                    </div>

                    {/* VIEW ONLY INFO */}
                    <div className="info-list">
                        <div>
                            <span>Status</span>
                            <strong>{selectedApplication.status}</strong>
                        </div>

                        <div>
                            <span>Created</span>
                            <strong>{selectedApplication.createdAt || '—'}</strong>
                        </div>

                        <div>
                            <span>Updated</span>
                            <strong>{selectedApplication.updatedAt || '—'}</strong>
                        </div>
                    </div>

                    {/* DESCRIPTION */}
                    <h3>Job description</h3>

                    {!isEditingApplication ? (
                        <p>{selectedApplication.description || 'No description provided.'}</p>
                    ) : (
                        <textarea
                            className="edit-input"
                            rows="2"
                            value={draft.description || ''}
                            onChange={(e) =>
                                setDraft({ ...draft, description: e.target.value })
                            }
                        />
                    )}

                    {/* NOTES */}
                    <h3>Notes</h3>

                    {!isEditingApplication ? (
                        <p>{selectedApplication.notes || 'No notes provided.'}</p>
                    ) : (
                        <textarea
                            className="edit-input"
                            rows="2"
                            value={draft.notes || ''}
                            onChange={(e) =>
                                setDraft({ ...draft, notes: e.target.value })
                            }
                        />
                    )}
                </article>
            </div>

            {/* STATUS */}
            <div className="detail-layout">
                <article className="card">
                    <h2>Status</h2>

                    {latestStatus ? (
                        <button
                            className="status-summary-btn"
                            onClick={onOpenStatusHistory}
                        >
                            <div>
                                <strong>{latestStatus.jaStatus ?? 'Applied'}</strong>
                                <p className="muted">
                                    {latestStatus.note || 'No note.'}
                                </p>
                            </div>
                            <span className="badge">View</span>
                        </button>
                    ) : (
                        <p className="muted">No status history yet.</p>
                    )}

                    <div className="button-row">
                        <button className="secondary-btn" onClick={onAddStatus}>
                            Add
                        </button>

                        {latestStatus && (
                            <>
                                <button className="secondary-btn" onClick={onEditLastStatus}>
                                    Edit last
                                </button>
                                <button
                                    className="secondary-btn danger"
                                    onClick={onDeleteLastStatus}
                                >
                                    Remove last
                                </button>
                            </>
                        )}
                    </div>
                </article>
            </div>

            {/* EVENTS */}
            <div className="detail-layout">
                <article className="card">
                    <h2>Scheduled events</h2>

                    {scheduledEvents.length > 0 ? (
                        <div className="stack-list">
                            {scheduledEvents.map((event, index) => (
                                <div key={event.id ?? index} className="editable-item">
                                    <h3>{event.eventName}</h3>
                                    <p className="muted">
                                        {event.eventType} ·{' '}
                                        {event.eventDate
                                            ? new Date(event.eventDate).toLocaleString()
                                            : 'No date'}
                                    </p>
                                    <p>{event.note || 'No note.'}</p>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="muted">No scheduled events yet.</p>
                    )}

                    <button
                        className="primary-btn"
                        onClick={() => setIsCreateEventModalOpen(true)}
                        style={{ marginTop: 12 }}
                    >
                        Add event
                    </button>
                </article>
            </div>

            {/* MATCH */}
            <div className="detail-layout">
                <article className="card">
                    <h2>Match status</h2>

                    <textarea
                        className="edit-input"
                        rows="3"
                        placeholder="Paste job ad..."
                    />

                    <div className="button-row">
                        <button className="secondary-btn">Match</button>
                        <button className="secondary-btn danger">
                            Clear
                        </button>
                    </div>
                </article>
            </div>

            {/* MODAL */}
            {isCreateEventModalOpen && (
                <CreateEventModal onClose={() => setIsCreateEventModalOpen(false)} />
            )}
        </section>
    )
}

export default ApplicationDetailPage