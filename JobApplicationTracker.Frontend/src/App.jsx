import { useEffect, useState } from 'react'
import './App.css'

const API_URL = 'http://localhost:5131/api/User'

function App() {
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadUsers = async () => {
      try {
        const response = await fetch(API_URL)

        if (!response.ok) {
          throw new Error('Failed to load users')
        }

        const data = await response.json()
        setUsers(data)
      } catch (err) {
        setError(err.message || 'Something went wrong')
      } finally {
        setLoading(false)
      }
    }

    loadUsers()
  }, [])

  return (
    <main className="app">
      <h1>Users</h1>

      {loading && <p className="status">Loading users...</p>}

      {error && !loading && <p className="status error">{error}</p>}

      {!loading && !error && users.length === 0 && (
        <p className="status">No users found.</p>
      )}

      {!loading && !error && users.length > 0 && (
        <ul className="user-list">
          {users.map((user) => (
            <li key={user.id} className="user-card">
              <span className="user-id">#{user.id}</span>
              <span className="username">{user.username}</span>
            </li>
          ))}
        </ul>
      )}
    </main>
  )
}

export default App
