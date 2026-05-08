import { useEffect, useState } from 'react'
import './App.css'

import Sidebar from './components/Sidebar'

import DashboardPage from './pages/DashboardPage'
import ApplicationsPage from './pages/ApplicationsPage'
import TodosPage from './pages/TodosPage'
import ProfilePage from './pages/ProfilePage'
import ApplicationDetailPage from './pages/ApplicationDetailPage'
import JobMatchPage from './pages/JobMatchPage'
import LoginPage from './pages/LoginPage'

import { profile } from './data/appData'
import { Routes, Route, Navigate, useNavigate, useLocation } from 'react-router-dom'

import { ApplicationsAPI } from "./api/applications"
import { StatusAPI } from "./api/statuses"
import { EventAPI } from "./api/events"

import { mapApplication } from "./utils/applicationMapper"
import { JobApplicationMinimal } from "./models/JobApplicationMinimal"
import { storage } from "./utils/storage"

/* -------------------- TYPES -------------------- */

type Token = string | null
type StatusType = any
type EventType = any

/* -------------------- PRIVATE ROUTE -------------------- */

function PrivateRoute({
                          token,
                          children,
                      }: {
    token: Token
    children: React.ReactNode
}) {
    return token ? <>{children}</> : <Navigate to="/login" replace />
}

/* -------------------- APP -------------------- */

function App() {
    const [token, setTokenState] = useState<Token>(() => storage.getToken())

    const [applications, setApplications] = useState<JobApplicationMinimal[]>([])
    const [statusTypes, setStatusTypes] = useState<StatusType[]>([])
    const [eventTypes, setEventTypes] = useState<EventType[]>([])
    const [loading, setLoading] = useState<boolean>(false)

    const navigate = useNavigate()
    const location = useLocation()

    const isAuthPage = location.pathname === '/login'

    /* -------------------- DATA -------------------- */

    useEffect(() => {
        const loadData = async () => {
            try {
                const [applicationsRes, statusTypesRes, eventTypesRes] =
                    await Promise.all([
                        ApplicationsAPI.getAll(),
                        StatusAPI.getTypes(),
                        EventAPI.getTypes(),
                    ])

                setApplications(applicationsRes)
                setStatusTypes(statusTypesRes)
                setEventTypes(eventTypesRes)
            } catch (error) {
                console.error("Failed to load initial data:", error)
            }
        }

        loadData()
    }, [])

    /* -------------------- LOGIN -------------------- */

    const handleLogin = (newToken: string) => {
        storage.setToken(newToken)
        setTokenState(newToken)
        navigate('/dashboard', { replace: true })
    }

    /* -------------------- LOGOUT -------------------- */

    const handleLogout = () => {
        storage.clearToken()
        setTokenState(null)
        setApplications([])
        setStatusTypes([])
        setEventTypes([])
        navigate('/login', { replace: true })
    }

    /* -------------------- ACTIONS -------------------- */

    const handleDeleteApplication = async (id: string) => {
        await ApplicationsAPI.remove(id)
        setApplications(prev => prev.filter(app => app.id !== id))
    }

    const handleMarkRejected = async (id: string) => {
        const updated = await ApplicationsAPI.deny(id)
        const mapped = mapApplication(updated)

        setApplications(prev =>
            prev.map(app => (app.id === id ? mapped : app))
        )
    }

    return (
        <div className={`shell ${token ? 'with-sidebar' : ''}`}>

            {!isAuthPage && token && (
                <Sidebar onLogout={handleLogout} />
            )}

            <main className="content">

                {loading && token && (
                    <div className="loading">Loading...</div>
                )}

                <Routes>

                    {/* LOGIN */}
                    <Route
                        path="/login"
                        element={
                            token
                                ? <Navigate to="/dashboard" replace />
                                : <LoginPage onLogin={handleLogin} />
                        }
                    />

                    {/* DASHBOARD */}
                    <Route
                        path="/dashboard"
                        element={
                            <PrivateRoute token={token}>
                                <DashboardPage applications={applications} />
                            </PrivateRoute>
                        }
                    />

                    {/* APPLICATIONS */}
                    <Route
                        path="/applications"
                        element={
                            <PrivateRoute token={token}>
                                <ApplicationsPage
                                    applications={applications}
                                    availableStatuses={statusTypes}
                                    eventTypes={eventTypes}
                                    setApplications={setApplications}
                                />
                            </PrivateRoute>
                        }
                    />

                    {/* DETAILS */}
                    <Route
                        path="/applications/:id"
                        element={
                            <PrivateRoute token={token}>
                                <ApplicationDetailPage />
                            </PrivateRoute>
                        }
                    />

                    {/* MATCH */}
                    <Route
                        path="/match"
                        element={
                            <PrivateRoute token={token}>
                                <JobMatchPage profile={profile} />
                            </PrivateRoute>
                        }
                    />

                    {/* TODOS */}
                    <Route
                        path="/todos"
                        element={
                            <PrivateRoute token={token}>
                                <TodosPage />
                            </PrivateRoute>
                        }
                    />

                    {/* PROFILE */}
                    <Route
                        path="/profile"
                        element={
                            <PrivateRoute token={token}>
                                <ProfilePage profile={profile} />
                            </PrivateRoute>
                        }
                    />

                    {/* FALLBACK */}
                    <Route
                        path="/"
                        element={
                            token
                                ? <Navigate to="/dashboard" replace />
                                : <Navigate to="/login" replace />
                        }
                    />

                    <Route
                        path="*"
                        element={
                            token
                                ? <Navigate to="/dashboard" replace />
                                : <Navigate to="/login" replace />
                        }
                    />

                </Routes>
            </main>
        </div>
    )
}

export default App