import { useState } from 'react'
import { buildApplicationPayload, scoreJobMatch } from '../utils/jobMatcher'

function JobMatchPage({
                          profile,
                          onCreateApplicationFromMatch,
                      }) {
    const [jobDescription, setJobDescription] = useState('')
    const [generatedApplication, setGeneratedApplication] = useState(null)
    const [matchResult, setMatchResult] = useState(null)

    const canAnalyze = jobDescription.trim().length > 0

    const handleAnalyze = () => {
        const result = scoreJobMatch(jobDescription, profile)
        const payload = buildApplicationPayload(jobDescription, profile, result)

        setMatchResult(result)
        setGeneratedApplication(payload)
    }

    const handleCreateApplication = async () => {
        if (!generatedApplication) return
        await onCreateApplicationFromMatch(generatedApplication)
    }

    return (
        <section className="page">
            <div className="page-header">
                <div>
                    <span className="eyebrow">Match & Create</span>
                    <h1>Paste a job description and see how well it fits your profile</h1>
                    <p>Use this page to compare a role against your profile, then create a new application in one click.</p>
                </div>
            </div>

            <div className="match-layout">
                <article className="card">
                    <div className="card-top">
                        <div>
                            <h2>Job description</h2>
                            <p>Paste the full posting here.</p>
                        </div>
                    </div>

                    <textarea
                        className="job-input"
                        rows="16"
                        value={jobDescription}
                        onChange={(e) => setJobDescription(e.target.value)}
                        placeholder="Paste the job ad here..."
                    />

                    <div className="button-row">
                        <button type="button" className="secondary-btn" onClick={() => setJobDescription('')}>
                            Clear
                        </button>
                        <button type="button" className="primary-btn" onClick={handleAnalyze} disabled={!canAnalyze}>
                            Analyze match
                        </button>
                    </div>
                </article>

                <article className="card">
                    <div className="card-top">
                        <div>
                            <h2>Match result</h2>
                            <p>Automatic score based on your profile.</p>
                        </div>
                    </div>

                    {matchResult ? (
                        <>
                            <div className="match-score">
                                <span>Match score</span>
                                <strong>{matchResult.score}%</strong>
                            </div>

                            <div className="progress">
                                <div className="progress-bar match-bar" style={{ width: `${matchResult.score}%` }} />
                            </div>

                            <p className="muted" style={{ marginTop: '12px' }}>
                                {matchResult.summary}
                            </p>

                            <div className="match-columns">
                                <div>
                                    <h3>Matched skills</h3>
                                    <div className="tag-list">
                                        {matchResult.matchedSkills.length > 0 ? (
                                            matchResult.matchedSkills.map((skill) => (
                                                <span key={skill} className="tag">
                                                    {skill}
                                                </span>
                                            ))
                                        ) : (
                                            <span className="muted">No direct matches found.</span>
                                        )}
                                    </div>
                                </div>

                                <div>
                                    <h3>Missing skills</h3>
                                    <div className="tag-list">
                                        {matchResult.missingSkills.length > 0 ? (
                                            matchResult.missingSkills.map((skill) => (
                                                <span key={skill} className="tag">
                                                    {skill}
                                                </span>
                                            ))
                                        ) : (
                                            <span className="muted">Nice — nothing obvious is missing.</span>
                                        )}
                                    </div>
                                </div>
                            </div>

                            <div className="button-row" style={{ marginTop: '16px' }}>
                                <button
                                    type="button"
                                    className="primary-btn"
                                    onClick={handleCreateApplication}
                                    disabled={!generatedApplication}
                                >
                                    Create application
                                </button>
                            </div>
                        </>
                    ) : (
                        <p className="muted">Paste a job description and click Analyze match.</p>
                    )}
                </article>
            </div>
        </section>
    )
}

export default JobMatchPage