import { useMemo, useState } from 'react'

const emptyEntryMessages = {
    education: 'No education entries yet.',
    workExperiences: 'No work experience entries yet.',
    trainings: 'No trainings yet.',
    skills: 'No skills yet.',
}

function ProfilePage({ profile }) {
    const [draft, setDraft] = useState(() => ({
        account: {
            username: profile.account?.username ?? '',
            email: profile.account?.email ?? '',
            password: '',
        },
        aboutMe: profile.aboutMe ?? '',
        education: profile.education ?? [],
        workExperiences: profile.workExperiences ?? [],
        trainings: profile.trainings ?? [],
        skills: profile.skills ?? [],
        notes: profile.notes ?? '',
    }))

    const [editing, setEditing] = useState({
        account: false,
        aboutMe: false,
        notes: false,
        education: false,
        workExperiences: false,
        trainings: false,
        skills: false,
    })

    const [saveState, setSaveState] = useState({
        account: false,
        aboutMe: false,
        notes: false,
        education: {},
        workExperiences: {},
        trainings: {},
        skills: {},
    })

    const formatDate = (value) => {
        if (!value) return '—'
        const date = new Date(value)
        return Number.isNaN(date.getTime()) ? String(value) : date.toLocaleDateString()
    }

    const formatList = (items) => (items?.length ? items.join(', ') : '—')

    const normalizeTextList = (value) =>
        value
            .split(',')
            .map((item) => item.trim())
            .filter(Boolean)

    const startEditing = (section) => {
        setEditing((current) => ({ ...current, [section]: true }))
    }

    const stopEditing = (section) => {
        setEditing((current) => ({ ...current, [section]: false }))
    }

    const updateAccount = (field, value) => {
        setDraft((current) => ({
            ...current,
            account: { ...current.account, [field]: value },
        }))
        setSaveState((current) => ({ ...current, account: false }))
    }

    const updateItem = (section, index, field, value) => {
        setDraft((current) => ({
            ...current,
            [section]: current[section].map((item, itemIndex) =>
                itemIndex === index ? { ...item, [field]: value } : item,
            ),
        }))
        setSaveState((current) => ({
            ...current,
            [section]: { ...current[section], [index]: false },
        }))
    }

    const updateNotes = (value) => {
        setDraft((current) => ({ ...current, notes: value }))
        setSaveState((current) => ({ ...current, notes: false }))
    }

    const removeItem = (section, index) => {
        setDraft((current) => ({
            ...current,
            [section]: current[section].filter((_, itemIndex) => itemIndex !== index),
        }))
        setSaveState((current) => {
            const updatedSection = { ...current[section] }
            delete updatedSection[index]
            return { ...current, [section]: updatedSection }
        })
    }

    const addItem = (section, item) => {
        setDraft((current) => ({
            ...current,
            [section]: [...current[section], { id: crypto.randomUUID(), ...item }],
        }))
        setEditing((current) => ({ ...current, [section]: true }))
    }

    const saveAccount = () => {
        setSaveState((current) => ({ ...current, account: true }))
        setEditing((current) => ({ ...current, account: false }))
    }

    const saveSection = (section) => {
        setSaveState((current) => ({ ...current, [section]: true }))
        setEditing((current) => ({ ...current, [section]: false }))
    }

    const saveItem = (section, index) => {
        setSaveState((current) => ({
            ...current,
            [section]: { ...current[section], [index]: true },
        }))
        setEditing((current) => ({ ...current, [section]: false }))
    }

    const stats = useMemo(
        () => [
            {
                label: 'Education',
                value: draft.education.length,
                hint: draft.education.length === 1 ? 'entry' : 'entries',
            },
            {
                label: 'Work',
                value: draft.workExperiences.length,
                hint: draft.workExperiences.length === 1 ? 'entry' : 'entries',
            },
            {
                label: 'Trainings',
                value: draft.trainings.length,
                hint: draft.trainings.length === 1 ? 'entry' : 'entries',
            },
            {
                label: 'Skills',
                value: draft.skills.length,
                hint: draft.skills.length === 1 ? 'entry' : 'entries',
            },
        ],
        [draft.education.length, draft.workExperiences.length, draft.trainings.length, draft.skills.length],
    )

    const renderTextValue = (value) => <span className="read-only-value">{value || '—'}</span>

    const renderEditableList = (sectionKey, title, addButtonLabel, createItem, renderFields, summarizeItem) => (
        <article className="card profile-card">
            <div className="section-header compact">
                <div>
                    <h2>{title}</h2>
                    <p className="muted">
                        {draft[sectionKey].length === 0
                            ? emptyEntryMessages[sectionKey]
                            : `${draft[sectionKey].length} item${draft[sectionKey].length === 1 ? '' : 's'}`}
                    </p>
                </div>

                <div className="section-actions">
                    {editing[sectionKey] ? (
                        <>
                            <button type="button" className="secondary-btn" onClick={() => saveSection(sectionKey)}>
                                {saveState[sectionKey] === true ? 'Saved' : 'Save'}
                            </button>
                            <button type="button" className="secondary-btn" onClick={() => stopEditing(sectionKey)}>
                                Done
                            </button>
                        </>
                    ) : (
                        <button type="button" className="secondary-btn" onClick={() => startEditing(sectionKey)}>
                            Edit
                        </button>
                    )}

                    <button type="button" className="secondary-btn" onClick={() => addItem(sectionKey, createItem())}>
                        + {addButtonLabel}
                    </button>
                </div>
            </div>

            <div className="stack-list">
                {draft[sectionKey].length === 0 ? (
                    <div className="empty-state profile-empty-state">
                        <h3>Nothing here yet</h3>
                        <p>{emptyEntryMessages[sectionKey]}</p>
                    </div>
                ) : (
                    draft[sectionKey].map((item, index) => (
                        <div key={item.id ?? index} className="editable-item">
                            <div className="editable-item-header">
                                <div>
                                    <h3>{item.degree || item.name || item.position || 'Untitled entry'}</h3>
                                    <p className="muted">{summarizeItem(item)}</p>
                                </div>
                                <div className="item-status-row">
                                    {saveState[sectionKey]?.[index] ? <span className="save-pill">Saved</span> : null}
                                </div>
                            </div>

                            {editing[sectionKey] ? (
                                <div className="editable-item-stack">{renderFields(item, index, false)}</div>
                            ) : (
                                <div className="read-only-stack">{renderFields(item, index, true)}</div>
                            )}

                            {editing[sectionKey] ? (
                                <div className="item-actions">
                                    <button
                                        type="button"
                                        className="secondary-btn"
                                        onClick={() => saveItem(sectionKey, index)}
                                    >
                                        {saveState[sectionKey]?.[index] ? 'Saved' : 'Save'}
                                    </button>
                                    <button
                                        type="button"
                                        className="secondary-btn danger"
                                        onClick={() => removeItem(sectionKey, index)}
                                    >
                                        Delete
                                    </button>
                                </div>
                            ) : null}
                        </div>
                    ))
                )}
            </div>
        </article>
    )

    return (
        <section className="page profile-page">
            <div className="page-header profile-hero card">
                <div className="profile-hero-copy">
                    <span className="eyebrow">Profile</span>
                    <h1>Make your profile feel polished</h1>
                    <p>Compact, clean, and easy to scan — with editing hidden until you need it.</p>
                </div>

                <div className="profile-hero-card">
                    <div className="profile-avatar">
                        {(draft.account.username || 'U').slice(0, 1).toUpperCase()}
                    </div>
                    <div>
                        <p className="profile-role">{draft.account.username || 'Your username'}</p>
                        <p className="muted">{draft.account.email || 'your.email@example.com'}</p>
                    </div>
                </div>
            </div>

{/*            <div className="profile-summary-grid">
                {stats.map((stat) => (
                    <article key={stat.label} className="card profile-summary-card">
                        <span className="stat-kicker">{stat.label}</span>
                        <strong className="stat-value">{stat.value}</strong>
                        <span className="stat-label">{stat.hint}</span>
                    </article>
                ))}
            </div>*/}

            <div className="profile-layout">
                <article className="card profile-card profile-card-primary">
                    <div className="section-header compact">
                        <div>
                            <h2>Account</h2>
                            <p className="muted">Keep your login details up to date.</p>
                        </div>

                        {editing.account ? (
                            <>
                                <button type="button" className="secondary-btn" onClick={saveAccount}>
                                    {saveState.account ? 'Saved' : 'Save'}
                                </button>
                                <button type="button" className="secondary-btn" onClick={() => stopEditing('account')}>
                                    Done
                                </button>
                            </>
                        ) : (
                            <button type="button" className="secondary-btn" onClick={() => startEditing('account')}>
                                Edit
                            </button>
                        )}
                    </div>

                    <div className="profile-form-stack">
                        <div className="info-row">
                            <span className="info-label">Username</span>
                            {editing.account ? (
                                <input
                                    type="text"
                                    value={draft.account.username}
                                    onChange={(e) => updateAccount('username', e.target.value)}
                                    placeholder="Your username"
                                />
                            ) : (
                                renderTextValue(draft.account.username)
                            )}
                        </div>

                        <div className="info-row">
                            <span className="info-label">Email</span>
                            {editing.account ? (
                                <input
                                    type="email"
                                    value={draft.account.email}
                                    onChange={(e) => updateAccount('email', e.target.value)}
                                    placeholder="you@example.com"
                                />
                            ) : (
                                renderTextValue(draft.account.email)
                            )}
                        </div>

                        {editing.account ? (
                            <div className="info-row">
                                <span className="info-label">Password</span>
                                <input
                                    type="password"
                                    value={draft.account.password}
                                    onChange={(e) => updateAccount('password', e.target.value)}
                                    placeholder="Leave blank to keep current password"
                                />
                            </div>
                        ) : null}
                    </div>
                </article>

                <article className="card profile-card profile-card-secondary">
                    <div className="section-header compact">
                        <div>
                            <h2>About me</h2>
                            <p className="muted">A short summary helps people understand you faster.</p>
                        </div>

                        {editing.aboutMe ? (
                            <>
                                <button
                                    type="button"
                                    className="secondary-btn"
                                    onClick={() => {
                                        setSaveState((current) => ({ ...current, aboutMe: true }))
                                        stopEditing('aboutMe')
                                    }}
                                >
                                    {saveState.aboutMe ? 'Saved' : 'Save'}
                                </button>
                                <button type="button" className="secondary-btn" onClick={() => stopEditing('aboutMe')}>
                                    Done
                                </button>
                            </>
                        ) : (
                            <button type="button" className="secondary-btn" onClick={() => startEditing('aboutMe')}>
                                Edit
                            </button>
                        )}
                    </div>

                    {editing.aboutMe ? (
                        <textarea
                            rows="4"
                            value={draft.aboutMe}
                            onChange={(e) => {
                                setDraft((current) => ({ ...current, aboutMe: e.target.value }))
                                setSaveState((current) => ({ ...current, aboutMe: false }))
                            }}
                            placeholder="Add a short personal summary..."
                        />
                    ) : (
                        <p className="read-only-block">{draft.aboutMe || 'No summary added yet.'}</p>
                    )}
                </article>

                {renderEditableList(
                    'education',
                    'Education',
                    'Add education',
                    () => ({
                        degree: '',
                        isFinished: false,
                        school: '',
                        majors: [],
                        skills: [],
                        notes: '',
                    }),
                    (item, index, readOnly = false) =>
                        readOnly ? (
                            <div className="read-only-grid">
                                <div><span className="info-label">Degree</span>{renderTextValue(item.degree)}</div>
                                <div><span className="info-label">School</span>{renderTextValue(item.school)}</div>
                                <div><span className="info-label">Status</span>{renderTextValue(item.isFinished ? 'Finished' : 'In progress')}</div>
                                <div><span className="info-label">Majors</span>{renderTextValue(formatList(item.majors))}</div>
                                <div><span className="info-label">Skills</span>{renderTextValue(formatList(item.skills))}</div>
                                <div><span className="info-label">Notes</span>{renderTextValue(item.notes)}</div>
                            </div>
                        ) : (
                            <>
                                <label className="field">
                                    <span>Degree</span>
                                    <input
                                        type="text"
                                        value={item.degree ?? ''}
                                        onChange={(e) => updateItem('education', index, 'degree', e.target.value)}
                                    />
                                </label>
                                <label className="field field-inline">
                                    <span>Finished</span>
                                    <input
                                        type="checkbox"
                                        checked={!!item.isFinished}
                                        onChange={(e) => updateItem('education', index, 'isFinished', e.target.checked)}
                                    />
                                </label>
                                <label className="field">
                                    <span>School</span>
                                    <input
                                        type="text"
                                        value={item.school ?? ''}
                                        onChange={(e) => updateItem('education', index, 'school', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Majors</span>
                                    <input
                                        type="text"
                                        value={formatList(item.majors)}
                                        onChange={(e) =>
                                            updateItem('education', index, 'majors', normalizeTextList(e.target.value))
                                        }
                                        placeholder="Comma separated"
                                    />
                                </label>
                                <label className="field">
                                    <span>Skills</span>
                                    <input
                                        type="text"
                                        value={formatList(item.skills)}
                                        onChange={(e) =>
                                            updateItem('education', index, 'skills', normalizeTextList(e.target.value))
                                        }
                                        placeholder="Comma separated"
                                    />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea
                                        rows="3"
                                        value={item.notes ?? ''}
                                        onChange={(e) => updateItem('education', index, 'notes', e.target.value)}
                                        placeholder="Anything worth remembering..."
                                    />
                                </label>
                            </>
                        ),
                    (item) => `${item.school || 'School not set'} · ${item.isFinished ? 'Finished' : 'In progress'}`,
                )}

                {renderEditableList(
                    'workExperiences',
                    'Work experience',
                    'Add work experience',
                    () => ({
                        startDate: '',
                        endDate: '',
                        company: '',
                        position: '',
                        jobDescription: [],
                        skills: [],
                        notes: '',
                    }),
                    (item, index, readOnly = false) =>
                        readOnly ? (
                            <div className="read-only-grid">
                                <div><span className="info-label">Company</span>{renderTextValue(item.company)}</div>
                                <div><span className="info-label">Position</span>{renderTextValue(item.position)}</div>
                                <div><span className="info-label">Dates</span>{renderTextValue(`${formatDate(item.startDate)} — ${formatDate(item.endDate)}`)}</div>
                                <div><span className="info-label">Description</span>{renderTextValue(formatList(item.jobDescription))}</div>
                                <div><span className="info-label">Notes</span>{renderTextValue(item.notes)}</div>
                            </div>
                        ) : (
                            <>
                                <label className="field">
                                    <span>Company</span>
                                    <input
                                        type="text"
                                        value={item.company ?? ''}
                                        onChange={(e) => updateItem('workExperiences', index, 'company', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Position</span>
                                    <input
                                        type="text"
                                        value={item.position ?? ''}
                                        onChange={(e) => updateItem('workExperiences', index, 'position', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Start date</span>
                                    <input
                                        type="date"
                                        value={item.startDate ?? ''}
                                        onChange={(e) => updateItem('workExperiences', index, 'startDate', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>End date</span>
                                    <input
                                        type="date"
                                        value={item.endDate ?? ''}
                                        onChange={(e) => updateItem('workExperiences', index, 'endDate', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Job description</span>
                                    <textarea
                                        rows="3"
                                        value={formatList(item.jobDescription)}
                                        onChange={(e) =>
                                            updateItem(
                                                'workExperiences',
                                                index,
                                                'jobDescription',
                                                normalizeTextList(e.target.value),
                                            )
                                        }
                                        placeholder="Comma separated"
                                    />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea
                                        rows="3"
                                        value={item.notes ?? ''}
                                        onChange={(e) => updateItem('workExperiences', index, 'notes', e.target.value)}
                                        placeholder="Anything worth remembering..."
                                    />
                                </label>
                            </>
                        ),
                    (item) => `${item.company || 'Company not set'} · ${formatDate(item.startDate)} — ${formatDate(item.endDate)}`,
                )}

                {renderEditableList(
                    'trainings',
                    'Trainings',
                    'Add training',
                    () => ({
                        startDate: '',
                        endDate: '',
                        name: '',
                        type: '',
                        certification: [],
                        skills: [],
                        notes: '',
                    }),
                    (item, index, readOnly = false) =>
                        readOnly ? (
                            <div className="read-only-grid">
                                <div><span className="info-label">Name</span>{renderTextValue(item.name)}</div>
                                <div><span className="info-label">Type</span>{renderTextValue(item.type)}</div>
                                <div><span className="info-label">Dates</span>{renderTextValue(`${formatDate(item.startDate)} — ${formatDate(item.endDate)}`)}</div>
                                <div><span className="info-label">Certification</span>{renderTextValue(formatList(item.certification))}</div>
                                <div><span className="info-label">Notes</span>{renderTextValue(item.notes)}</div>
                            </div>
                        ) : (
                            <>
                                <label className="field">
                                    <span>Name</span>
                                    <input
                                        type="text"
                                        value={item.name ?? ''}
                                        onChange={(e) => updateItem('trainings', index, 'name', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Type</span>
                                    <input
                                        type="text"
                                        value={item.type ?? ''}
                                        onChange={(e) => updateItem('trainings', index, 'type', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Start date</span>
                                    <input
                                        type="date"
                                        value={item.startDate ?? ''}
                                        onChange={(e) => updateItem('trainings', index, 'startDate', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>End date</span>
                                    <input
                                        type="date"
                                        value={item.endDate ?? ''}
                                        onChange={(e) => updateItem('trainings', index, 'endDate', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Certification</span>
                                    <textarea
                                        rows="3"
                                        value={formatList(item.certification)}
                                        onChange={(e) =>
                                            updateItem(
                                                'trainings',
                                                index,
                                                'certification',
                                                normalizeTextList(e.target.value),
                                            )
                                        }
                                        placeholder="Comma separated"
                                    />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea
                                        rows="3"
                                        value={item.notes ?? ''}
                                        onChange={(e) => updateItem('trainings', index, 'notes', e.target.value)}
                                        placeholder="Anything worth remembering..."
                                    />
                                </label>
                            </>
                        ),
                    (item) => `${item.type || 'Type not set'} · ${formatDate(item.startDate)} — ${formatDate(item.endDate)}`,
                )}

                {renderEditableList(
                    'skills',
                    'Skills',
                    'Add skill',
                    () => ({
                        name: '',
                        aliases: [],
                        level: '',
                        weight: '',
                        notes: '',
                    }),
                    (item, index, readOnly = false) =>
                        readOnly ? (
                            <div className="read-only-grid">
                                <div><span className="info-label">Name</span>{renderTextValue(item.name)}</div>
                                <div><span className="info-label">Aliases</span>{renderTextValue(formatList(item.aliases))}</div>
                                <div><span className="info-label">Level</span>{renderTextValue(item.level)}</div>
                                <div><span className="info-label">Weight</span>{renderTextValue(item.weight)}</div>
                                <div><span className="info-label">Notes</span>{renderTextValue(item.notes)}</div>
                            </div>
                        ) : (
                            <>
                                <label className="field">
                                    <span>Name</span>
                                    <input
                                        type="text"
                                        value={item.name ?? ''}
                                        onChange={(e) => updateItem('skills', index, 'name', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Aliases</span>
                                    <input
                                        type="text"
                                        value={formatList(item.aliases)}
                                        onChange={(e) =>
                                            updateItem('skills', index, 'aliases', normalizeTextList(e.target.value))
                                        }
                                        placeholder="Comma separated"
                                    />
                                </label>
                                <label className="field">
                                    <span>Level</span>
                                    <input
                                        type="text"
                                        value={item.level ?? ''}
                                        onChange={(e) => updateItem('skills', index, 'level', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Weight</span>
                                    <input
                                        type="text"
                                        value={item.weight ?? ''}
                                        onChange={(e) => updateItem('skills', index, 'weight', e.target.value)}
                                    />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea
                                        rows="3"
                                        value={item.notes ?? ''}
                                        onChange={(e) => updateItem('skills', index, 'notes', e.target.value)}
                                        placeholder="Anything worth remembering..."
                                    />
                                </label>
                            </>
                        ),
                    (item) => `${item.level || 'No level set'} · Weight: ${item.weight || '—'}`,
                )}

                <article className="card profile-card profile-notes-card">
                    <div className="section-header compact">
                        <div>
                            <h2>Notes</h2>
                            <p className="muted">General notes about the profile.</p>
                        </div>

                        {editing.notes ? (
                            <>
                                <button
                                    type="button"
                                    className="secondary-btn"
                                    onClick={() => {
                                        setSaveState((current) => ({ ...current, notes: true }))
                                        stopEditing('notes')
                                    }}
                                >
                                    {saveState.notes ? 'Saved' : 'Save'}
                                </button>
                                <button type="button" className="secondary-btn" onClick={() => stopEditing('notes')}>
                                    Done
                                </button>
                            </>
                        ) : (
                            <button type="button" className="secondary-btn" onClick={() => startEditing('notes')}>
                                Edit
                            </button>
                        )}
                    </div>

                    {editing.notes ? (
                        <textarea
                            rows="4"
                            value={draft.notes}
                            onChange={(e) => updateNotes(e.target.value)}
                            placeholder="General notes about the profile..."
                        />
                    ) : (
                        <p className="read-only-block">{draft.notes || 'No notes yet.'}</p>
                    )}
                </article>
            </div>
        </section>
    )
}

export default ProfilePage