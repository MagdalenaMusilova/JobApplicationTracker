function normalizeStatus(status) {
    const value = String(status ?? '').trim()
    return value.charAt(0).toUpperCase() + value.slice(1).toLowerCase()
}

function formatUpdatedAt(history) {
    if (!history?.length) return 'Just now'

    const last = history[history.length - 1]
    const raw = last?.updatedAt ?? last?.createdAt
    const date = new Date(raw)

    if (Number.isNaN(date.getTime())) return 'Updated recently'

    return date.toLocaleDateString()
}

export function mapApplication(dto) {
    const history = dto.statusHistory ?? []
    const last = history.at(-1)

    return {
        id: dto.id,
        company: dto.company ?? '',
        title: dto.position ?? '',
        location: dto.location ?? 'Remote',
        status: normalizeStatus(last?.jaStatus),
        updatedAt: formatUpdatedAt(history),
        statusHistory: history,
        scheduledEvents: dto.scheduledEvents ?? [],
        backend: dto,
    }
}