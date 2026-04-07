namespace JobApplicationTracker.Models.Users;

public class Education
{
    public Guid Id { get; set; } 
    public string? Degree { get; set; }
    public required bool IsFinished { get; set; }
    public string School { get; set; }
    public IEnumerable<string> Majors { get; set; }
    public ICollection<SkillUsage> Skills { get; set; } = new List<SkillUsage>();
    public string? Notes { get; set; } = string.Empty;  
}