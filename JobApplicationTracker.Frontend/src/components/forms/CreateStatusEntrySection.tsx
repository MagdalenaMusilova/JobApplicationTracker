import { ChangeEvent } from "react";
import { JAStatusType } from "../../models/enums/JAStatusType";
import { CreateJAEvent } from "../../models/create/CreateJAEvent";
import {CreateJobApplication} from "../../models/create/CreateJobApplication";

interface CreateStatusSectionProps {
    header?: string
    createdStatus: CreateJAEvent
    availableStatuses: JAStatusType[]
    onChange: (updated: CreateJAEvent) => void
}

function CreateStatusSection({
                                 header = "New status",
                                 createdStatus,
                                 availableStatuses, 
                                 onChange,
                             }: CreateStatusSectionProps) {
    const update = (field: keyof CreateJAEvent, value: string) => {
        onChange({
            ...createdStatus,
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
                    Status type
                    <select
                        value={createdStatus.eventType}
                        onChange={handleChange}
                    >
                        {availableStatuses.map((s) => (
                            <option key={s.statusValue} value={s.statusName}>
                                {s.statusName}
                            </option>
                        ))}
                    </select>
                </label>

                <label>
                    Status note
                    <input
                        type="text"
                        value={createdStatus.note ?? ""}
                        onChange={handleNoteChange}
                        placeholder="Optional note for the status"
                    />
                </label>

            </div>
        </div>
    );
}

export default CreateStatusSection;