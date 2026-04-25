export function normalizeText(value) {
    return String(value ?? '')
        .toLowerCase()
        .replace(/\s+/g, ' ')
        .trim()
}

export function extractKeywords(profile) {
    const skillNames = (profile.skills ?? []).map((skill) => skill.name).filter(Boolean)

    const profileText = [
        profile.aboutMe,
        profile.notes,
        ...(profile.education ?? []).flatMap((item) => [item.degree, item.school, ...(item.majors ?? []), ...(item.skills ?? [])]),
        ...(profile.workExperiences ?? []).flatMap((item) => [
            item.company,
            item.position,
            ...(item.jobDescription ?? []),
            ...(item.skills ?? []).map((skill) => skill?.skill?.name ?? skill?.name ?? skill),
        ]),
        ...(profile.trainings ?? []).flatMap((item) => [
            item.name,
            item.type,
            ...(item.certification ?? []),
            ...(item.skills ?? []).map((skill) => skill?.skill?.name ?? skill?.name ?? skill),
        ]),
    ]
        .filter(Boolean)
        .join(' ')

    return {
        skillNames,
        profileText: normalizeText(profileText),
    }
}

export function scoreJobMatch(jobDescription, profile) {
    const jobText = normalizeText(jobDescription)
    const { skillNames, profileText } = extractKeywords(profile)

    const matchedSkills = []
    const missingSkills = []

    let score = 25

    for (const skill of skillNames) {
        const normalizedSkill = normalizeText(skill)
        if (!normalizedSkill) continue

        const foundInJob = jobText.includes(normalizedSkill)
        const foundInProfile = profileText.includes(normalizedSkill)

        if (foundInJob) {
            matchedSkills.push(skill)
            score += 8
        } else if (foundInProfile) {
            score += 2
        } else {
            missingSkills.push(skill)
        }
    }

    const commonKeywords = [
        'c#',
        '.net',
        'asp.net',
        'asp.net core',
        'sql',
        'postgres',
        'react',
        'javascript',
        'typescript',
        'api',
        'rest',
        'azure',
        'docker',
        'testing',
        'architecture',
        'communication',
        'cloud',
    ]

    for (const keyword of commonKeywords) {
        if (jobText.includes(keyword)) {
            score += 3
        }
    }

    const cappedScore = Math.max(0, Math.min(100, score))

    return {
        score: cappedScore,
        matchedSkills,
        missingSkills,
        summary:
            cappedScore >= 80
                ? 'Strong match — worth applying quickly.'
                : cappedScore >= 55
                    ? 'Good fit — some tailoring recommended.'
                    : 'Mixed fit — consider adjusting the application.',
    }
}

export function inferCompanyAndRole(jobDescription) {
    const text = String(jobDescription ?? '').trim()
    const lines = text.split(/\r?\n/).map((line) => line.trim()).filter(Boolean)

    const firstLine = lines[0] ?? ''
    const secondLine = lines[1] ?? ''

    const roleCandidates = [
        firstLine,
        secondLine,
        ...lines.filter((line) => /engineer|developer|manager|designer|analyst|specialist|architect/i.test(line)),
    ].filter(Boolean)

    const title =
        roleCandidates.find((line) => /engineer|developer|manager|designer|analyst|specialist|architect/i.test(line)) ??
        firstLine ??
        'New role'

    const companyMatch =
        text.match(/(?:at|@)\s+([A-Z][\w&.-]+(?:\s+[A-Z][\w&.-]+){0,3})/) ??
        text.match(/^([A-Z][\w&.-]+(?:\s+[A-Z][\w&.-]+){0,3})$/m)

    const company = companyMatch?.[1] ?? 'Unknown company'

    return {
        company,
        title,
    }
}

export function buildApplicationPayload(jobDescription, profile, matchResult) {
    const inferred = inferCompanyAndRole(jobDescription)

    return {
        company: inferred.company,
        title: inferred.title,
        location: 'Remote',
        description: String(jobDescription ?? '').trim(),
        notes: [
            `Match score: ${matchResult.score}%`,
            `Matched skills: ${matchResult.matchedSkills.length ? matchResult.matchedSkills.join(', ') : 'None'}`,
            `Missing skills: ${matchResult.missingSkills.length ? matchResult.missingSkills.join(', ') : 'None'}`,
        ].join('\n'),
        requirements: matchResult.matchedSkills,
        profileSummary: profile.aboutMe ?? '',
    }
}