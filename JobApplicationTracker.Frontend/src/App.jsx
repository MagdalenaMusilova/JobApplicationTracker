import { useEffect, useMemo, useState } from 'react'
import './App.css'

import Sidebar from './components/Sidebar'
import Modal from './components/Modal'
import DashboardPage from './pages/DashboardPage'
import ApplicationsPage from './pages/ApplicationsPage'
import TodosPage from './pages/TodosPage'
import ProfilePage from './pages/ProfilePage'
import ApplicationDetailPage from './pages/ApplicationDetailPage'
import { profile } from './data/appData'
import { computeAutomaticMatchScore, statusSteps } from './utils/matchScore'

function getApiBaseUrl() {
  const baseUrl = import.meta.env.VITE_API_BASE_URL

  if (!baseUrl) {
    throw new Error('VITE_API_BASE_URL is missing')
  }

  return baseUrl
}

function normalizeStatus(status) {
  const value = String(status ?? '').toLowerCase()

  if (value.includes('interview')) return 'Interview'
  if (value.includes('offer')) return 'Offer'
  if (value.includes('close')) return 'Closed'
  if (value.includes('progress')) return 'In progress'

  return 'Applied'
}

function formatUpdatedAt(statusHistory) {
  const history = Array.isArray(statusHistory) ? statusHistory : []

  if (history.length === 0) {
    return 'Just now'
  }

  const lastItem = history[history.length - 1]
  const rawDate = lastItem?.createdAt ?? lastItem?.date ?? lastItem?.timestamp

  if (!rawDate) {
    return 'Updated recently'
  }

  const date = new Date(rawDate)
  if (Number.isNaN(date.getTime())) {
    return 'Updated recently'
  }

  return date.toLocaleDateString()
}

function mapBackendApplication(dto) {
  const lastStatusEntry = Array.isArray(dto.statusHistory) && dto.statusHistory.length > 0
      ? dto.statusHistory[dto.statusHistory.length - 1]
      : null

  return {
    id: dto.id,
    company: dto.company ?? '',
    title: dto.position ?? '',
    location: dto.location ?? 'Remote',
    status: normalizeStatus(lastStatusEntry?.jaStatus),
    updatedAt: formatUpdatedAt(dto.statusHistory),
    description: dto.note ?? 'No description provided.',
    notes: dto.note ?? '',
    requirements: [],
    nextStep: 'Continue tracking',
    dueDate: 'Soon',
    priority: 'Medium',
    backend: dto,
  }
}

