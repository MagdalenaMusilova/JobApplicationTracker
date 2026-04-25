import { useMemo, useState } from 'react'
import ApplicationCard from '../components/ApplicationCard'

function ApplicationsPage({
                              applications,
                              statusSteps,
                              navigateToApplication,
                              setCreateOpen,
                              onDeleteApplication,
                          }) {
    const [filters, setFilters] = useState('all')
    const [searchText, setSearchText] = useState('')

    const filteredApplications = useMemo(() => {
        const normalizedSearch = searchText.trim().toLowerCase()

        return applications.filter((app) => {
            const matchesStatus = filters === 'all' || app.status === filters
            const matchesSearch =
                normalizedSearch.length === 0 ||
                `${app.company} ${app.title} ${app.description} ${app.notes} ${app.status}`
                    .toLowerCase()
                    .includes(normalizedSearch)

            return matchesStatus && matchesSearch
        })
    }, [applications, filters, searchText])

    const applicationCount = applications.length
    const visibleCount = filteredApplications.length
    const activeFilterLabel = filters === 'all' ? 'All statuses' : filters

    return (
        <section className="page applications-page">
            <div className="page-header applications-hero">
                <div className="applications-hero-copy">
                    <span className="eyebrow">Application tracker</span>
                    <h1>Applications</h1>
                    <p>Browse, search, and filter all tracked applications in one clean view.</p>
                </div>

                <div className="applications-hero-actions">
                    <div className="applications-hero-stats">
                        <div className="applications-hero-stat card">
                            <span>Total</span>
                            <strong>{applicationCount}</strong>
                        </div>
                        <div className="applications-hero-stat card">
                            <span>Showing</span>
                            <strong>{visibleCount}</strong>
                        </div>
                        <div className="applications-hero-stat card">
                            <span>Filter</span>
                            <strong>{activeFilterLabel}</strong>
                        </div>
                    </div>

                    <button type="button" className="primary-btn" onClick={() => setCreateOpen(true)}>
                        + New Application
                    </button>
                </div>
            </div>

            <div className="card applications-toolbar">
                <div className="card-top">
                    <div>
                        <h2>Search & filters</h2>
                        <p>Quickly narrow the list to the applications you care about.</p>
                    </div>
                </div>

                <div className="search-row applications-search-row">
                    <input
                        type="text"
                        placeholder="Search company, position, status..."
                        value={searchText}
                        onChange={(e) => setSearchText(e.target.value)}
                    />
                </div>

                <div className="filter-row applications-filter-row">
                    <button
                        type="button"
                        className={filters === 'all' ? 'filter-btn active' : 'filter-btn'}
                        onClick={() => setFilters('all')}
                    >
                        All
                    </button>
                    {statusSteps.map((step) => (
                        <button
                            key={step}
                            type="button"
                            className={filters === step ? 'filter-btn active' : 'filter-btn'}
                            onClick={() => setFilters(step)}
                        >
                            {step}
                        </button>
                    ))}
                </div>
            </div>

            {visibleCount === 0 ? (
                <div className="empty-state applications-empty-state">
                    <h2>No applications found</h2>
                    <p>Try a different search term or change the selected filter.</p>
                </div>
            ) : (
                <div className="grid applications-grid">
                    {filteredApplications.map((app) => (
                        <ApplicationCard
                            key={app.id}
                            app={app}
                            statusSteps={statusSteps}
                            onClick={() => navigateToApplication(app.id)}
                            onDelete={() => onDeleteApplication(app.id)}
                        />
                    ))}
                </div>
            )}
        </section>
    )
}

export default ApplicationsPage