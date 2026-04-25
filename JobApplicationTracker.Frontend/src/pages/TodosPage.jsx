import { useState, useMemo } from 'react'

function isSameDay(a, b) {
    return (
        a.getFullYear() === b.getFullYear() &&
        a.getMonth() === b.getMonth() &&
        a.getDate() === b.getDate()
    )
}

function getTaskDate(app) {
    if (app.closestEvent?.eventDate) {
        const d = new Date(app.closestEvent.eventDate)
        if (!Number.isNaN(d.getTime())) return d
    }
    return null
}

function formatGroupDate(date) {
    return date.toLocaleDateString(undefined, { weekday: 'long', month: 'long', day: 'numeric' })
}

/**
 * Returns an array of Date objects representing the full calendar grid
 * for the given month/year. Pads with days from the previous and next
 * months so the grid always starts on Monday and ends on Sunday.
 */
function buildMonthGrid(year, month) {
    const firstOfMonth = new Date(year, month, 1)
    const lastOfMonth = new Date(year, month + 1, 0)

    // Monday-based: 0=Mon … 6=Sun
    const startOffset = firstOfMonth.getDay() === 0 ? 6 : firstOfMonth.getDay() - 1
    const endOffset = lastOfMonth.getDay() === 0 ? 0 : 7 - lastOfMonth.getDay()

    const cells = []

    for (let i = startOffset; i > 0; i--) {
        const d = new Date(firstOfMonth)
        d.setDate(firstOfMonth.getDate() - i)
        cells.push(d)
    }
    for (let d = new Date(firstOfMonth); d <= lastOfMonth; d.setDate(d.getDate() + 1)) {
        cells.push(new Date(d))
    }
    for (let i = 1; i <= endOffset; i++) {
        const d = new Date(lastOfMonth)
        d.setDate(lastOfMonth.getDate() + i)
        cells.push(d)
    }

    return cells
}

const WEEKDAY_LABELS = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']

