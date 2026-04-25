function Sidebar({ screen, setScreen, setCreateOpen, onLogout }) {
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
                    className={screen === 'profile' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('profile')}
                >
                    Profile
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
                    className={screen === 'todos' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('todos')}
                >
                    ToDo’s
                </button>
                <button
                    type="button"
                    className={screen === 'match' ? 'nav-item active' : 'nav-item'}
                    onClick={() => setScreen('match')}
                >
                    Match & Create
                </button>
            </nav>

            <div className="sidebar-actions">
                <button type="button" className="primary-btn" onClick={() => setCreateOpen(true)}>
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