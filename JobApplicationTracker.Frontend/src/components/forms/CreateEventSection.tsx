import { ChangeEvent } from 'react'
import {CreateJAEvent} from "../../models/create/CreateJAEvent";
import {JAEventType} from "../../models/enums/JAEventType";

interface CreateEventSectionProps {
    header?: string
    createdEvent: CreateJAEvent,
    eventTypes: JAEventType[],
    onChange: (updated: CreateJAEvent) => void
}

function CreateEventSection({
                              header = "New event",
                              createdEvent, 
                              eventTypes,
                              onChange
                          }: CreateEventSectionProps) {

    const update = <K extends keyof CreateJAEvent>(
        field: K,
        value: CreateJAEvent[K]
    ) => {
        onChange({
            ...createdEvent,
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
                        value={createdEvent.eventName}
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("eventName", e.target.value)
                        }
                        required
                    />
                </label>

                <label>
                    Event type
                    <select
                        value={createdEvent.eventType}
                        onChange={(e: ChangeEvent<HTMLSelectElement>) =>
                            update("eventType", Number(e.target.value))
                        }
                        required
                    >
                        <option value="">Select type</option>
                        {eventTypes.map((type) => (
                            <option key={type.value} value={type.value}>
                                {type.label}
                            </option>
                        ))}
                    </select>
                </label>

                <label>
                    Event duration
                    <select
                        value={createdEvent.isWholeDay ? "allDay" : "timed"}
                        onChange={(e: ChangeEvent<HTMLSelectElement>) =>
                            update("isWholeDay", e.target.value === "allDay")
                        }
                    >
                        <option value="allDay">Full day</option>
                        <option value="timed">Specific time</option>
                    </select>
                </label>

                <label>
                    {createdEvent.isWholeDay ? "Event date" : "Event date & time"}
                    <input
                        type={createdEvent.isWholeDay ? "date" : "datetime-local"}
                        value={createdEvent.eventDate}
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("eventDate", e.target.value)
                        }
                        required
                    />
                </label>

                <label className="modal-span-2">
                    Note
                    <textarea
                        value={createdEvent.note ?? ""}
                        onChange={(e: ChangeEvent<HTMLTextAreaElement>) =>
                            update("note", e.target.value)
                        }
                    />
                </label>
            </div>
        </div>
    )
}

export default CreateEventSection