function TodosPage({ todoItems, setCreateOpen }) {
    const [finished, setFinished] = useState(new Set())

    const today = useMemo(() => {
        const d = new Date()
        d.setHours(0, 0, 0, 0)
        return d
    }, [])

    const twoWeeksEnd = useMemo(() => {
        const d = new Date(today)
        d.setDate(today.getDate() + 13)
        return d
    }, [today])

    // Allow navigating months
    const [viewYear, setViewYear] = useState(today.getFullYear())
    const [viewMonth, setViewMonth] = useState(today.getMonth())

    const calendarCells = useMemo(() => buildMonthGrid(viewYear, viewMonth), [viewYear, viewMonth])

    const goToPrevMonth = () => {
        if (viewMonth === 0) { setViewMonth(11); setViewYear(y => y - 1) }
        else setViewMonth(m => m - 1)
    }

    const goToNextMonth = () => {
        if (viewMonth === 11) { setViewMonth(0); setViewYear(y => y + 1) }
        else setViewMonth(m => m + 1)
    }

    const toggleFinished = (id) => {
        setFinished((prev) => {
            const next = new Set(prev)
            if (next.has(id)) next.delete(id)
            else next.add(id)
            return next
        })
    }

    const daysWithTasks = useMemo(() => {
        const set = new Set()
        for (const app of todoItems) {
            const d = getTaskDate(app)
            if (d) set.add(d.toDateString())
        }
        return set
    }, [todoItems])

    const groupedTasks = useMemo(() => {
        const allDays = []
        const cursor = new Date(today)
        for (let i = 0; i < 14; i++) {
            allDays.push(new Date(cursor))
            cursor.setDate(cursor.getDate() + 1)
        }
        return allDays
            .map((day) => ({
                day,
                tasks: todoItems.filter((app) => {
                    const d = getTaskDate(app)
                    return d && isSameDay(d, day)
                }),
            }))
            .filter((g) => g.tasks.length > 0)
    }, [today, todoItems])

    const monthLabel = new Date(viewYear, viewMonth, 1).toLocaleDateString(undefined, {
        month: 'long',
        year: 'numeric',
    })

    const isCurrentMonthView = viewYear === today.getFullYear() && viewMonth === today.getMonth()

    return (
        <section className="page">
            <div className="page-header">
                <div>
                    <h1>ToDo's</h1>
                    <p>Upcoming interviews, follow-ups, and calendar-based reminders.</p>
                </div>
                <button type="button" className="primary-btn" onClick={() => setCreateOpen(true)}>
                    + New Application
                </button>
            </div>

            {/* ── Calendar ── */}
            <article className="card">
                <div className="todo-cal-nav">
                    <button type="button" className="todo-cal-nav-btn" onClick={goToPrevMonth}>‹</button>
                    <h2>{monthLabel}</h2>
                    <button type="button" className="todo-cal-nav-btn" onClick={goToNextMonth}>›</button>
                    {!isCurrentMonthView && (
                        <button
                            type="button"
                            className="secondary-btn"
                            style={{ marginLeft: 'auto', padding: '6px 14px', fontSize: 13 }}
                            onClick={() => { setViewYear(today.getFullYear()); setViewMonth(today.getMonth()) }}
                        >
                            Today
                        </button>
                    )}
                </div>

                <div className="todo-calendar">
                    {WEEKDAY_LABELS.map((label) => (
                        <div key={label} className="todo-cal-header">{label}</div>
                    ))}

                    {calendarCells.map((day) => {
                        const isToday = isSameDay(day, today)
                        const hasTask = daysWithTasks.has(day.toDateString())
                        const inWindow = day >= today && day <= twoWeeksEnd
                        const isCurrentMonth = day.getMonth() === viewMonth
                        const isPast = day < today

                        return (
                            <div
                                key={day.toDateString()}
                                className={[
                                    'todo-cal-day',
                                    isToday ? 'todo-cal-day--today' : '',
                                    hasTask ? 'todo-cal-day--has-tasks' : '',
                                    isPast ? 'todo-cal-day--past' : '',
                                    inWindow ? 'todo-cal-day--in-window' : '',
                                    !isCurrentMonth ? 'todo-cal-day--overflow' : '',
                                ]
                                    .filter(Boolean)
                                    .join(' ')}
                            >
                                <span className="todo-cal-num">{day.getDate()}</span>
                                {hasTask && <span className="todo-cal-dot" />}
                            </div>
                        )
                    })}
                </div>
            </article>

            {/* ── Grouped task list ── */}
            <article className="card">
                <h2>Upcoming tasks</h2>
                {groupedTasks.length === 0 ? (
                    <div className="empty-state" style={{ marginTop: 14 }}>
                        <strong>No scheduled tasks in the next 2 weeks</strong>
                        <p>Add scheduled events to your applications to see them here.</p>
                    </div>
                ) : (
                    <div className="todo-groups">
                        {groupedTasks.map(({ day, tasks }) => (
                            <div key={day.toDateString()} className="todo-group">
                                <p className="todo-group-label">{formatGroupDate(day)}</p>
                                <div className="todo-list">
                                    {tasks.map((app) => {
                                        const done = finished.has(app.id)
                                        return (
                                            <div
                                                key={app.id}
                                                className={['todo-item', done ? 'todo-item--done' : ''].filter(Boolean).join(' ')}
                                            >
                                                <button
                                                    type="button"
                                                    className="todo-check"
                                                    onClick={() => toggleFinished(app.id)}
                                                    aria-label={done ? 'Mark as not done' : 'Mark as done'}
                                                >
                                                    {done ? '✓' : ''}
                                                </button>
                                                <div className="todo-item-body">
                                                    <strong>{app.company}</strong>
                                                    <p>
                                                        {app.title}
                                                        {app.closestEvent?.eventType
                                                            ? ` · ${app.closestEvent.eventType}`
                                                            : app.nextStep
                                                                ? ` · ${app.nextStep}`
                                                                : ''}
                                                    </p>
                                                </div>
                                                <div className="todo-meta">
                                                    <span className="badge">{app.priority}</span>
                                                    <span className="badge">{app.status}</span>
                                                </div>
                                            </div>
                                        )
                                    })}
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </article>
        </section>
    )
}

export default TodosPage