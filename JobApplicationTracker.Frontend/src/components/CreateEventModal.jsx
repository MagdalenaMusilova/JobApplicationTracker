import React, { useState } from 'react';
import Modal from "./Modal.jsx";

const CreateEventModal = ({ isOpen, onClose }) => {
    const [eventName, setEventName] = useState('');
    const [eventType, setEventType] = useState('');
    const [eventDate, setEventDate] = useState('');
    const [note, setNote] = useState('');
    const [isAllDay, setIsAllDay] = useState(true);
    
    const handleSubmit = (e) => {
        e.preventDefault();
        // Implement logic to create the event
        console.log('Create Event:', { eventName, eventType, eventDate, note });
        onClose();
    };

    return (
        <Modal
            title="Create new Event"
            subtitle="Add event name, type, date, and notes."
            isOpen={isOpen}
            onClose={onClose}
        >
            <form className="modal-form" onSubmit={handleSubmit}>
                <div className="modal-section">
                    <h4>Event Details</h4>

                    <div className="modal-grid">
                        {/* Event name */}
                        <label>
                            Event name
                            <input
                                type="text"
                                value={eventName}
                                onChange={(e) => setEventName(e.target.value)}
                                required
                            />
                        </label>

                        {/* Event type */}
                        <label>
                            Event type
                            <select
                                value={eventType}
                                onChange={(e) => setEventType(e.target.value)}
                                required
                            >
                                <option value="">Select an event type</option>
                                <option value="Conference">Conference</option>
                                <option value="Interview">Interview</option>
                                <option value="Meeting">Meeting</option>
                            </select>
                        </label>

                        {/* Full day vs specific time */}
                        <label>
                            Event duration
                            <select
                                value={isAllDay ? "allDay" : "timed"}
                                onChange={(e) => setIsAllDay(e.target.value === "allDay")}
                            >
                                <option value="allDay">Full day</option>
                                <option value="timed">Specific time</option>
                            </select>
                        </label>

                        {/* Event date */}
                        <label>
                            {isAllDay ? "Event date" : "Event date & time"}
                            <input
                                type={isAllDay ? "date" : "datetime-local"}
                                value={eventDate}
                                onChange={(e) => setEventDate(e.target.value)}
                                required
                            />
                        </label>

                        {/* Note */}
                        <label className="modal-span-2">
                            Note (optional)
                            <textarea
                                rows="3"
                                value={note}
                                onChange={(e) => setNote(e.target.value)}
                            />
                        </label>
                    </div>
                </div>

                <div className="button-row modal-actions">
                    <button
                        type="button"
                        className="secondary-btn"
                        onClick={onClose}
                    >
                        Cancel
                    </button>
                    <button type="submit" className="primary-btn">
                        Create event
                    </button>
                </div>
            </form>
        </Modal>
    );
};

export default CreateEventModal;