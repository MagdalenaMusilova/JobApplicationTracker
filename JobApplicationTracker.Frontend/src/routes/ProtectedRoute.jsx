import { Navigate } from 'react-router-dom'
import { storage } from '../utils/storage'

export default function ProtectedRoute({ children }) {
    const token = storage.getToken()

    if (!token) {
        return <Navigate to="/login" replace />
    }

    return children
}