export const statusSteps = ['Applied', 'In progress', 'Interview', 'Offer', 'Closed']

export const statusToProgress = {
    Applied: 20,
    'In progress': 45,
    Interview: 70,
    Offer: 90,
    Closed: 100,
}

export const keywordWeights = {
    csharp: 10,
    '.net': 12,
    'asp.net': 14,
    sql: 10,
    react: 10,
    javascript: 8,
    api: 10,
    azure: 10,
    docker: 7,
    testing: 5,
    architecture: 8,
    communication: 4,
    cloud: 7,
}

export function computeAutomaticMatchScore(application, candidateSkills) {
    const haystack = `${application.title} ${application.company} ${application.description} ${application.notes} ${(application.requirements || []).join(' ')}`.toLowerCase()

    let score = 35

    for (const skill of candidateSkills) {
        const key = skill.name.toLowerCase()
        if (haystack.includes(key)) {
            score += keywordWeights[key] ?? 6
        }
    }

    return Math.max(0, Math.min(100, score))
}

export function getStatusIndex(status) {
    const index = statusSteps.indexOf(status)
    return index === -1 ? 0 : index
}