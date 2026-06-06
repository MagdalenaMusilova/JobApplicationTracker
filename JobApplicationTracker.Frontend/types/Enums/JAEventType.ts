export enum JAEventType
{
    Other = 0,
    Call = 1,
    Interview = 2,
    Task = 3,
}

export const eventTypeLabels: Record<JAEventType, string> = {
    [JAEventType.Other]: 'Other',
    [JAEventType.Call]: 'Call',
    [JAEventType.Interview]: 'Interview',
    [JAEventType.Task]: 'Task',
};

export const eventTypeColors: Record<JAEventType, string> = {
    [JAEventType.Other]: 'bg-gray-500/20 text-gray-400 border-gray-500/30',
    [JAEventType.Call]: 'bg-cyan-500/20 text-cyan-400 border-cyan-500/30',
    [JAEventType.Interview]: 'bg-indigo-500/20 text-indigo-400 border-indigo-500/30',
    [JAEventType.Task]: 'bg-purple-500/20 text-purple-400 border-purple-500/30',
};