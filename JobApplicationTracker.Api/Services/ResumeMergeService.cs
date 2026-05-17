using AutoMapper;
using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public class ResumeMergeService : IResumeMergeService
{
    private readonly IAiAgentService _aiAgentService;
    
    public ResumeMergeService(IAiAgentService aiAgentService)
    {
        _aiAgentService = aiAgentService;
    }
    
    public async Task<UserResumeDto> MergeAsync(UserResumeDto existing, UserResumeDto incoming)
    {
        return new UserResumeDto
        {
            Id = existing.Id,
            UserId = existing.UserId,
            WorkExperiences = await MergeWorkExperiences(existing.WorkExperiences, incoming.WorkExperiences),
            Education = await MergeEducation(existing.Education, incoming.Education),
            Trainings = await MergeTrainings(existing.Trainings, incoming.Trainings),
            Skills = MergeSkills(existing.Skills, incoming.Skills),
            UncategorizedSkillUsages = MergeSkillUsages(existing.UncategorizedSkillUsages, incoming.UncategorizedSkillUsages),
        };
    }

    private async Task<ICollection<WorkExperienceDto>> MergeWorkExperiences(
        ICollection<WorkExperienceDto> existing,
        ICollection<WorkExperienceDto> incoming)
    {
        var result = existing.ToList();

        foreach (var newEntry in incoming)
        {
            var match = result.FirstOrDefault(e =>
                string.Equals(e.Company, newEntry.Company, StringComparison.OrdinalIgnoreCase) &&
                e.StartDate == newEntry.StartDate);

            if (match is null)
            {
                result.Add(newEntry);
            }
            else
            {
                // Enrich: fill gaps in existing entry
                match.EndDate ??= newEntry.EndDate;
                match.Position ??= newEntry.Position;
                match.Notes = await MergeNotes(match.Notes, newEntry.Notes);
                match.Skills = MergeSkillUsages(match.Skills, newEntry.Skills);
            }
        }

        return result;
    }

    private async Task<ICollection<EducationDto>> MergeEducation(
        ICollection<EducationDto> existing,
        ICollection<EducationDto> incoming)
    {
        var result = existing.ToList();

        foreach (var newEntry in incoming)
        {
            var match = result.FirstOrDefault(e =>
                string.Equals(e.School, newEntry.School, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(e.Degree, newEntry.Degree, StringComparison.OrdinalIgnoreCase));

            if (match is null)
                result.Add(newEntry);
            else
            {
                match.Notes = await MergeNotes(match.Notes, newEntry.Notes);
                match.Skills = MergeSkillUsages(match.Skills, newEntry.Skills);
            }
        }

        return result;
    }

    private async Task<ICollection<TrainingDto>> MergeTrainings(
        ICollection<TrainingDto> existing,
        ICollection<TrainingDto> incoming)
    {
        var result = existing.ToList();

        foreach (var newEntry in incoming)
        {
            var match = result.FirstOrDefault(e =>
                string.Equals(e.Name, newEntry.Name, StringComparison.OrdinalIgnoreCase) &&
                e.StartDate == newEntry.StartDate);

            if (match is null)
                result.Add(newEntry);
            else
                match.Notes = await MergeNotes(match.Notes, newEntry.Notes);
        }

        return result;
    }

    private ICollection<JobSkillDto> MergeSkills(
        ICollection<JobSkillDto> existing,
        ICollection<JobSkillDto> incoming)
    {
        var result = existing.ToList();

        foreach (var newSkill in incoming)
        {
            var match = result.FirstOrDefault(e =>
                string.Equals(e.Name, newSkill.Name, StringComparison.OrdinalIgnoreCase));

            if (match is null)
            {
                result.Add(newSkill);
            }
            else
            {
                match.Aliases = (match.Aliases ?? [])
                    .Union(newSkill.Aliases ?? [], StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
        }

        return result;
    }

    private ICollection<SkillUsageDto> MergeSkillUsages(
        ICollection<SkillUsageDto> existing,
        ICollection<SkillUsageDto> incoming)
    {
        var result = existing.ToList();

        foreach (var newUsage in incoming)
        {
            var alreadyExists = result.Any(e =>
                string.Equals(e.Description, newUsage.Description, StringComparison.OrdinalIgnoreCase));

            if (!alreadyExists)
                result.Add(newUsage);
        }

        return result;
    }

    private async Task<string?> MergeNotes(string? existing, string? incoming)
    {
        if (string.IsNullOrWhiteSpace(existing)) return incoming;
        if (string.IsNullOrWhiteSpace(incoming)) return existing;
        // Exact/substring match — skip AI call entirely
        if (existing.Contains(incoming, StringComparison.OrdinalIgnoreCase)) return existing;
        if (incoming.Contains(existing, StringComparison.OrdinalIgnoreCase)) return incoming;

        var prompt = $"""
                      You are merging notes from two versions of the same resume entry.
                      Combine them into a single, concise note. Remove any duplicate information.
                      Do not add commentary, explanations, or formatting — output only the merged note text.

                      Note A: {existing}
                      Note B: {incoming}
                      """;

        var merged = await _aiAgentService.MakeRequestAsync(prompt);
        return string.IsNullOrWhiteSpace(merged) ? existing : merged.Trim();
    }
}