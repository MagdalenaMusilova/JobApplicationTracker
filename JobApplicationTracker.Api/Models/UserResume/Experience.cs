using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class Experience
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserResumeId { get; set; }
    public List<SkillUsage> Skills { get; set; } = new();

}