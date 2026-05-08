import { useState } from 'react'
import { AuthAPI } from '../api/auth'
import '../styles/LoginPage.css'

export default function LoginPage({ onLogin }) {

    const [mode, setMode] = useState('signin')
    const [form, setForm] = useState({
        username: '',
        password: '',
    })

    const [loading, setLoading] = useState(false)
    const [error, setError] = useState('')

    const handleChange = (e) => {
        setForm(prev => ({
            ...prev,
            [e.target.name]: e.target.value,
        }))
    }

    const handleSubmit = async (e) => {
        e.preventDefault()
        if (loading) return

        try {
            setLoading(true)
            setError('')

            if (mode === 'signup') {
                await AuthAPI.signup(form)
            }

            const res = await AuthAPI.signin(form)

            const token = res?.token ?? res?.Token

            if (!token) {
                throw new Error('Wrong username or password')
            }

            // ONLY notify App
            onLogin(token)

        } catch (err) {
            setError(err?.message || 'Wrong username or password')
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-shell">
            <div className="auth-panel card">

                <div className="auth-header">
                    <div className="brand">JobTracker</div>

                    <h1>
                        {mode === 'signup' ? 'Create account' : 'Welcome back'}
                    </h1>

                    <p className="muted">
                        {mode === 'signup'
                            ? 'Create an account to start tracking applications'
                            : 'Log in to continue'}
                    </p>
                </div>

                <form className="auth-form" onSubmit={handleSubmit}>

                    <label>
                        Username
                        <input
                            name="username"
                            value={form.username}
                            onChange={handleChange}
                            required
                        />
                    </label>

                    <label>
                        Password
                        <input
                            name="password"
                            type="password"
                            value={form.password}
                            onChange={handleChange}
                            required
                        />
                    </label>

                    {error && <div className="auth-error">{error}</div>}

                    <button className="primary-btn" disabled={loading}>
                        {loading ? 'Please wait...' :
                            mode === 'signup' ? 'Create account' : 'Log in'}
                    </button>

                    <button
                        type="button"
                        className="secondary-btn"
                        onClick={() =>
                            setMode(m => (m === 'signin' ? 'signup' : 'signin'))
                        }
                    >
                        {mode === 'signup'
                            ? 'Already have an account? Log in'
                            : "Don't have an account? Sign up"}
                    </button>

                </form>

            </div>
        </div>
    )
}