using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class SkillUsage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Guid ExperienceId { get; set; }
    public required Guid SkillId { get; set; }
    [MaxLength(2000)]
    public required string Description { get; set; }
}