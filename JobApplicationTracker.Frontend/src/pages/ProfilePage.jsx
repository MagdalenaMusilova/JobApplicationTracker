import { useRef, useState } from 'react'

const emptyEntryMessages = {
    education: 'No education entries yet.',
    workExperiences: 'No work experience entries yet.',
    trainings: 'No trainings yet.',
    skills: 'No skills yet.',
}

function buildInitialDraft(profile) {
    return {
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
    }
}

function ProfilePage({ profile }) {
    const [draft, setDraft] = useState(() => buildInitialDraft(profile))
    const [savedDraft, setSavedDraft] = useState(() => buildInitialDraft(profile))
    const [isEditing, setIsEditing] = useState(false)
    const [saved, setSaved] = useState(false)

    // Resume upload state
    const [resumeFile, setResumeFile] = useState(null)       // uploaded file
    const [extractedData, setExtractedData] = useState(null) // parsed result
    const [isExtracting, setIsExtracting] = useState(false)
    const [resumeError, setResumeError] = useState('')
    const fileInputRef = useRef(null)

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

    // ── Batch edit controls ──────────────────────────────────────────────────

    const startEditing = () => {
        setSaved(false)
        setIsEditing(true)
    }

    const cancelEditing = () => {
        setDraft(savedDraft)
        setIsEditing(false)
    }

    const saveAll = () => {
        setSavedDraft(draft)
        setSaved(true)
        setIsEditing(false)
    }

    // ── Field updaters ───────────────────────────────────────────────────────

    const updateAccount = (field, value) => {
        setDraft((cur) => ({ ...cur, account: { ...cur.account, [field]: value } }))
    }

    const updateField = (field, value) => {
        setDraft((cur) => ({ ...cur, [field]: value }))
    }

    const updateItem = (section, index, field, value) => {
        setDraft((cur) => ({
            ...cur,
            [section]: cur[section].map((item, i) => (i === index ? { ...item, [field]: value } : item)),
        }))
    }

    const removeItem = (section, index) => {
        setDraft((cur) => ({ ...cur, [section]: cur[section].filter((_, i) => i !== index) }))
    }

    const addItem = (section, item) => {
        setDraft((cur) => ({ ...cur, [section]: [...cur[section], { id: crypto.randomUUID(), ...item }] }))
    }

    // ── Resume upload ────────────────────────────────────────────────────────

    const handleFileChange = (e) => {
        const file = e.target.files?.[0]
        if (!file) return
        setResumeFile(file)
        setExtractedData(null) // new file invalidates old extraction
        setResumeError('')
    }

    const handleExtract = async () => {
        if (!resumeFile) return
        setIsExtracting(true)
        setResumeError('')
        try {
            const formData = new FormData()
            formData.append('File', resumeFile)
            const res = await fetch('/api/resume/extract', { method: 'POST', body: formData })
            if (!res.ok) throw new Error(await res.text())
            const data = await res.json()
            setExtractedData(data)
        } catch (err) {
            setResumeError(err.message || 'Failed to extract resume.')
        } finally {
            setIsExtracting(false)
        }
    }

    const resetResume = () => {
        setResumeFile(null)
        setExtractedData(null)
        setResumeError('')
        if (fileInputRef.current) fileInputRef.current.value = ''
    }

    const applyResume = (merge) => {
        const extracted = extractedData
        setDraft((cur) => {
            if (!merge) {
                return { ...buildInitialDraft(extracted), account: cur.account }
            }
            return {
                ...cur,
                aboutMe: cur.aboutMe || extracted.aboutMe || '',
                education: [
                    ...cur.education,
                    ...(extracted.education ?? []).map((e) => ({ id: crypto.randomUUID(), ...e })),
                ],
                workExperiences: [
                    ...cur.workExperiences,
                    ...(extracted.workExperiences ?? []).map((e) => ({ id: crypto.randomUUID(), ...e })),
                ],
                trainings: [
                    ...cur.trainings,
                    ...(extracted.trainings ?? []).map((e) => ({ id: crypto.randomUUID(), ...e })),
                ],
                skills: [
                    ...cur.skills,
                    ...(extracted.skills ?? []).map((e) => ({ id: crypto.randomUUID(), ...e })),
                ],
                notes: cur.notes
                    ? `${cur.notes}\n\n--- From resume ---\n${extracted.notes ?? ''}`
                    : extracted.notes ?? '',
            }
        })
        resetResume()
        setIsEditing(true)
    }

    // ── Render helpers ───────────────────────────────────────────────────────

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

                {isEditing && (
                    <button
                        type="button"
                        className="secondary-btn"
                        onClick={() => addItem(sectionKey, createItem())}
                    >
                        + {addButtonLabel}
                    </button>
                )}
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
                            </div>

                            {isEditing ? (
                                <div className="editable-item-stack">{renderFields(item, index, false)}</div>
                            ) : (
                                <div className="read-only-stack">{renderFields(item, index, true)}</div>
                            )}

                            {isEditing && (
                                <div className="item-actions">
                                    <button
                                        type="button"
                                        className="secondary-btn danger"
                                        onClick={() => removeItem(sectionKey, index)}
                                    >
                                        Delete
                                    </button>
                                </div>
                            )}
                        </div>
                    ))
                )}
            </div>
        </article>
    )

    return (
        <section className="page profile-page">
            {/* ── Hero ── */}
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

                <div className="profile-batch-toolbar">
                    {isEditing ? (
                        <>
                            <button type="button" className="primary-btn" onClick={saveAll}>
                                Save all
                            </button>
                            <button type="button" className="secondary-btn" onClick={cancelEditing}>
                                Cancel
                            </button>
                        </>
                    ) : (
                        <button type="button" className="primary-btn" onClick={startEditing}>
                            {saved ? '✓ Saved — Edit again' : 'Edit profile'}
                        </button>
                    )}
                </div>
            </div>

            <div className="profile-layout">
                {/* ── Resume upload ── */}
                <article className="card profile-card profile-resume-card">
                    <div className="section-header compact">
                        <div>
                            <h2>Import from PDF resume</h2>
                            <p className="muted">Upload your CV to populate or enrich your profile automatically.</p>
                        </div>
                    </div>

                    {/* Row 1: file picker + extract */}
                    <div className="resume-upload-area">
                        <input
                            ref={fileInputRef}
                            type="file"
                            accept=".pdf"
                            id="resume-file-input"
                            className="resume-file-input"
                            onChange={handleFileChange}
                        />
                        <label htmlFor="resume-file-input" className="resume-file-label">
                            {resumeFile ? resumeFile.name : 'Choose PDF…'}
                        </label>

                        <button
                            type="button"
                            className="secondary-btn"
                            onClick={handleExtract}
                            disabled={!resumeFile || isExtracting || !!extractedData}
                        >
                            {isExtracting ? 'Extracting…' : 'Extract'}
                        </button>
                    </div>

                    {/* Row 2: add + merge */}
                    <div className="resume-apply-area">
                        <button
                            type="button"
                            className="primary-btn"
                            onClick={() => applyResume(false)}
                            disabled={!extractedData}
                        >
                            Add
                        </button>

                        <button
                            type="button"
                            className="secondary-btn"
                            onClick={() => applyResume(true)}
                            disabled={!extractedData}
                        >
                            Merge
                        </button>

                        {extractedData && (
                            <span className="resume-ready-badge">✓ Extracted</span>
                        )}
                    </div>

                    {resumeError && (
                        <p className="resume-error">{resumeError}</p>
                    )}
                </article>

                {/* ── Account ── */}
                <article className="card profile-card profile-card-primary">
                    <div className="section-header compact">
                        <div>
                            <h2>Account</h2>
                            <p className="muted">Keep your login details up to date.</p>
                        </div>
                    </div>

                    <div className="profile-form-stack">
                        <div className="info-row">
                            <span className="info-label">Username</span>
                            {isEditing ? (
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
                            {isEditing ? (
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

                        {isEditing && (
                            <div className="info-row">
                                <span className="info-label">Password</span>
                                <input
                                    type="password"
                                    value={draft.account.password}
                                    onChange={(e) => updateAccount('password', e.target.value)}
                                    placeholder="Leave blank to keep current password"
                                />
                            </div>
                        )}
                    </div>
                </article>

                {/* ── About me ── */}
                <article className="card profile-card profile-card-secondary">
                    <div className="section-header compact">
                        <div>
                            <h2>About me</h2>
                            <p className="muted">A short summary helps people understand you faster.</p>
                        </div>
                    </div>

                    {isEditing ? (
                        <textarea
                            rows="4"
                            value={draft.aboutMe}
                            onChange={(e) => updateField('aboutMe', e.target.value)}
                            placeholder="Add a short personal summary..."
                        />
                    ) : (
                        <p className="read-only-block">{draft.aboutMe || 'No summary added yet.'}</p>
                    )}
                </article>

                {/* ── Education ── */}
                {renderEditableList(
                    'education',
                    'Education',
                    'Add education',
                    () => ({ degree: '', isFinished: false, school: '', majors: [], skills: [], notes: '' }),
                    (item, index, readOnly) =>
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
                                    <input type="text" value={item.degree ?? ''} onChange={(e) => updateItem('education', index, 'degree', e.target.value)} />
                                </label>
                                <label className="field field-inline">
                                    <span>Finished</span>
                                    <input type="checkbox" checked={!!item.isFinished} onChange={(e) => updateItem('education', index, 'isFinished', e.target.checked)} />
                                </label>
                                <label className="field">
                                    <span>School</span>
                                    <input type="text" value={item.school ?? ''} onChange={(e) => updateItem('education', index, 'school', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Majors</span>
                                    <input type="text" value={formatList(item.majors)} onChange={(e) => updateItem('education', index, 'majors', normalizeTextList(e.target.value))} placeholder="Comma separated" />
                                </label>
                                <label className="field">
                                    <span>Skills</span>
                                    <input type="text" value={formatList(item.skills)} onChange={(e) => updateItem('education', index, 'skills', normalizeTextList(e.target.value))} placeholder="Comma separated" />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea rows="3" value={item.notes ?? ''} onChange={(e) => updateItem('education', index, 'notes', e.target.value)} placeholder="Anything worth remembering..." />
                                </label>
                            </>
                        ),
                    (item) => `${item.school || 'School not set'} · ${item.isFinished ? 'Finished' : 'In progress'}`,
                )}

                {/* ── Work experience ── */}
                {renderEditableList(
                    'workExperiences',
                    'Work experience',
                    'Add work experience',
                    () => ({ startDate: '', endDate: '', company: '', position: '', jobDescription: [], skills: [], notes: '' }),
                    (item, index, readOnly) =>
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
                                    <input type="text" value={item.company ?? ''} onChange={(e) => updateItem('workExperiences', index, 'company', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Position</span>
                                    <input type="text" value={item.position ?? ''} onChange={(e) => updateItem('workExperiences', index, 'position', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Start date</span>
                                    <input type="date" value={item.startDate ?? ''} onChange={(e) => updateItem('workExperiences', index, 'startDate', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>End date</span>
                                    <input type="date" value={item.endDate ?? ''} onChange={(e) => updateItem('workExperiences', index, 'endDate', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Job description</span>
                                    <textarea rows="3" value={formatList(item.jobDescription)} onChange={(e) => updateItem('workExperiences', index, 'jobDescription', normalizeTextList(e.target.value))} placeholder="Comma separated" />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea rows="3" value={item.notes ?? ''} onChange={(e) => updateItem('workExperiences', index, 'notes', e.target.value)} placeholder="Anything worth remembering..." />
                                </label>
                            </>
                        ),
                    (item) => `${item.company || 'Company not set'} · ${formatDate(item.startDate)} — ${formatDate(item.endDate)}`,
                )}

                {/* ── Trainings ── */}
                {renderEditableList(
                    'trainings',
                    'Trainings',
                    'Add training',
                    () => ({ startDate: '', endDate: '', name: '', type: '', certification: [], skills: [], notes: '' }),
                    (item, index, readOnly) =>
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
                                    <input type="text" value={item.name ?? ''} onChange={(e) => updateItem('trainings', index, 'name', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Type</span>
                                    <input type="text" value={item.type ?? ''} onChange={(e) => updateItem('trainings', index, 'type', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Start date</span>
                                    <input type="date" value={item.startDate ?? ''} onChange={(e) => updateItem('trainings', index, 'startDate', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>End date</span>
                                    <input type="date" value={item.endDate ?? ''} onChange={(e) => updateItem('trainings', index, 'endDate', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Certification</span>
                                    <textarea rows="3" value={formatList(item.certification)} onChange={(e) => updateItem('trainings', index, 'certification', normalizeTextList(e.target.value))} placeholder="Comma separated" />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea rows="3" value={item.notes ?? ''} onChange={(e) => updateItem('trainings', index, 'notes', e.target.value)} placeholder="Anything worth remembering..." />
                                </label>
                            </>
                        ),
                    (item) => `${item.type || 'Type not set'} · ${formatDate(item.startDate)} — ${formatDate(item.endDate)}`,
                )}

                {/* ── Skills ── */}
                {renderEditableList(
                    'skills',
                    'Skills',
                    'Add skill',
                    () => ({ name: '', aliases: [], level: '', weight: '', notes: '' }),
                    (item, index, readOnly) =>
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
                                    <input type="text" value={item.name ?? ''} onChange={(e) => updateItem('skills', index, 'name', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Aliases</span>
                                    <input type="text" value={formatList(item.aliases)} onChange={(e) => updateItem('skills', index, 'aliases', normalizeTextList(e.target.value))} placeholder="Comma separated" />
                                </label>
                                <label className="field">
                                    <span>Level</span>
                                    <input type="text" value={item.level ?? ''} onChange={(e) => updateItem('skills', index, 'level', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Weight</span>
                                    <input type="text" value={item.weight ?? ''} onChange={(e) => updateItem('skills', index, 'weight', e.target.value)} />
                                </label>
                                <label className="field">
                                    <span>Notes</span>
                                    <textarea rows="3" value={item.notes ?? ''} onChange={(e) => updateItem('skills', index, 'notes', e.target.value)} placeholder="Anything worth remembering..." />
                                </label>
                            </>
                        ),
                    (item) => `${item.level || 'No level set'} · Weight: ${item.weight || '—'}`,
                )}

                {/* ── Notes ── */}
                <article className="card profile-card profile-notes-card">
                    <div className="section-header compact">
                        <div>
                            <h2>Notes</h2>
                            <p className="muted">General notes about the profile.</p>
                        </div>
                    </div>

                    {isEditing ? (
                        <textarea
                            rows="4"
                            value={draft.notes}
                            onChange={(e) => updateField('notes', e.target.value)}
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