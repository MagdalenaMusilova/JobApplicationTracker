function EventFormSection({ header = "Event", event, onChange }) {
    const update = (field, value) => {
        onChange({
            ...event,
            [field]: value,
        });
    };

    return (
        <div className="modal-section">
            <h4>{header}</h4>

            <div className="modal-grid">
                <label>
                    Event name
                    <input
                        type="text"
                        value={event.name}
                        onChange={(e) => update("name", e.target.value)}
                        required
                    />
                </label>

                <label>
                    Event type
                    <select
                        value={event.type}
                        onChange={(e) => update("type", e.target.value)}
                        required
                    >
                        <option value="">Select type</option>
                        <option value="Interview">Interview</option>
                        <option value="Meeting">Meeting</option>
                    </select>
                </label>

                <label>
                    Event duration
                    <select
                        value={event.isAllDay ? "allDay" : "timed"}
                        onChange={(e) => update("isAllDay", e.target.value === "allDay")}
                    >
                        <option value="allDay">Full day</option>
                        <option value="timed">Specific time</option>
                    </select>
                </label>

                <label>
                    {event.isAllDay ? "Event date" : "Event date & time"}
                    <input
                        type={event.isAllDay ? "date" : "datetime-local"}
                        value={event.date}
                        onChange={(e) => update("date", e.target.value)}
                        required
                    />
                </label>

                <label className="modal-span-2">
                    Note
                    <textarea
                        value={event.note || ""}
                        onChange={(e) => update("note", e.target.value)}
                    />
                </label>
            </div>
        </div>
    );
}

export default EventFormSection;