async function apiRequest(path, options = {}) {
  const response = await fetch(`${getApiBaseUrl()}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(options.headers ?? {}),
    },
    ...options,
  })

  if (!response.ok) {
    const message = await response.text().catch(() => '')
    throw new Error(message || `Request failed with status ${response.status}`)
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}

function App() {
  const [screen, setScreen] = useState('dashboard')
  const [selectedApplicationId, setSelectedApplicationId] = useState(null)
  const [applications, setApplications] = useState([])
  const [loadingApplications, setLoadingApplications] = useState(true)
  const [applicationsError, setApplicationsError] = useState(null)
  const [createOpen, setCreateOpen] = useState(false)

  const [newApplication, setNewApplication] = useState({
    company: '',
    title: '',
    location: '',
    description: '',
  })

  useEffect(() => {
    const loadApplications = async () => {
      try {
        setLoadingApplications(true)
        setApplicationsError(null)

        const data = await apiRequest('/api/applications')
        const mappedApplications = Array.isArray(data) ? data.map(mapBackendApplication) : []

        setApplications(mappedApplications)

        if (mappedApplications.length > 0) {
          setSelectedApplicationId((currentId) => currentId ?? mappedApplications[0].id)
        }
      } catch (error) {
        setApplicationsError(error.message)
      } finally {
        setLoadingApplications(false)
      }
    }

    loadApplications()
  }, [])

  const selectedApplication = useMemo(() => {
    if (!applications.length || selectedApplicationId == null) return null
    return applications.find((item) => item.id === selectedApplicationId) ?? applications[0] ?? null
  }, [applications, selectedApplicationId])

  const selectedMatchScore = useMemo(() => {
    if (!selectedApplication) return 0
    return computeAutomaticMatchScore(selectedApplication, profile.skills)
  }, [selectedApplication])

  const todoItems = useMemo(() => {
    return applications
        .filter((app) => app.status !== 'Closed')
        .map((app) => ({
          ...app,
          matchScore: computeAutomaticMatchScore(app, profile.skills),
        }))
        .sort((a, b) => {
          const rank = { High: 0, Medium: 1, Low: 2 }
          return (rank[a.priority] ?? 9) - (rank[b.priority] ?? 9)
        })
  }, [applications])

  const dashboardRecentApplications = useMemo(() => {
    return [...applications].slice(0, 3)
  }, [applications])

  const dashboardNextActions = useMemo(() => todoItems.slice(0, 4), [todoItems])

  const todoCalendar = useMemo(() => {
    const days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    return days.map((day) => ({
      day,
      items: todoItems.filter((item) => (item.dueDate || '').includes(day)),
    }))
  }, [todoItems])

  const pipelineSummary = useMemo(() => {
    const grouped = {
      Applied: 0,
      'In progress': 0,
      Interview: 0,
      Offer: 0,
      Closed: 0,
    }

    for (const app of applications) {
      grouped[app.status] = (grouped[app.status] ?? 0) + 1
    }

    return grouped
  }, [applications])

  const dashboardStats = useMemo(() => {
    const activeApplications = applications.filter((app) => app.status !== 'Closed').length
    const interviewCount = applications.filter((app) => app.status === 'Interview').length
    const urgentTasks = todoItems.filter((app) => app.priority === 'High').length
    const bestFit = applications.length
        ? Math.max(...applications.map((app) => computeAutomaticMatchScore(app, profile.skills)))
        : 0

    return {
      activeApplications,
      interviewCount,
      urgentTasks,
      bestFit,
    }
  }, [applications, todoItems])

  const urgentActionItems = useMemo(() => {
    return dashboardNextActions.filter((app) => app.priority === 'High').slice(0, 3)
  }, [dashboardNextActions])

  const navigateToApplication = (id) => {
    setSelectedApplicationId(id)
    setScreen('application')
  }

  const handleCreateApplication = async (event) => {
    event.preventDefault()

    try {
      const payload = {
        userId: 1,
        company: newApplication.company,
        position: newApplication.title,
        note: newApplication.description || '',
        jaStatus: 'Applied',
        jaStatusNote: newApplication.location || 'Remote',
      }

      const created = await apiRequest('/api/applications', {
        method: 'POST',
        body: JSON.stringify(payload),
      })

      const mappedCreated = mapBackendApplication(created)

      setApplications((current) => [mappedCreated, ...current])
      setSelectedApplicationId(mappedCreated.id)
      setNewApplication({ company: '', title: '', location: '', description: '' })
      setCreateOpen(false)
      setScreen('application')
    } catch (error) {
      setApplicationsError(error.message)
    }
  }

  if (loadingApplications) {
    return (
        <div className="shell">
          <Sidebar screen={screen} setScreen={setScreen} setCreateOpen={setCreateOpen} />
          <main className="content">
            <section className="page">
              <h1>Loading applications...</h1>
            </section>
          </main>
        </div>
    )
  }

  if (applicationsError) {
    return (
        <div className="shell">
          <Sidebar screen={screen} setScreen={setScreen} setCreateOpen={setCreateOpen} />
          <main className="content">
            <section className="page">
              <h1>Couldn’t load applications</h1>
              <p>{applicationsError}</p>
            </section>
          </main>
        </div>
    )
  }

  return (
      <div className="shell">
        <Sidebar screen={screen} setScreen={setScreen} setCreateOpen={setCreateOpen} />

        <main className="content">
          {screen === 'dashboard' && (
              <DashboardPage
                  profile={profile}
                  applications={applications}
                  dashboardStats={dashboardStats}
                  pipelineSummary={pipelineSummary}
                  urgentActionItems={urgentActionItems}
                  dashboardRecentApplications={dashboardRecentApplications}
                  navigateToApplication={navigateToApplication}
              />
          )}

          {screen === 'applications' && (
              <ApplicationsPage
                  applications={applications}
                  statusSteps={statusSteps}
                  navigateToApplication={navigateToApplication}
                  setCreateOpen={setCreateOpen}
              />
          )}

          {screen === 'todos' && (
              <TodosPage
                  todoItems={todoItems}
                  todoCalendar={todoCalendar}
                  setCreateOpen={setCreateOpen}
              />
          )}

          {screen === 'profile' && <ProfilePage profile={profile} />}

          {screen === 'application' && selectedApplication && (
              <ApplicationDetailPage
                  profile={profile}
                  selectedApplication={selectedApplication}
                  selectedMatchScore={selectedMatchScore}
                  navigateBack={() => setScreen('applications')}
                  openCreate={() => setCreateOpen(true)}
              />
          )}
        </main>

        {createOpen && (
            <Modal
                title="Create new application"
                subtitle="Quick entry form for adding a job in seconds."
                onClose={() => setCreateOpen(false)}
            >
              <form className="auth-form" onSubmit={handleCreateApplication}>
                <label>
                  Company
                  <input
                      type="text"
                      value={newApplication.company}
                      onChange={(e) => setNewApplication({ ...newApplication, company: e.target.value })}
                      required
                  />
                </label>

                <label>
                  Role
                  <input
                      type="text"
                      value={newApplication.title}
                      onChange={(e) => setNewApplication({ ...newApplication, title: e.target.value })}
                      required
                  />
                </label>

                <label>
                  Location
                  <input
                      type="text"
                      value={newApplication.location}
                      onChange={(e) => setNewApplication({ ...newApplication, location: e.target.value })}
                      placeholder="Remote"
                  />
                </label>

                <label>
                  Description
                  <textarea
                      rows="4"
                      value={newApplication.description}
                      onChange={(e) =>
                          setNewApplication({ ...newApplication, description: e.target.value })
                      }
                  />
                </label>

                <div className="button-row">
                  <button type="button" className="secondary-btn" onClick={() => setCreateOpen(false)}>
                    Cancel
                  </button>
                  <button type="submit" className="primary-btn">
                    Create application
                  </button>
                </div>
              </form>
            </Modal>
        )}
      </div>
  )
}

export default App