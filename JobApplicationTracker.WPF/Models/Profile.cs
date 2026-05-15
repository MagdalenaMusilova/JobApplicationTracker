namespace JobApplicationTracker.Wpf.Models.Profile;

public class UserProfileDto
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string? Email { get; set; }
}

public class UserResumeDto
{
    public Guid? Id { get; set; }
    public string? AboutMe { get; set; }
    public string? Notes { get; set; }
    public List<WorkExperienceDto> WorkExperiences { get; set; } = [];
    public List<EducationDto> Education { get; set; } = [];
    public List<TrainingDto> Trainings { get; set; } = [];
    public List<JobSkillDto> Skills { get; set; } = [];
}

public class WorkExperienceDto
{
    public string? Company { get; set; }
    public string? Position { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IEnumerable<string> JobDescription { get; set; } = [];
    public string? Notes { get; set; }

    public string DateRange =>
        $"{StartDate?.ToString("M/d/yyyy") ?? "—"} — {EndDate?.ToString("M/d/yyyy") ?? "—"}";

    public string Description =>
        JobDescription.Any() ? string.Join(", ", JobDescription) : "—";
}

public class EducationDto
{
    public string? Degree { get; set; }
    public string? School { get; set; }
    public bool IsFinished { get; set; }
    public IEnumerable<string> Majors { get; set; } = [];
    public string? Notes { get; set; }

    public string Status => IsFinished ? "Finished" : "In progress";
    public string MajorsText => Majors.Any() ? string.Join(", ", Majors) : "—";
    public string Subtitle => $"{School} · {Status}";
}

public class TrainingDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IEnumerable<string>? Certification { get; set; }
    public string? Notes { get; set; }

    public string DateRange =>
        $"{StartDate?.ToString("M/d/yyyy") ?? "—"} — {EndDate?.ToString("M/d/yyyy") ?? "—"}";

    public string CertificationText =>
        Certification?.Any() == true ? string.Join(", ", Certification) : "—";

    public string Subtitle => $"{Type} · {DateRange}";
}

public class JobSkillDto
{
    public string? Name { get; set; }
    public IEnumerable<string>? Aliases { get; set; }
    public string? Level { get; set; }
    public string? Notes { get; set; }

    public string AliasesText => Aliases?.Any() == true ? string.Join(", ", Aliases) : "—";
    public string LevelText => Level ?? "—";
    public string Subtitle => Level ?? "";
}