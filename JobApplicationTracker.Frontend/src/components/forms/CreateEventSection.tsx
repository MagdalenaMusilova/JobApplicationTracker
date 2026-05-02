import { ChangeEvent } from 'react'

interface Event {
    name: string
    type: string
    isAllDay: boolean
    date: string
    note?: string
}

interface EventFormSectionProps {
    header?: string
    event: Event
    onChange: (updated: Event) => void
}

function EventFormSection({
                              header = "Event",
                              event,
                              onChange
                          }: EventFormSectionProps) {

    const update = (field: keyof Event, value: string | boolean) => {
        onChange({
            ...event,
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
                    Event name
                    <input
                        type="text"
                        value={event.name}
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("name", e.target.value)
                        }
                        required
                    />
                </label>

                <label>
                    Event type
                    <select
                        value={event.type}
                        onChange={(e: ChangeEvent<HTMLSelectElement>) =>
                            update("type", e.target.value)
                        }
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
                        onChange={(e: ChangeEvent<HTMLSelectElement>) =>
                            update("isAllDay", e.target.value === "allDay")
                        }
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
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("date", e.target.value)
                        }
                        required
                    />
                </label>

                <label className="modal-span-2">
                    Note
                    <textarea
                        value={event.note ?? ""}
                        onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
                            update("note", e.target.value)
                        }
                    />
                </label>
            </div>
        </div>
    )
}

export default EventFormSection