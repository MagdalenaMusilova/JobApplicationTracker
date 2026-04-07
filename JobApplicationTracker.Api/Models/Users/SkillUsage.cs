namespace JobApplicationTracker.Models.Users;

public class SkillUsage
{
    public Guid Id { get; set; }
    public required JobSkill Skill { get; set; }
    public required string Description { get; set; }
}