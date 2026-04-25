import { useEffect, useMemo, useState } from 'react'
import './App.css'

import Sidebar from './components/Sidebar'
import Modal from './components/Modal'
import DashboardPage from './pages/DashboardPage'
import ApplicationsPage from './pages/ApplicationsPage'
import TodosPage from './pages/TodosPage'
import ProfilePage from './pages/ProfilePage'
import ApplicationDetailPage from './pages/ApplicationDetailPage'
import JobMatchPage from './pages/JobMatchPage'
import { profile } from './data/appData'
import { computeAutomaticMatchScore, statusSteps } from './utils/matchScore'

const TOKEN_STORAGE_KEY = 'job-tracker-token'

function getApiBaseUrl() {
  const baseUrl = import.meta.env.VITE_API_BASE_URL

  if (!baseUrl) {
    throw new Error('VITE_API_BASE_URL is missing')
  }

  return baseUrl
}

function normalizeStatus(status) {
  const value = String(status ?? '').trim()
  return value.charAt(0).toUpperCase() + value.slice(1).toLowerCase()
}

function formatDateTime(value) {
  if (!value) return '—'
  const date = new Date(value)
  if (Number.isNaN(date.getTime())) return '—'
  return date.toLocaleString()
}

function getClosestEvent(statusHistory, scheduledEvents) {
  const events = Array.isArray(scheduledEvents) ? scheduledEvents : []
  if (events.length === 0) return null

  const now = Date.now()
  const parsedEvents = events
      .map((event) => ({
        ...event,
        parsedDate: event?.eventDate ? new Date(event.eventDate).getTime() : Number.NaN,
      }))
      .filter((event) => !Number.isNaN(event.parsedDate))

  if (parsedEvents.length === 0) return null

  const futureEvents = parsedEvents
      .filter((event) => event.parsedDate >= now)
      .sort((a, b) => a.parsedDate - b.parsedDate)

  if (futureEvents.length > 0) return futureEvents[0]

  return parsedEvents.sort((a, b) => b.parsedDate - a.parsedDate)[0]
}

function formatUpdatedAt(statusHistory) {
  const history = Array.isArray(statusHistory) ? statusHistory : []

  if (history.length === 0) {
    return 'Just now'
  }

  const lastItem = history[history.length - 1]
  const rawDate = lastItem?.updatedAt ?? lastItem?.createdAt

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
  const statusHistory = Array.isArray(dto.statusHistory) ? dto.statusHistory : []
  const lastStatusEntry = statusHistory.length > 0 ? statusHistory[statusHistory.length - 1] : null
  const jobDescription =
      dto.jobListingDto?.jobDescription ??
      dto.jobDescription ??
      dto.description ??
      ''

  const scheduledEvents = Array.isArray(dto.scheduledEvents) ? dto.scheduledEvents : []
  const closestEvent = getClosestEvent(statusHistory, scheduledEvents)

  return {
    id: dto.id,
    company: dto.company ?? '',
    title: dto.position ?? '',
    location: dto.location ?? 'Remote',
    status: normalizeStatus(lastStatusEntry?.jaStatus),
    updatedAt: formatUpdatedAt(statusHistory),
    description: jobDescription,
    notes: dto.note ?? '',
    statusHistory,
    scheduledEvents,
    closestEvent,
    requirements: [],
    nextStep: 'Continue tracking',
    dueDate: 'Soon',
    priority: 'Medium',
    backend: dto,
  }
}

function getStoredToken() {
  return localStorage.getItem(TOKEN_STORAGE_KEY) ?? ''
}

function setStoredToken(token) {
  if (token) {
    localStorage.setItem(TOKEN_STORAGE_KEY, token)
  } else {
    localStorage.removeItem(TOKEN_STORAGE_KEY)
  }
}

