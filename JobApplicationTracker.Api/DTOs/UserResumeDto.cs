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
}

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