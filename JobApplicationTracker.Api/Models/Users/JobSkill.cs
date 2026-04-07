using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Models.Users;

public class JobSkill
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public IEnumerable<string>? Aliases { get; set; }
    public SkillLevel? Level { get; set; }
    public SkillWeight Weight { get; set; }
    public ICollection<SkillUsage> Skills { get; set; } = new List<SkillUsage>();
    public string? Notes { get; set; } = string.Empty; 
}