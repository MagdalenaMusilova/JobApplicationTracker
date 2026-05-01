import { useMemo, useState } from 'react'
import ApplicationCard from '../components/ApplicationCard'
import Modal from "../components/Modal.jsx"
import ApplicationFormSection from "../components/forms/ApplicationFormSection.jsx"
import StatusFormSection from "../components/forms/StatusFormSection.jsx"
import EventFormSection from "../components/forms/EventFormSection.jsx"
import CreateApplicationModal from "../modalWindows/CreateApplicationModal.jsx";

function ApplicationsPage({
                              applications,
                              avaibleStatuses,
                              onDeleteApplication,
                              onMarkAppRejected,
                              onCreateApplication
                          }) {
    const [filters, setFilters] = useState('all')
    const [searchText, setSearchText] = useState('')
    const [createOpen, setCreateOpen] = useState(false)

    const filteredApplications = useMemo(() => {
        const q = searchText.trim().toLowerCase()

        return applications.filter((app) => {
            const matchesStatus =
                filters === 'all' || app.status?.type === filters

            const matchesSearch =
                q.length === 0 ||
                `${app.company} ${app.position} ${app.jobDescription} ${app.notes} ${app.status?.type}`
                    .toLowerCase()
                    .includes(q)

            return matchesStatus && matchesSearch
        })
    }, [applications, filters, searchText])

    const handleSubmit = () => {
        const newApplication = {
            id: Date.now(),
            ...form,
            event: form.event || null
        }

        onCreateApplication(newApplication)

        setForm({
            company: "",
            position: "",
            jobDescription: "",
            notes: "",
            status: {
                type: avaibleStatuses[0] || "",
                note: ""
            },
            event: null
        })

        setCreateOpen(false)
    }



    const applicationCount = applications.length
    const visibleCount = filteredApplications.length
    const activeFilterLabel = filters === 'all' ? 'All statuses' : filters

    return (
        <section className="page applications-page">

            {/* HEADER */}
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

                    <button
                        type="button"
                        className="primary-btn"
                        onClick={() => setCreateOpen(true)}
                    >
                        + New Application
                    </button>
                </div>
            </div>

            {/* SEARCH + FILTERS */}
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
                        className={filters === 'all' ? 'filter-btn active' : 'filter-btn'}
                        onClick={() => setFilters('all')}
                    >
                        All
                    </button>

                    {avaibleStatuses.map((step) => (
                        <button
                            key={step}
                            className={filters === step ? 'filter-btn active' : 'filter-btn'}
                            onClick={() => setFilters(step)}
                        >
                            {step}
                        </button>
                    ))}
                </div>
            </div>

            {/* LIST */}
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
                            onDelete={() => onDeleteApplication(app.id)}
                            onRejected={() => onMarkAppRejected(app.id)}
                        />
                    ))}
                </div>
            )}

            {/* MODAL */}
            {createOpen && (
                <CreateApplicationModal
                    onClose={() => setCreateOpen(false)}
                    avaibleStatuses={avaibleStatuses}
                />
                )}
        </section>
    )
}

export default ApplicationsPage