import { ChangeEvent } from 'react'
import {CreateJobApplication} from "../../models/create/CreateJobApplication";

interface CreateApplicationSectionProps {
    header?: string
    createdApplication: CreateJobApplication
    onChange: (updated: CreateJobApplication) => void
}

function CreateApplicationSection({
                                    header = "New application",
                                    createdApplication,
                                    onChange
                                }: CreateApplicationSectionProps) {

    const update = (field: keyof CreateJobApplication, value: string) => {
        onChange({
            ...createdApplication,
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
                        value={createdApplication.company}
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
                        value={createdApplication.position}
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
                        value={createdApplication.jobDescription}
                        onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
                            update("jobDescription", e.target.value)
                        }
                    />
                </label>

                <label className="modal-span-2">
                    Notes
                    <textarea
                        rows={4}
                        value={createdApplication.note}
                        onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
                            update("note", e.target.value)
                        }
                    />
                </label>
            </div>
        </div>
    )
}

export default CreateApplicationSection