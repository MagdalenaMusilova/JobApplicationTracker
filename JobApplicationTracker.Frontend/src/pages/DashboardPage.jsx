import StatCard from '../components/StatCard'
import { avaibleStatuses } from '../utils/matchScore'

function DashboardPage({
                           profile,
                           applications,
                           todoItems
                       }) {
    return (
        <section className="page dashboard-page">
            {/*<div className="dashboard-hero card">
                <div className="dashboard-hero-copy">
                    <span className="eyebrow">Dashboard overview</span>
                    <h1>Hi, {profile.account.username.split(' ')[0]} — here’s the fast view.</h1>
                    <p>
                        Everything important is grouped so you can spot progress, priorities, and recent changes fast.
                    </p>
                </div>

                <div className="dashboard-hero-card">
                    <div className="hero-stat">
                        <span>Best match</span>
                        <strong>{dashboardStats.bestFit}%</strong>
                    </div>
                    <div className="progress">
                        <div className="progress-bar match-bar" style={{ width: `${dashboardStats.bestFit}%` }} />
                    </div>
                    <div className="hero-chip-row">
                        <span className="hero-chip">{dashboardStats.activeApplications} active</span>
                        <span className="hero-chip">{dashboardStats.interviewCount} interviews</span>
                        <span className="hero-chip">{dashboardStats.urgentTasks} urgent tasks</span>
                    </div>
                </div>
            </div>

            <div className="stats-grid">
                <StatCard kicker="Tracked" value={applications.length} label="applications total" />
                <StatCard kicker="Active" value={dashboardStats.activeApplications} label="still in progress" />
                <StatCard kicker="Interviews" value={dashboardStats.interviewCount} label="up next" />
                <StatCard kicker="Urgent" value={dashboardStats.urgentTasks} label="high-priority tasks" />
            </div>

            <div className="dashboard-grid dashboard-grid-main">
                <article className="card dashboard-focus-card">
                    <div className="card-top">
                        <div>
                            <h2>What to do next</h2>
                            <p>Top priority items that need attention first.</p>
                        </div>
                    </div>

                    <div className="focus-list">
                        {urgentActionItems.length > 0 ? urgentActionItems.map((app) => (
                            <button
                                key={app.id}
                                type="button"
                                className="focus-item"
                                onClick={() => navigateToApplication(app.id)}
                            >
                                <div className="focus-left">
                                    <span className="badge danger">{app.priority}</span>
                                    <div>
                                        <strong>{app.company}</strong>
                                        <p>{app.nextStep}</p>
                                    </div>
                                </div>
                                <div className="focus-right">
                                    <span>{app.dueDate}</span>
                                    <span className="recent-arrow">›</span>
                                </div>
                            </button>
                        )) : (
                            <div className="empty-state">
                                <strong>All caught up</strong>
                                <p>No high-priority items right now.</p>
                            </div>
                        )}
                    </div>
                </article>

                <article className="card">
                    <div className="card-top">
                        <div>
                            <h2>Pipeline snapshot</h2>
                            <p>Quick distribution across stages.</p>
                        </div>
                    </div>

                    <div className="pipeline-grid compact">
                        {statusSteps.map((step) => (
                            <div key={step} className="pipeline-item">
                                <span>{step}</span>

                                <strong>{pipelineSummary[step] ?? 0}</strong>

                            </div>
                        ))}
                    </div>
                </article>
            </div>

            <div className="dashboard-grid dashboard-grid-secondary">
                <article className="card">
                    <div className="card-top">
                        <div>
                            <h2>Recent updates</h2>
                            <p>Your last touched applications.</p>
                        </div>
                    </div>

                    <div className="recent-list">
                        {dashboardRecentApplications.map((app) => (
                            <button
                                key={app.id}
                                type="button"
                                className="recent-item"
                                onClick={() => navigateToApplication(app.id)}
                            >
                                <div className="recent-main">
                                    <strong>{app.company}</strong>
                                    <span>{app.title} · {app.status}</span>
                                </div>
                                <div className="recent-meta">
                                    <span>{app.updatedAt}</span>
                                    <span className="recent-arrow">›</span>
                                </div>
                            </button>
                        ))}
                    </div>
                </article>

                <article className="card">
                    <div className="card-top">
                        <div>
                            <h2>Quick profile check</h2>
                            <p>Your current profile details in one glance.</p>
                        </div>
                    </div>

                    <div className="summary-stack">
                        <div className="summary-line">
                            <span>Role</span>
                            <strong>{profile.role}</strong>
                        </div>
                        <div className="summary-line">
                            <span>Location</span>
                            <strong>{profile.location}</strong>
                        </div>
                        <div className="summary-line">
                            <span>CV</span>
                            <strong>{profile.cv}</strong>
                        </div>
                        <div className="summary-line">
                            <span>Focus</span>
                            <strong>Backend / API roles</strong>
                        </div>
                    </div>
                </article>
            </div>*/}
        </section>
    )
}

export default DashboardPage