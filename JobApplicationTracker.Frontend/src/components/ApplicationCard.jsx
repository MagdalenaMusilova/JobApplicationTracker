import { getStatusIndex, statusToProgress } from '../utils/matchScore'

function ApplicationCard({ app, onClick, statusSteps }) {
    const stepIndex = getStatusIndex(app.status)
    const progress = statusToProgress[app.status] ?? 0

    return (
        <article className="card clickable" onClick={onClick}>
            <div className="card-top">
                <div>
                    <h2>{app.title}</h2>
                    <p>{app.company}</p>
                </div>
                <span className="badge">{app.status}</span>
            </div>

            <div className="meta">
                <span>{app.location}</span>
                <span>{app.updatedAt}</span>
            </div>

            <div className="fit-row">
                <span>Application status</span>
                <strong>
                    {stepIndex + 1}/{statusSteps.length}
                </strong>
            </div>

            <div className="progress" title={`Status progress: ${app.status}`}>
                <div className="progress-bar" style={{ width: `${progress}%` }} />
            </div>

            <p className="muted">{app.description}</p>
        </article>
    )
}

export default ApplicationCard