async function apiRequest(path, options = {}, token = '') {
  const response = await fetch(`${getApiBaseUrl()}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
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
  const [token, setToken] = useState(() => getStoredToken())
  const [authMode, setAuthMode] = useState('signin')
  const [authForm, setAuthForm] = useState({
    username: '',
    password: '',
  })
  const [authError, setAuthError] = useState('')
  const [authLoading, setAuthLoading] = useState(false)

  const [screen, setScreen] = useState('dashboard')
  const [selectedApplicationId, setSelectedApplicationId] = useState(null)
  const [applications, setApplications] = useState([])
  const [loadingApplications, setLoadingApplications] = useState(true)
  const [applicationsError, setApplicationsError] = useState(null)
  const [createOpen, setCreateOpen] = useState(false)
  const [editOpen, setEditOpen] = useState(false)
  const [editForm, setEditForm] = useState({ company: '', position: '', notes: '' })
  const [statusEditOpen, setStatusEditOpen] = useState(false)
  const [statusEditMode, setStatusEditMode] = useState('add') // 'add' | 'edit'
  const [statusEditForm, setStatusEditForm] = useState({ jaStatus: 'Applied', note: '' })
  const [statusHistoryOpen, setStatusHistoryOpen] = useState(false)
  const [statusHistoryApplication, setStatusHistoryApplication] = useState(null)
  const [confirmOpen, setConfirmOpen] = useState(false)
  const [confirmAction, setConfirmAction] = useState(null)
  const [confirmMessage, setConfirmMessage] = useState('')
  const [availableStatuses, setAvailableStatuses] = useState([])


  const [newApplication, setNewApplication] = useState({
    company: '',
    position: '',
    jobDescription: '',
    notes: '',
    status: 'Applied',
    statusNote: '',
  })

  useEffect(() => {
    if (!token) {
      setLoadingApplications(false)
      return
    }

    const loadApplications = async () => {
      try {
        setLoadingApplications(true)
        setApplicationsError(null)

        const data = await apiRequest('/api/applications', {}, token)
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
  }, [token])

  useEffect(() => {
    const loadStatuses = async () => {
      try {
        const data = await apiRequest('/api/statuses', {}, token)
        setAvailableStatuses(data.sort((a, b) => a.order - b.order))
      } catch (error) {
        setApplicationsError(`Failed to load statuses: ${error.message}`)
      }
    }

    if (token) loadStatuses()
  }, [token])
  

  const handleAuthSubmit = async (event) => {
    event.preventDefault()

    try {
      setAuthLoading(true)
      setAuthError('')

      if (authMode === 'signup') {
        await apiRequest('/api/users', {
          method: 'POST',
          body: JSON.stringify({
            username: authForm.username,
            password: authForm.password,
          }),
        })
      }

      const response = await apiRequest('/api/auth/signin', {
        method: 'POST',
        body: JSON.stringify({
          username: authForm.username,
          password: authForm.password,
        }),
      })

      const nextToken = response?.token ?? response?.Token

      if (!nextToken) {
        throw new Error('Authentication succeeded, but no token was returned.')
      }

      setStoredToken(nextToken)
      setToken(nextToken)
      setAuthForm({ username: '', password: '' })
    } catch (error) {
      setAuthError(error.message)
    } finally {
      setAuthLoading(false)
    }
  }

  const handleLogout = () => {
    setStoredToken('')
    setToken('')
    setApplications([])
    setSelectedApplicationId(null)
    setScreen('dashboard')
  }

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

  const openStatusHistory = (app) => {
    setStatusHistoryApplication(app)
    setStatusHistoryOpen(true)
  }

  const closeStatusHistory = () => {
    setStatusHistoryOpen(false)
    setStatusHistoryApplication(null)
  }

  const requestConfirm = (message, action) => {
    setConfirmMessage(message)
    setConfirmAction(() => action)
    setConfirmOpen(true)
  }

  const handleConfirm = async () => {
    setConfirmOpen(false)
    if (confirmAction) await confirmAction()
  }

  const handleDeleteApplication = async (applicationId) => {
    requestConfirm('Are you sure you want to delete this application? This cannot be undone.', async () => {
      try {
        await apiRequest(`/api/applications/${applicationId}`, { method: 'DELETE' }, token)
        setApplications((current) => current.filter((app) => app.id !== applicationId))
        if (selectedApplicationId === applicationId) {
          setSelectedApplicationId(null)
          setScreen('applications')
        }
      } catch (error) {
        setApplicationsError(error.message)
      }
    })
  }

  const handleCreateApplication = async (event) => {
    event.preventDefault()

    try {
      const payload = {
        company: newApplication.company,
        position: newApplication.position,
        note: newApplication.notes || '',
        jaStatus: newApplication.status || 'Applied',
        jaStatusNote: newApplication.statusNote || '',
      }

      const created = await apiRequest(
          '/api/applications',
          {
            method: 'POST',
            body: JSON.stringify(payload),
          },
          token,
      )

      const mappedCreated = mapBackendApplication(created)

      setApplications((current) => [mappedCreated, ...current])
      setSelectedApplicationId(mappedCreated.id)
      setNewApplication({
        company: '',
        position: '',
        jobDescription: '',
        notes: '',
        status: 'Applied',
        statusNote: '',
      })
      setCreateOpen(false)
      setScreen('application')
    } catch (error) {
      setApplicationsError(error.message)
    }
  }

  const handleCreateApplicationFromMatch = async (matchedApplication) => {
    try {
      const payload = {
        company: matchedApplication.company,
        position: matchedApplication.title,
        note: matchedApplication.notes || matchedApplication.description || '',
        jaStatus: 'Applied',
        jaStatusNote: matchedApplication.notes || '',
      }

      const created = await apiRequest(
          '/api/applications',
          {
            method: 'POST',
            body: JSON.stringify(payload),
          },
          token,
      )

      const mappedCreated = mapBackendApplication(created)

      setApplications((current) => [mappedCreated, ...current])
      setSelectedApplicationId(mappedCreated.id)
      setScreen('application')
    } catch (error) {
      setApplicationsError(error.message)
    }
  }

  const openEditApplication = (app) => {
    setEditForm({
      company: app.company,
      position: app.title,
      notes: app.notes,
    })
    setEditOpen(true)
  }


  const getAllowedTransitions = (currentStatusName) => {
    const current = availableStatuses.find((s) => s.name === currentStatusName)
    if (!current) return availableStatuses

    const terminal = ['Rejected']
    const rejectedOnly = ['Accepted']
    const forwardPlusTaskAndInterview = ['Offer']
    const forwardPlusTask = ['Interview']
    // Whishlist, Applied, Task → forward only (Task can also repeat itself, handled below)

    if (terminal.includes(current.name)) return []

    if (rejectedOnly.includes(current.name)) {
      return availableStatuses.filter((s) => s.name === 'Rejected')
    }

    const forward = availableStatuses.filter((s) => s.order > current.order)

    if (forwardPlusTaskAndInterview.includes(current.name)) {
      const extras = availableStatuses.filter((s) => s.name === 'Task' || s.name === 'Interview')
      return [...new Map([...forward, ...extras].map((s) => [s.name, s])).values()]
          .sort((a, b) => a.order - b.order)
    }

    if (forwardPlusTask.includes(current.name)) {
      const extras = availableStatuses.filter((s) => s.name === 'Task' || s.name === 'Interview')
      return [...new Map([...forward, ...extras].map((s) => [s.name, s])).values()]
          .sort((a, b) => a.order - b.order)
    }

    // Task → forward + Task itself
    if (current.name === 'Task') {
      const extras = availableStatuses.filter((s) => s.name === 'Task')
      return [...new Map([...forward, ...extras].map((s) => [s.name, s])).values()]
          .sort((a, b) => a.order - b.order)
    }

    return forward
  }

  const openAddStatus = (app) => {
    const history = Array.isArray(app.statusHistory) ? app.statusHistory : []
    const last = history[history.length - 1]
    const currentName = last?.jaStatus ?? null

    const allowed = getAllowedTransitions(currentName)
    const firstAllowed = allowed[0]?.name ?? ''

    setStatusEditMode('add')
    setStatusEditForm({ jaStatus: firstAllowed, note: '', allowedStatuses: allowed })
    setStatusEditOpen(true)
  }

  const openEditLastStatus = (app) => {
    const history = Array.isArray(app.statusHistory) ? app.statusHistory : []
    const last = history[history.length - 1]
    if (!last) return

    // For editing, compute transitions from the entry *before* the last one
    const prevName = history.length >= 2 ? history[history.length - 2].jaStatus ?? null : null
    const allowed = getAllowedTransitions(prevName)

    setStatusEditMode('edit')
    setStatusEditForm({ jaStatus: last.jaStatus ?? '', note: last.note ?? '', id: last.id, allowedStatuses: allowed })
    setStatusEditOpen(true)
  }

  const handleDeleteLastStatus = async (app) => {
    const history = Array.isArray(app.statusHistory) ? app.statusHistory : []
    const last = history[history.length - 1]
    if (!last) return

    requestConfirm(`Remove status "${last.jaStatus ?? 'Applied'}"? This cannot be undone.`, async () => {
      try {
        const updated = await apiRequest(`/api/applications/entry/${last.id}`, { method: 'DELETE' }, token)
        const mappedUpdated = mapBackendApplication(updated)
        setApplications((current) => current.map((a) => (a.id === mappedUpdated.id ? mappedUpdated : a)))
      } catch (error) {
        setApplicationsError(error.message)
      }
    })
  }

  const handleStatusEditSubmit = async (event) => {
    event.preventDefault()

    try {
      if (statusEditMode === 'add') {
        const updated = await apiRequest(
            '/api/applications/entry',
            {
              method: 'POST',
              body: JSON.stringify({
                jobApplicationId: selectedApplicationId,
                jaStatus: statusEditForm.jaStatus,
                note: statusEditForm.note || '',
              }),
            },
            token,
        )
        const mappedUpdated = mapBackendApplication(updated)
        setApplications((current) => current.map((a) => (a.id === mappedUpdated.id ? mappedUpdated : a)))
      } else {
        await apiRequest(
            `/api/applications/entry/${statusEditForm.id}`,
            {
              method: 'PUT',
              body: JSON.stringify({
                jobApplicationId: selectedApplicationId,
                jaStatus: statusEditForm.jaStatus,
                note: statusEditForm.note || '',
              }),
            },
            token,
        )
        // Refresh the application to reflect updated status
        const refreshed = await apiRequest(`/api/applications/${selectedApplicationId}`, {}, token)
        const mappedRefreshed = mapBackendApplication(refreshed)
        setApplications((current) => current.map((a) => (a.id === mappedRefreshed.id ? mappedRefreshed : a)))
      }

      setStatusEditOpen(false)
    } catch (error) {
      setApplicationsError(error.message)
    }
  }

  const handleEditApplication = async (event) => {
    event.preventDefault()

    try {
      const payload = {
        company: editForm.company,
        position: editForm.position,
        note: editForm.notes || '',
      }

      const updated = await apiRequest(
          `/api/applications/${selectedApplicationId}`,
          {
            method: 'PUT',
            body: JSON.stringify(payload),
          },
          token,
      )

      const mappedUpdated = mapBackendApplication(updated)

      setApplications((current) =>
          current.map((app) => (app.id === mappedUpdated.id ? mappedUpdated : app)),
      )
      setEditOpen(false)
    } catch (error) {
      setApplicationsError(error.message)
    }
  }

  if (!token) {
    return (
        <div className="auth-shell">
          <section className="auth-panel card">
            <p className="eyebrow">Job Application Tracker</p>
            <h1>{authMode === 'signup' ? 'Create your account' : 'Welcome back'}</h1>
            <p className="muted">
              {authMode === 'signup'
                  ? 'Create an account to start tracking applications.'
                  : 'Log in to access your job applications.'}
            </p>

            <form className="auth-form" onSubmit={handleAuthSubmit}>
              <label>
                Username
                <input
                    type="text"
                    value={authForm.username}
                    onChange={(e) => setAuthForm({ ...authForm, username: e.target.value })}
                    required
                    autoComplete="username"
                />
              </label>

              <label>
                Password
                <input
                    type="password"
                    value={authForm.password}
                    onChange={(e) => setAuthForm({ ...authForm, password: e.target.value })}
                    required
                    autoComplete={authMode === 'signup' ? 'new-password' : 'current-password'}
                />
              </label>

              {authError && <p className="auth-error">{authError}</p>}

              <div className="button-row">
                <button
                    type="button"
                    className="secondary-btn"
                    onClick={() => setAuthMode(authMode === 'signup' ? 'signin' : 'signup')}
                >
                  {authMode === 'signup' ? 'I already have an account' : 'Create an account'}
                </button>
                <button type="submit" className="primary-btn" disabled={authLoading}>
                  {authLoading ? 'Please wait...' : authMode === 'signup' ? 'Sign up' : 'Log in'}
                </button>
              </div>
            </form>
          </section>
        </div>
    )
  }

  if (loadingApplications) {
    return (
        <div className="shell">
          <Sidebar
              screen={screen}
              setScreen={setScreen}
              setCreateOpen={setCreateOpen}
              onLogout={handleLogout}
          />
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
          <Sidebar
              screen={screen}
              setScreen={setScreen}
              setCreateOpen={setCreateOpen}
              onLogout={handleLogout}
          />
          <main className="content">
            <section className="page">
              <h1>Couldn't load applications</h1>
              <p>{applicationsError}</p>
            </section>
          </main>
        </div>
    )
  }

  return (
      <div className="shell">
        <Sidebar
            screen={screen}
            setScreen={setScreen}
            setCreateOpen={setCreateOpen}
            onLogout={handleLogout}
        />

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
                  onDeleteApplication={handleDeleteApplication}
              />
          )}

          {screen === 'match' && (
              <JobMatchPage
                  profile={profile}
                  onCreateApplicationFromMatch={handleCreateApplicationFromMatch}
              />
          )}

          {screen === 'todos' && (
              <TodosPage todoItems={todoItems} todoCalendar={todoCalendar} setCreateOpen={setCreateOpen} />
          )}

          {screen === 'profile' && <ProfilePage profile={profile} />}

          {screen === 'application' && selectedApplication && (
              <ApplicationDetailPage
                  profile={profile}
                  selectedApplication={selectedApplication}
                  selectedMatchScore={selectedMatchScore}
                  navigateBack={() => setScreen('applications')}
                  openCreate={() => setCreateOpen(true)}
                  openEdit={() => openEditApplication(selectedApplication)}
                  onDeleteApplication={handleDeleteApplication}
                  onOpenStatusHistory={() => openStatusHistory(selectedApplication)}
                  onAddStatus={() => openAddStatus(selectedApplication)}
                  onEditLastStatus={() => openEditLastStatus(selectedApplication)}
                  onDeleteLastStatus={() => handleDeleteLastStatus(selectedApplication)}
              />
          )}
        </main>

        {createOpen && (
            <Modal
                title="Create new application"
                subtitle="Add company, role, description, notes, and first status."
                onClose={() => setCreateOpen(false)}
            >
              <form className="modal-form" onSubmit={handleCreateApplication}>
                <div className="modal-grid">
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
                    Position
                    <input
                        type="text"
                        value={newApplication.position}
                        onChange={(e) => setNewApplication({ ...newApplication, position: e.target.value })}
                        required
                    />
                  </label>

                  <label className="modal-span-2">
                    Job description
                    <textarea
                        rows="5"
                        value={newApplication.jobDescription}
                        onChange={(e) =>
                            setNewApplication({ ...newApplication, jobDescription: e.target.value })
                        }
                    />
                  </label>

                  <label className="modal-span-2">
                    Notes
                    <textarea
                        rows="4"
                        value={newApplication.notes}
                        onChange={(e) => setNewApplication({ ...newApplication, notes: e.target.value })}
                    />
                  </label>

                  <label>
                    First status
                    <select
                        value={newApplication.status}
                        onChange={(e) => setNewApplication({ ...newApplication, status: e.target.value })}
                    >
                      {availableStatuses.map((s) => (
                          <option key={s.name} value={s.name}>{s.name}</option>
                      ))}
                    </select>
                  </label>

                  <label>
                    Status note
                    <input
                        type="text"
                        value={newApplication.statusNote}
                        onChange={(e) => setNewApplication({ ...newApplication, statusNote: e.target.value })}
                        placeholder="Optional note for the first status"
                    />
                  </label>
                </div>

                <div className="button-row modal-actions">
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

        {editOpen && (
            <Modal
                title="Edit application"
                subtitle="Update the company, position, or notes."
                onClose={() => setEditOpen(false)}
            >
              <form className="modal-form" onSubmit={handleEditApplication}>
                <div className="modal-grid">
                  <label>
                    Company
                    <input
                        type="text"
                        value={editForm.company}
                        onChange={(e) => setEditForm({ ...editForm, company: e.target.value })}
                        required
                    />
                  </label>

                  <label>
                    Position
                    <input
                        type="text"
                        value={editForm.position}
                        onChange={(e) => setEditForm({ ...editForm, position: e.target.value })}
                        required
                    />
                  </label>

                  <label className="modal-span-2">
                    Notes
                    <textarea
                        rows="4"
                        value={editForm.notes}
                        onChange={(e) => setEditForm({ ...editForm, notes: e.target.value })}
                    />
                  </label>
                </div>

                <div className="button-row modal-actions">
                  <button type="button" className="secondary-btn" onClick={() => setEditOpen(false)}>
                    Cancel
                  </button>
                  <button type="submit" className="primary-btn">
                    Save changes
                  </button>
                </div>
              </form>
            </Modal>
        )}
        {statusEditOpen && (
            <Modal
                title={statusEditMode === 'add' ? 'Add status' : 'Edit last status'}
                subtitle={statusEditMode === 'add' ? 'Push a new status entry onto this application.' : 'Update the latest status entry.'}
                onClose={() => setStatusEditOpen(false)}
            >
              {statusEditForm.allowedStatuses?.length === 0 ? (
                  <div>
                    <p className="muted">This application has already reached a final status. No further statuses can be added.</p>
                    <div className="button-row modal-actions">
                      <button type="button" className="secondary-btn" onClick={() => setStatusEditOpen(false)}>Close</button>
                    </div>
                  </div>
              ) : (
                  <form className="modal-form" onSubmit={handleStatusEditSubmit}>
                    <div className="modal-grid">
                      <label>
                        Status
                        <select
                            value={statusEditForm.jaStatus}
                            onChange={(e) => setStatusEditForm({ ...statusEditForm, jaStatus: e.target.value })}
                        >
                          {(statusEditForm.allowedStatuses ?? availableStatuses).map((s) => (
                              <option key={s.name} value={s.name}>{s.name}</option>
                          ))}
                        </select>
                      </label>

                      <label className="modal-span-2">
                        Note
                        <input
                            type="text"
                            value={statusEditForm.note}
                            onChange={(e) => setStatusEditForm({ ...statusEditForm, note: e.target.value })}
                            placeholder="Optional note"
                        />
                      </label>
                    </div>

                    <div className="button-row modal-actions">
                      <button type="button" className="secondary-btn" onClick={() => setStatusEditOpen(false)}>
                        Cancel
                      </button>
                      <button type="submit" className="primary-btn">
                        {statusEditMode === 'add' ? 'Add status' : 'Save changes'}
                      </button>
                    </div>
                  </form>
              )}
            </Modal>
        )}
        {statusHistoryOpen && statusHistoryApplication && (
            <Modal
                title="Status history"
                subtitle={`${statusHistoryApplication.company} · ${statusHistoryApplication.title}`}
                onClose={closeStatusHistory}
            >
              <div className="stack-list">
                {(statusHistoryApplication.statusHistory || []).map((entry, index) => (
                    <div key={entry.id ?? `${entry.jaStatus}-${index}`} className="editable-item">
                      <div className="editable-item-header">
                        <div>
                          <h3>{entry.jaStatus ?? 'Applied'}</h3>
                          <p className="muted">Created: {formatDateTime(entry.createdAt)}</p>
                          <p className="muted">Updated: {formatDateTime(entry.updatedAt)}</p>
                        </div>
                      </div>
                      <p>{entry.note || 'No note.'}</p>
                    </div>
                ))}
              </div>
            </Modal>
        )}
        {confirmOpen && (
            <Modal
                title="Confirm action"
                subtitle={confirmMessage}
                onClose={() => setConfirmOpen(false)}
            >
              <div className="button-row modal-actions">
                <button type="button" className="secondary-btn" onClick={() => setConfirmOpen(false)}>
                  Cancel
                </button>
                <button type="button" className="primary-btn danger" onClick={handleConfirm}>
                  Confirm
                </button>
              </div>
            </Modal>
        )}
      </div>
  )
}

export default App