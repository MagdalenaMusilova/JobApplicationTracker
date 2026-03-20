import { useState } from 'react'
import './App.css'

const SIGN_IN_URL = 'http://localhost:5131/api/auth/signin'
const CREATE_USER_URL = 'http://localhost:5131/api/users'

function App() {
  const [mode, setMode] = useState('signin')

  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')

  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')

  const resetMessages = () => {
    setError('')
    setSuccess('')
  }

  const handleSignIn = async (event) => {
    event.preventDefault()
    setLoading(true)
    resetMessages()

    try {
      const response = await fetch(SIGN_IN_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
      })

      const data = await response.json()

      if (!response.ok) {
        throw new Error(typeof data === 'string' ? data : 'Sign in failed')
      }

      localStorage.setItem('token', data.token)
      localStorage.setItem('username', data.username)

      setSuccess('Signed in successfully.')
      setPassword('')
      setConfirmPassword('')
    } catch (err) {
      setError(err.message || 'Something went wrong')
    } finally {
      setLoading(false)
    }
  }

  const handleCreateUser = async (event) => {
    event.preventDefault()
    setLoading(true)
    resetMessages()

    if (password !== confirmPassword) {
      setError('Passwords do not match.')
      setLoading(false)
      return
    }

    try {
      const response = await fetch(CREATE_USER_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
      })

      const data = await response.json()

      if (!response.ok) {
        throw new Error(typeof data === 'string' ? data : 'Create user failed')
      }

      setSuccess(`User "${data.username}" created successfully.`)
      setMode('signin')
      setPassword('')
      setConfirmPassword('')
    } catch (err) {
      setError(err.message || 'Something went wrong')
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = mode === 'signin' ? handleSignIn : handleCreateUser

  return (
    <main className="app">
      <div className="auth-card">
        <div className="auth-tabs">
          <button
            type="button"
            className={mode === 'signin' ? 'tab active' : 'tab'}
            onClick={() => {
              setMode('signin')
              resetMessages()
            }}
          >
            Sign In
          </button>
          <button
            type="button"
            className={mode === 'register' ? 'tab active' : 'tab'}
            onClick={() => {
              setMode('register')
              resetMessages()
            }}
          >
            Create User
          </button>
        </div>

        <h1>{mode === 'signin' ? 'Sign In' : 'Create User'}</h1>
        <p className="subtitle">
          {mode === 'signin'
            ? 'Use your account to continue.'
            : 'Create a new account.'}
        </p>

        <form onSubmit={handleSubmit} className="auth-form">
          <label>
            Username
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              autoComplete="username"
              required
            />
          </label>

          <label>
            Password
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              autoComplete={mode === 'signin' ? 'current-password' : 'new-password'}
              required
            />
          </label>

          {mode === 'register' && (
            <label>
              Confirm Password
              <input
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                autoComplete="new-password"
                required
              />
            </label>
          )}

          <button type="submit" disabled={loading}>
            {loading
              ? mode === 'signin'
                ? 'Signing in...'
                : 'Creating...'
              : mode === 'signin'
                ? 'Sign In'
                : 'Create User'}
          </button>
        </form>

        {error && <p className="status error">{error}</p>}
        {success && <p className="status success">{success}</p>}
      </div>
    </main>
  )
}

export default App
