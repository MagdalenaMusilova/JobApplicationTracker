using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Models.UserProfile;

public class ResumeSkill
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserResumeId { get; set; }
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; } 
    public List<SkillUsage> Usages { get; set; } = new();
    [MaxLength(2000)]
    public string? Notes { get; set; } = string.Empty; 
}