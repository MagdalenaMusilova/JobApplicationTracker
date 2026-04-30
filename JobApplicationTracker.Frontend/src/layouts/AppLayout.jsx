import { Outlet } from 'react-router-dom'
import Sidebar from '../components/Sidebar'

export default function AppLayout({ onLogout }) {
    return (
        <div className="shell">
            <Sidebar onLogout={onLogout} />

            <main className="content">
                <Outlet />
            </main>
        </div>
    )
}