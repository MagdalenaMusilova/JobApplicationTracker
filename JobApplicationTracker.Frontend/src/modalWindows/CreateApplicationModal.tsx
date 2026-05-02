import React, { useState } from "react";
import Modal from "../components/Modal.jsx";
import CreateApplicationSection from "../components/forms/CreateApplicationSection";
import CreateStatusEntrySection from "../components/forms/CreateStatusEntrySection";
import CreateEventSection from "../components/forms/CreateEventSection";
import { JAStatusType } from "../models/enums/JAStatusType";
import { JAEventType } from "../models/enums/JAEventType";
import { ApplicationsAPI } from '../api/applications'; // Import the ApplicationsAPI
import { useNavigate } from 'react-router-dom';
import {CreateJobApplication} from "../models/create/CreateJobApplication"; // Import useNavigate for redirection

type CreateApplicationModalProps = {
    onClose: (open: boolean) => void;
    availableStatuses: JAStatusType[];
    eventTypes: JAEventType[];
};

export default function CreateApplicationModal({
                                                   onClose,
                                                   availableStatuses,
                                                   eventTypes,
                                               } : CreateApplicationModalProps) {
    const [form, setForm] = useState<CreateJobApplication>({
        company: "",
        position: "",
        jobDescription: "",
        note: "",
        initialStatus: {
            statusType: 0,
            note: ""
        },
        jaEvent: null
    });

    // State to hold validation errors
    const [errors, setErrors] = useState<any>({});

    const navigate = useNavigate(); // Initialize the navigation hook

    const validateForm = () => {
        let valid = true;
        const newErrors: any = {};

        if (!form.company) {
            newErrors.company = "Company is required";
            valid = false;
        }

        if (!form.position) {
            newErrors.position = "Position is required";
            valid = false;
        }

        setErrors(newErrors);
        return valid;
    };

    const handleCreateApplication = async () => {
        if (validateForm()) {
            try {
                const response = await ApplicationsAPI.create(form);
                console.log("Application created successfully:", response);

                // Redirect to the new application's page
                navigate(`/applications/${response.id}`);
            } catch (error) {
                console.error("Failed to create application", error);
            }
        }
    };

    const toggleEvent = (checked: boolean) => {
        setForm((prev: any) => ({
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
        }));
    };

    return (
        <Modal
            title="New application"
            subtitle="Track a new job opportunity"
            onClose={() => onClose(false)}
        >
            {/* APPLICATION */}
            <CreateApplicationSection
                createdApplication={form}
                onChange={(updated) =>
                    setForm((prev: any) => ({ ...prev, ...updated }))
                }
            />
            {errors.company && <p className="error">{errors.company}</p>}
            {errors.position && <p className="error">{errors.position}</p>}

            {/* STATUS */}
            <CreateStatusEntrySection
                createdStatus={form.initialStatus}
                availableStatuses={availableStatuses}
                onChange={(updatedStatus) =>
                    setForm((prev: any) => ({
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
                            checked={!!form.jaEvent}
                            onChange={(e) => toggleEvent(e.target.checked)}
                        />
                        <span className="toggle-slider"></span>
                        <span className="toggle-label">Add event</span>
                    </label>
                </div>

                {form.jaEvent && (
                    <CreateEventSection
                        createdEvent={form.jaEvent}
                        eventTypes={eventTypes}
                        onChange={(updatedEvent) =>
                            setForm((prev: any) => ({
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