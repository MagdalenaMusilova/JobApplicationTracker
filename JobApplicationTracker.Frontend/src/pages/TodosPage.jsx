function TodosPage({ todoItems, todoCalendar, setCreateOpen }) {
    return (
        <section className="page">
            <div className="page-header">
                <div>
                    <h1>ToDo’s</h1>
                    <p>Upcoming interviews, follow-ups, and calendar-based reminders.</p>
                </div>
                <button type="button" className="primary-btn" onClick={() => setCreateOpen(true)}>
                    + New Application
                </button>
            </div>

            <div className="detail-layout">
                <article className="card">
                    <h2>Calendar</h2>
                    <div className="calendar-grid">
                        {todoCalendar.map((day) => (
                            <div key={day.day} className="calendar-day">
                                <strong>{day.day}</strong>
                                <span>{day.items.length} items</span>
                            </div>
                        ))}
                    </div>
                </article>

                <article className="card">
                    <h2>Upcoming tasks</h2>
                    <div className="todo-list">
                        {todoItems.map((app) => (
                            <div key={app.id} className="todo-item">
                                <div>
                                    <strong>{app.company}</strong>
                                    <p>
                                        {app.title} · {app.nextStep}
                                    </p>
                                </div>
                                <div className="todo-meta">
                                    <span className="badge">{app.priority}</span>
                                    <span>{app.dueDate}</span>
                                </div>
                            </div>
                        ))}
                    </div>
                </article>
            </div>
        </section>
    )
}

export default TodosPage