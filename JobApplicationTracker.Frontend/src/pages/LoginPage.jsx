import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

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
                <h1>{mode === 'signup' ? 'Create account' : 'Welcome back'}</h1>
                <p className="muted">
                    {mode === 'signup'
                        ? 'Sign up to start tracking applications'
                        : 'Log in to continue'}
                </p>

                <form onSubmit={handleSubmit} className="auth-form">
                    <label>
                        Username
                        <input
                            name="username"
                            type="text"
                            value={form.username}
                            onChange={handleChange}
                            autoComplete="username"
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
                            autoComplete={
                                mode === 'signup' ? 'new-password' : 'current-password'
                            }
                            required
                        />
                    </label>

                    {error && <p className="auth-error">{error}</p>}

                    <button type="submit" disabled={loading}>
                        {loading
                            ? 'Loading...'
                            : mode === 'signup'
                                ? 'Sign up'
                                : 'Log in'}
                    </button>
                </form>

                <button
                    type="button"
                    className="link-button"
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