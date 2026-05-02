import {CreateJAStatusEntry} from "./CreateJAStatusEntry";
import {CreateJobListing} from "./CreateJobListing";
import {CreateJAEvent} from "./CreateJAEvent";

export interface CreateJobApplication {
    company: string,
    position: string,
    note: string | null,
    initialStatus: CreateJAStatusEntry,
    jobDescription: string,
    jaEvent: CreateJAEvent | null,
}