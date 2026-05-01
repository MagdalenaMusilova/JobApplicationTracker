import React, {useState} from "react";
import Modal from "../components/Modal.jsx";
import ApplicationFormSection from "../components/forms/ApplicationFormSection.jsx";
import StatusFormSection from "../components/forms/StatusFormSection.jsx";
import EventFormSection from "../components/forms/EventFormSection.jsx";

export default function CreateApplicationModal({
                                                onClose,
                                                avaibleStatuses
                                            }) {
    const [form, setForm] = useState<any>({
        company: "",
        position: "",
        jobDescription: "",
        notes: "",
        status: {
            type: avaibleStatuses[0] || "",
            note: ""
        },
        event: null
    })

    const handleCreateApplication = () => {
        console.log("Create Application", form);
    }
    
    const toggleEvent = (checked) => {
        setForm((prev) => ({
            ...prev,
            event: checked
                ? {
                    name: "",
                    type: "",
                    isAllDay: true,
                    date: "",
                    note: ""
                }
                : null
        }))
    }
    
    return (
        <Modal
            title="New application"
            subtitle="Track a new job opportunity"
            onClose={() => onClose(false)}
        >
            {/* APPLICATION */}
            <ApplicationFormSection
                application={form}
                onChange={(updated) =>
                    setForm((prev) => ({ ...prev, ...updated }))
                }
            />

            {/* STATUS */}
            <StatusFormSection
                status={form.status}
                availableStatuses=availableStatuses
                onChange={(updatedStatus) =>
                    setForm((prev) => ({
                        ...prev,
                        status: updatedStatus,
                    }))
                }
            />

            {/* EVENT TOGGLE */}
            <div className="modal-section">
                <div className="modal-section-header">
                    <h4>Event</h4>

                    <label className="toggle">
                        <input
                            type="checkbox"
                            checked={!!form.event}
                            onChange={(e) => toggleEvent(e.target.checked)}
                        />
                        <span className="toggle-slider"></span>
                        <span className="toggle-label">Add event</span>
                    </label>
                </div>

                {form.event && (
                    <EventFormSection
                        event={form.event}
                        onChange={(updatedEvent) =>
                            setForm((prev) => ({
                                ...prev,
                                event: updatedEvent,
                            }))
                        }
                    />
                )}
            </div>

            {/* ACTIONS */}
            <div className="modal-actions">
                <button
                    type="button"
                    className="secondary-btn"
                    onClick={() => onClose(false)}
                >
                    Cancel
                </button>

                <button
                    type="button"
                    className="primary-btn"
                    onClick={handleCreateApplication}
                >
                    Create
                </button>
            </div>
        </Modal>
    );
}