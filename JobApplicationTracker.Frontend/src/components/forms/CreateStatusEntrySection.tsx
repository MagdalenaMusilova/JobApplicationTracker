import { ChangeEvent } from "react";
import { JAStatusType } from "../../models/enums/JAStatusType";
import {CreateJAStatusEntry} from "../../models/create/CreateJAStatusEntry";

interface CreateStatusEntrySectionProps {
    header?: string
    createdStatus: CreateJAStatusEntry
    availableStatuses: JAStatusType[]
    onChange: (updated: CreateJAStatusEntry) => void
}

function CreateStatusEntrySection({
                                 header = "New status",
                                 createdStatus,
                                 availableStatuses, 
                                 onChange,
                             }: CreateStatusEntrySectionProps) {
    const update = <K extends keyof CreateJAStatusEntry>(
        field: K,
        value: CreateJAStatusEntry[K]
    ) => {
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
                        value={createdStatus.statusType}
                        onChange={(e: ChangeEvent<HTMLSelectElement>) =>
                            update("statusType", Number(e.target.value))
                        }
                        required
                    >
                        <option value="">Select type</option>
                        {availableStatuses.map((type) => (
                            <option key={type.value} value={type.value}>
                                {type.label}
                            </option>
                        ))}
                    </select>
                </label>

                <label>
                    Status note
                    <input
                        type="text"
                        value={createdStatus.note ?? ""}
                        onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            update("note", e.target.value)
                        }
                        placeholder="Optional note for the status"
                    />
                </label>

            </div>
        </div>
    );
}

export default CreateStatusEntrySection;