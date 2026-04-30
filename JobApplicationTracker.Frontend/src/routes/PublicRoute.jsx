import { Navigate } from 'react-router-dom'
import { storage } from '../utils/storage'

export default function PublicRoute({ children }) {
    const token = storage.getToken()

    if (token) {
        return <Navigate to="/app" replace />
    }

    return children
}