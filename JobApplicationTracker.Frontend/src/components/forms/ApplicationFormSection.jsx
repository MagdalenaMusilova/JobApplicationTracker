function ApplicationFormSection({ header = "New application", application, onChange }) {
    const update = (field, value) => {
        onChange({
            ...application,
            [field]: value,
        });
    };

    return (
        <div className="modal-section">
            <h4>{header}</h4>

            <div className="modal-grid">
                <label>
                    Company
                    <input
                        type="text"
                        value={application.company}
                        onChange={(e) => update("company", e.target.value)}
                        required
                    />
                </label>

                <label>
                    Position
                    <input
                        type="text"
                        value={application.position}
                        onChange={(e) => update("position", e.target.value)}
                        required
                    />
                </label>

                <label className="modal-span-2">
                    Job description
                    <textarea
                        rows="5"
                        value={application.jobDescription}
                        onChange={(e) => update("jobDescription", e.target.value)}
                    />
                </label>

                <label className="modal-span-2">
                    Notes
                    <textarea
                        rows="4"
                        value={application.notes}
                        onChange={(e) => update("notes", e.target.value)}
                    />
                </label>
            </div>
        </div>
    );
}

export default ApplicationFormSection;