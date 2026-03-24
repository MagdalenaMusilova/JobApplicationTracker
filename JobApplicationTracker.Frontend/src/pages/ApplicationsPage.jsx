import ApplicationCard from '../components/ApplicationCard'

function ApplicationsPage({
                              applications,
                              statusSteps,
                              filters,
                              setFilters,
                              searchText,
                              setSearchText,
                              navigateToApplication,
                              setCreateOpen,
                          }) {
    return (
        <section className="page">
            <div className="page-header">
                <div>
                    <h1>Applications</h1>
                    <p>Browse, search, and filter all tracked applications.</p>
                </div>
                <button type="button" className="primary-btn" onClick={() => setCreateOpen(true)}>
                    + New Application
                </button>
            </div>

            <div className="card">
                <div className="card-top">
                    <div>
                        <h2>Search & filters</h2>
                        <p>Find applications quickly.</p>
                    </div>
                </div>

                <div className="search-row">
                    <input
                        type="text"
                        placeholder="Search company, role, notes..."
                        value={searchText}
                        onChange={(e) => setSearchText(e.target.value)}
                    />
                </div>

                <div className="filter-row">
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

            <div className="grid">
                {applications.map((app) => (
                    <ApplicationCard
                        key={app.id}
                        app={app}
                        statusSteps={statusSteps}
                        onClick={() => navigateToApplication(app.id)}
                    />
                ))}
            </div>
        </section>
    )
}

export default ApplicationsPage