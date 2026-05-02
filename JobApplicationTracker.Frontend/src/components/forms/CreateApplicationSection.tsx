import { ChangeEvent } from 'react'

interface Application {
    company: string
    position: string
    jobDescription: string
    notes: string
}

interface ApplicationFormSectionProps {
    header?: string
    application: Application
    onChange: (updated: Application) => void
}

function ApplicationFormSection({
                                    header = "New application",
                                    application,
                                    onChange
                                }: ApplicationFormSectionProps) {

    const update = (field: keyof Application, value: string) => {
        onChange({
            ...application,
            [field]: value,
        })
    }

    return (
        <div className="modal-section">
            <div className="modal-section-header">
                <h4>{header}</h4>
            </div>

            <div className="modal-grid">
                <label>
                    Company
                    <input
                        type="text"
                        value={application.company}
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("company", e.target.value)
                        }
                        required
                    />
                </label>

                <label>
                    Position
                    <input
                        type="text"
                        value={application.position}
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("position", e.target.value)
                        }
                        required
                    />
                </label>

                <label className="modal-span-2">
                    Job description
                    <textarea
                        rows={5}
                        value={application.jobDescription}
                        onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
                            update("jobDescription", e.target.value)
                        }
                    />
                </label>

                <label className="modal-span-2">
                    Notes
                    <textarea
                        rows={4}
                        value={application.notes}
                        onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
                            update("notes", e.target.value)
                        }
                    />
                </label>
            </div>
        </div>
    )
}

export default ApplicationFormSection