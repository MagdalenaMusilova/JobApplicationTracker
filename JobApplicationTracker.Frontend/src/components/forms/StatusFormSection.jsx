function StatusFormSection({ header = "Status", status, onChange, availableStatuses }) {
    const update = (field, value) => {
        onChange({
            ...status,
            [field]: value,
        });
    };

    return (
        <div className="modal-section">
            <h4>{header}</h4>

            <div className="modal-grid">
                <label>
                    Status type
                    <select
                        value={status.type}
                        onChange={(e) => update("type", e.target.value)}
                    >
                        {availableStatuses.map((s) => (
                            <option key={s.name} value={s.name}>
                                {s.name}
                            </option>
                        ))}
                    </select>
                </label>

                <label>
                    Status note
                    <input
                        type="text"
                        value={status.note}
                        onChange={(e) => update("note", e.target.value)}
                        placeholder="Optional note for the status"
                    />
                </label>
            </div>
        </div>
    );
}

export default StatusFormSection;