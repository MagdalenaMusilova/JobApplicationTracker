import { statusToProgress } from '../utils/matchScore'

function ApplicationDetailPage({
                                   profile,
                                   selectedApplication,
                                   selectedMatchScore,
                                   navigateBack,
                                   openCreate,
                               }) {
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
                            <span key={skill} className="tag">
                    {skill}
                  </span>
                        ))}
                    </div>
                </article>
            </div>

            <div className="detail-layout">
                <article className="card">
                    <h2>Role description</h2>
                    <p>{selectedApplication.description}</p>

                    <h3>Key requirements</h3>
                    <div className="tag-list">
                        {(selectedApplication.requirements || []).map((req) => (
                            <span key={req} className="tag">
                    {req}
                  </span>
                        ))}
                    </div>
                </article>

                <article className="card">
                    <h2>Recruitment notes</h2>
                    <p>{selectedApplication.notes}</p>

                    <div className="timeline">
                        <div className="timeline-item">
                            <span>Applied</span>
                            <strong>Sent application</strong>
                        </div>
                        <div className="timeline-item">
                            <span>Tracking</span>
                            <strong>{selectedApplication.nextStep}</strong>
                        </div>
                    </div>
                </article>
            </div>
        </section>
    )
}

export default ApplicationDetailPage