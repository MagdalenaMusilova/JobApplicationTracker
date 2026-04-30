import { useNavigate, useLocation } from 'react-router-dom'

function Sidebar({ setCreateOpen, onLogout }) {
    const navigate = useNavigate()
    const location = useLocation()

    const isActive = (path) => location.pathname === path

    return (
        <aside className="sidebar">
            <div>
                <div className="brand">JobTracker</div>
                <p className="brand-subtitle">Frontend ready for backend integration</p>
            </div>

            <nav className="nav">
                <button
                    type="button"
                    className={isActive('/dashboard') ? 'nav-item active' : 'nav-item'}
                    onClick={() => navigate('/dashboard')}
                >
                    Dashboard
                </button>

                <button
                    type="button"
                    className={isActive('/profile') ? 'nav-item active' : 'nav-item'}
                    onClick={() => navigate('/profile')}
                >
                    Profile
                </button>

                <button
                    type="button"
                    className={isActive('/applications') ? 'nav-item active' : 'nav-item'}
                    onClick={() => navigate('/applications')}
                >
                    Applications
                </button>

                <button
                    type="button"
                    className={isActive('/todos') ? 'nav-item active' : 'nav-item'}
                    onClick={() => navigate('/todos')}
                >
                    ToDo’s
                </button>

                <button
                    type="button"
                    className={isActive('/match') ? 'nav-item active' : 'nav-item'}
                    onClick={() => navigate('/match')}
                >
                    Match & Create
                </button>
            </nav>

            <div className="sidebar-actions">
                <button
                    type="button"
                    className="primary-btn"
                    onClick={() => setCreateOpen(true)}
                >
                    + New Application
                </button>

                <button className="secondary-btn" onClick={onLogout}>
                    Log out
                </button>
            </div>
        </aside>
    )
}

export default Sidebar