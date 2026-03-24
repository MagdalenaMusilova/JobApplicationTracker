function Sidebar({ screen, setScreen, setCreateOpen }) {
    return (
        <aside className="sidebar">
            <div>
                <div className="brand">JobTracker</div>
                <p className="brand-subtitle">Frontend ready for backend integration</p>
            </div>

            <nav className="nav">
                <button
                    type="button"
                    className={screen === 'dashboard' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('dashboard')}
                >
                    Dashboard
                </button>
                <button
                    type="button"
                    className={screen === 'applications' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('applications')}
                >
                    Applications
                </button>
                <button
                    type="button"
                    className={screen === 'profile' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('profile')}
                >
                    Profile
                </button>
                <button
                    type="button"
                    className={screen === 'todos' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('todos')}
                >
                    ToDo’s
                </button>
            </nav>

            <div className="sidebar-actions">
                <button type="button" className="primary-btn" onClick={() => setCreateOpen(true)}>
                    + New Application
                </button>
            </div>
        </aside>
    )
}

export default Sidebar