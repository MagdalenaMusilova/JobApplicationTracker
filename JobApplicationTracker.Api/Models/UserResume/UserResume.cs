using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class UserResume
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public List<WorkExperience> WorkExperiences { get; set; } = new();
    public List<Education> Education { get; set; } = new();
    public List<Training> Trainings { get; set; } = new();
    public List<OtherExperience> UncategorizedExperiences { get; set; } = new();
    public List<ResumeSkill> Skills { get; set; } = new();
    [MaxLength(2500)]
    public string? AboutMe { get; set; }
    [MaxLength(20000)]
    public string? UncategorizedInfo { get; set; }
}