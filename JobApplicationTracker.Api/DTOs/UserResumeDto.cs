using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class UserResumeDto
{
    public Guid? Id { get; set; }
    public int? UserId { get; set; }
    public ICollection<WorkExperienceDto> WorkExperiences { get; set; } = new List<WorkExperienceDto>();
    public ICollection<EducationDto> Education { get; set; } = new List<EducationDto>();
    public ICollection<TrainingDto> Trainings { get; set; } = new List<TrainingDto>();
    public ICollection<JobSkillDto> Skills { get; set; } = new List<JobSkillDto>();
    public ICollection<SkillUsageDto> UncategorizedSkillUsages { get; set; } = new List<SkillUsageDto>();
    
    public override string ToString()
    {
        static string Pluralize(int count, string singular, string plural) =>
            count == 1 ? singular : plural;

        static string JoinOrNone(IEnumerable<string>? items)
        {
            var list = items?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            return list is { Count: > 0 }
                ? string.Join(", ", list)
                : "none";
        }

        static string Shorten(string? text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim();
            return text.Length <= maxLength ? text : text[..maxLength].TrimEnd() + "...";
        }

        var workExperiences = WorkExperiences?.ToList() ?? [];
        var educationItems = Education?.ToList() ?? [];
        var trainings = Trainings?.ToList() ?? [];
        var skills = Skills?.ToList() ?? [];
        var uncategorizedUsages = UncategorizedSkillUsages?.ToList() ?? [];

        var workExperienceCount = workExperiences.Count;
        var educationCount = educationItems.Count;
        var trainingCount = trainings.Count;
        var skillCount = skills.Count;
        var uncategorizedSkillUsageCount = uncategorizedUsages.Count;

        var latestRole = workExperiences
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Position) || !string.IsNullOrWhiteSpace(x.Company));

        var latestRoleText = latestRole is null
            ? "no listed work experience"
            : string.Join(" at ",
                new[]
                {
                    Shorten(latestRole.Position, 60),
                    Shorten(latestRole.Company, 60)
                }.Where(x => !string.IsNullOrWhiteSpace(x)));

        var topSkills = JoinOrNone(
            skills
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Select(x => x.Name!)
                .Take(5));

        var educationSummary = educationCount == 0
            ? "no education listed"
            : $"{educationCount} {Pluralize(educationCount, "education entry", "education entries")}";

        var trainingSummary = trainingCount == 0
            ? "no training listed"
            : $"{trainingCount} {Pluralize(trainingCount, "training", "trainings")}";

        return
            $"Resume for user {UserId?.ToString() ?? "unknown"}: " +
            $"{workExperienceCount} {Pluralize(workExperienceCount, "work experience", "work experiences")}, " +
            $"{educationSummary}, " +
            $"{trainingSummary}, " +
            $"{skillCount} {Pluralize(skillCount, "skill", "skills")}. " +
            $"Most recent role: {latestRoleText}. " +
            $"Top skills: {topSkills}. " +
            $"Uncategorized skill notes: {uncategorizedSkillUsageCount} {Pluralize(uncategorizedSkillUsageCount, "item", "items")}.";
    }
}

//todo split into separate DTOs

public class WorkExperienceDto
{
    public Guid? Id { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public IEnumerable<string> JobDescription { get; set; } = new List<string>();
    public ICollection<SkillUsageDto> Skills { get; set; } = new List<SkillUsageDto>();
    public string? Notes { get; set; } = string.Empty;
}

public class EducationDto
{
    public Guid? Id { get; set; }
    public string? Degree { get; set; }
    public bool IsFinished { get; set; }
    public string? School { get; set; }
    public IEnumerable<string> Majors { get; set; }
    public ICollection<SkillUsageDto> Skills { get; set; } = new List<SkillUsageDto>();
    public string? Notes { get; set; } = string.Empty;  
}

public class TrainingDto
{
    public Guid? Id { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public IEnumerable<string>? Certification { get; set; }
    public ICollection<SkillUsageDto> Skills { get; set; } = new List<SkillUsageDto>();
    public string? Notes { get; set; } = string.Empty;   
}

public class JobSkillDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<string>? Aliases { get; set; }
    public SkillLevel? Level { get; set; }
    public SkillWeight? Weight { get; set; }
    public ICollection<SkillUsageDto> Skills { get; set; } = new List<SkillUsageDto>();
    public string? Notes { get; set; } = string.Empty; 
}

public class SkillUsageDto
{
    public Guid? Id { get; set; }
    public required JobSkillDto Skill { get; set; }
    public required string Description { get; set; }
}