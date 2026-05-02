import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import  './LoginPage.css'

import { AuthAPI } from '../api/auth'
import { storage } from '../utils/storage'

export default function LoginPage() {
    const navigate = useNavigate()

    const [mode, setMode] = useState('signin') // 'signin' | 'signup'
    const [form, setForm] = useState({
        username: '',
        password: '',
    })

    const [loading, setLoading] = useState(false)
    const [error, setError] = useState('')

    const handleChange = (e) => {
        setForm({
            ...form,
            [e.target.name]: e.target.value,
        })
    }

    const handleSubmit = async (e) => {
        e.preventDefault()

        try {
            setLoading(true)
            setError('')

            // SIGN UP (optional step)
            if (mode === 'signup') {
                await AuthAPI.signup({
                    username: form.username,
                    password: form.password,
                })
            }

            // SIGN IN
            const res = await AuthAPI.signin({
                username: form.username,
                password: form.password,
            })

            const token = res?.token ?? res?.Token

            if (!token) {
                throw new Error('No token returned from server')
            }

            // store auth
            storage.setToken(token)

            // redirect into app
            navigate('/app', { replace: true })
        } catch (err) {
            setError(err.message || 'Login failed')
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="auth-shell">
            <section className="auth-card">
                <div className="auth-header">
                    <h1>{mode === 'signup' ? 'Create account' : 'Welcome back'}</h1>
                    <p>
                        {mode === 'signup'
                            ? 'Start tracking your job applications'
                            : 'Log in to continue'}
                    </p>
                </div>

                <form onSubmit={handleSubmit} className="auth-form">
                    <div className="form-group">
                        <label>Username</label>
                        <input
                            name="username"
                            type="text"
                            value={form.username}
                            onChange={handleChange}
                            autoComplete="username"
                            placeholder="Enter your username"
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label>Password</label>
                        <input
                            name="password"
                            type="password"
                            value={form.password}
                            onChange={handleChange}
                            autoComplete={
                                mode === 'signup' ? 'new-password' : 'current-password'
                            }
                            placeholder="Enter your password"
                            required
                        />
                    </div>

                    {error && <div className="auth-error">{error}</div>}

                    <button className="auth-submit" type="submit" disabled={loading}>
                        {loading
                            ? 'Please wait...'
                            : mode === 'signup'
                                ? 'Create account'
                                : 'Log in'}
                    </button>
                </form>

                <div className="auth-divider">
                    <span>or</span>
                </div>

                <button
                    type="button"
                    className="auth-toggle"
                    onClick={() =>
                        setMode((m) => (m === 'signup' ? 'signin' : 'signup'))
                    }
                >
                    {mode === 'signup'
                        ? 'Already have an account? Log in'
                        : "Don't have an account? Sign up"}
                </button>
            </section>
        </div>
    )
}