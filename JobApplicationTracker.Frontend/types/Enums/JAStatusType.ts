export enum JAStatusType
{
    Wishlist  = 100,
    Applied    = 200,
    Task       = 300,
    Interview  = 400,
    Offer      = 500,
    Accepted   = 1000,
    Rejected   = 2000,
}

export const jaStatusLabels: Record<JAStatusType, string> = {
    [JAStatusType.Wishlist]: 'Wishlist',
    [JAStatusType.Applied]: 'Applied',
    [JAStatusType.Task]: 'Task',
    [JAStatusType.Interview]: 'Interview',
    [JAStatusType.Offer]: 'Offer',
    [JAStatusType.Accepted]: 'Accepted',
    [JAStatusType.Rejected]: 'Rejected',
};

export const applicationStatusColors: Record<JAStatusType, string> = {
    [JAStatusType.Wishlist]: 'bg-gray-500/20 text-gray-400 border-gray-500/30',
    [JAStatusType.Applied]: 'bg-blue-500/20 text-blue-400 border-blue-500/30',
    [JAStatusType.Task]: 'bg-cyan-500/20 text-cyan-400 border-cyan-500/30',
    [JAStatusType.Interview]: 'bg-purple-500/20 text-purple-400 border-purple-500/30',
    [JAStatusType.Offer]: 'bg-amber-500/20 text-amber-400 border-amber-500/30',
    [JAStatusType.Accepted]: 'bg-green-500/20 text-green-400 border-green-500/30',
    [JAStatusType.Rejected]: 'bg-red-500/20 text-red-400 border-red-500/30',
};