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

  console.log(import.meta.env)

  useEffect(() => {
    console.log('VITE_API_BASE_URL:', import.meta.env.VITE_API_BASE_URL)
  }, [])
  
  useEffect(() => {
    const loadApplications = async () => {
      try {
        setLoadingApplications(true)
        setApplicationsError(null)

        const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/api/applications`)

        if (!response.ok) {
          throw new Error(`Failed to load applications: ${response.status}`)
        }

        const data = await response.json()
        setApplications(data)

        if (data.length > 0) {
          setSelectedApplicationId((currentId) => currentId ?? data[0].id)
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

  const handleCreateApplication = (event) => {
    event.preventDefault()

    const nextApp = {
      id: `APP-${String(applications.length + 1001)}`,
      company: newApplication.company,
      title: newApplication.title,
      location: newApplication.location || 'Remote',
      status: 'Applied',
      updatedAt: 'Just now',
      description: newApplication.description || 'New application created.',
      notes: 'Application added successfully.',
      requirements: [],
      nextStep: 'Initial tracking',
      dueDate: 'Soon',
      priority: 'Medium',
    }

    setApplications([nextApp, ...applications])
    setSelectedApplicationId(nextApp.id)
    setNewApplication({ company: '', title: '', location: '', description: '' })
    setCreateOpen(false)
    setScreen('application')
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