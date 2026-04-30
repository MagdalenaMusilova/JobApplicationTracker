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
import { Routes, Route, Navigate, useNavigate } from 'react-router-dom'
import {ApplicationsAPI} from "./api/applications.js";
import {mapApplication} from "./utils/applicationMapper.js";


const TOKEN_STORAGE_KEY = 'job-tracker-token'

function getApiBaseUrl() {
    const baseUrl = import.meta.env.VITE_API_BASE_URL
    if (!baseUrl) throw new Error('VITE_API_BASE_URL is missing')
    return baseUrl
}

function getStoredToken() {
    return localStorage.getItem(TOKEN_STORAGE_KEY) ?? ''
}

function setStoredToken(token) {
    if (token) localStorage.setItem(TOKEN_STORAGE_KEY, token)
    else localStorage.removeItem(TOKEN_STORAGE_KEY)
}

async function apiRequest(path, options = {}, token = '') {
    const res = await fetch(`${getApiBaseUrl()}${path}`, {
        headers: {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
        },
        ...options,
    })

    if (!res.ok) {
        const msg = await res.text().catch(() => '')
        throw new Error(msg || `Request failed with status ${res.status}`)
    }

    if (res.status === 204) return null
    return res.json()
}

function App() {
    const [token, setToken] = useState(() => getStoredToken())

    // AUTH STATE
    const [authMode, setAuthMode] = useState('signin')
    const [authForm, setAuthForm] = useState({ username: '', password: '' })
    const [authError, setAuthError] = useState('')
    const [authLoading, setAuthLoading] = useState(false)

    const [selectedApplicationId, setSelectedApplicationId] = useState(null)

    const [applications, setApplications] = useState([])
    const [loadingApplications, setLoadingApplications] = useState(true)
    const [applicationsError, setApplicationsError] = useState(null)

    const [createOpen, setCreateOpen] = useState(false)

    const navigate = useNavigate()
    
    // ---------------- AUTH REDIRECT LOGIC ----------------
    useEffect(() => {
        if (token) {
            navigate('/dashboard')
        } else {
            navigate('/')
        }
    }, [token])

    // ---------------- LOAD DATA ----------------
    useEffect(() => {
        if (!token) {
            setLoadingApplications(false)
            return
        }

        const load = async () => {
            try {
                setLoadingApplications(true)
                const data = await apiRequest('/api/applications', {}, token)
                setApplications(Array.isArray(data) ? data : [])
            } catch (e) {
                setApplicationsError(e.message)
            } finally {
                setLoadingApplications(false)
            }
        }

        load()
    }, [token])

    // ---------------- LOGOUT FIX ----------------
    const handleLogout = () => {
        setStoredToken('')
        setToken('')
        setApplications([])
        setSelectedApplicationId(null)
        setScreen('auth')
    }

    // ---------------- AUTH SUBMIT (minimal) ----------------
    const handleAuthSubmit = async (e) => {
        e.preventDefault()
        try {
            setAuthLoading(true)
            setAuthError('')

            const res = await apiRequest('/api/auth/signin', {
                method: 'POST',
                body: JSON.stringify(authForm),
            })

            const nextToken = res?.token
            if (!nextToken) throw new Error('No token returned')

            setStoredToken(nextToken)
            setToken(nextToken)
            setScreen('dashboard')
        } catch (err) {
            setAuthError(err.message)
        } finally {
            setAuthLoading(false)
        }
    }

    // ---------------- AUTH SCREEN ----------------
    if (!token) {
        return (
            <div className="auth-shell">
                <section className="auth-panel card">
                    <h1>{authMode === 'signup' ? 'Create account' : 'Welcome back'}</h1>

                    <form onSubmit={handleAuthSubmit}>
                        <input
                            placeholder="username"
                            value={authForm.username}
                            onChange={(e) =>
                                setAuthForm({ ...authForm, username: e.target.value })
                            }
                        />

                        <input
                            type="password"
                            placeholder="password"
                            value={authForm.password}
                            onChange={(e) =>
                                setAuthForm({ ...authForm, password: e.target.value })
                            }
                        />

                        {authError && <p>{authError}</p>}

                        <button disabled={authLoading}>
                            {authLoading ? 'Loading...' : 'Login'}
                        </button>
                    </form>
                </section>
            </div>
        )
    }


    const handleOnMarkAppRejected = async (id) => {
        const updated = await ApplicationsAPI.deny(id)
        const mapped = mapApplication(updated)

        setApplications((prev) =>
            prev.map((app) =>
                app.id === id ? mapped : app
            )
        )
    }

    const handleOnDeleteApplication = async (id) => {
        await ApplicationsAPI.remove(id)

        setApplications((prev) =>
            prev.filter((app) => app.id !== id)
        );
    }
    
    
    // ---------------- MAIN APP ----------------
    return (
        <div className="shell">
            <Sidebar
                setCreateOpen={setCreateOpen}
                onLogout={handleLogout}
            />

            <main className="content">
                <Routes>
                    <Route path="/" element={<Navigate to="/dashboard" />} />

                    <Route path="/dashboard" element={<DashboardPage applications={applications} />} />
                    <Route path="/applications" element={<ApplicationsPage applications={applications} statusSteps={statusSteps} onDeleteApplication={ handleOnDeleteApplication} onMarkAppRejected={handleOnMarkAppRejected} />} />
                    <Route path="/applications/:id" element={<ApplicationDetailPage />} />
                    <Route path="/match" element={<JobMatchPage profile={profile} />} />
                    <Route path="/todos" element={<TodosPage />} />
                    <Route path="/profile" element={<ProfilePage profile={profile} />} />
                </Routes>
            </main>
        </div>
    )
}

export default App