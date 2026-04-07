namespace JobApplicationTracker.Models.Users;

public class WorkExperience
{
    public Guid Id { get; set; }
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public required string Company { get; set; }
    public required string Position { get; set; }
    public IEnumerable<string> JobDescription { get; set; } = new List<string>();
    public ICollection<SkillUsage> Skills { get; set; } = new List<SkillUsage>();
    public string? Notes { get; set; } = string.Empty;
}