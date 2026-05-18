using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class Education : Experience
{
    [MaxLength(200)]
    public string? Degree { get; set; }
    [Required]
    public required bool IsFinished { get; set; }
    [Required]
    [MaxLength(200)]
    public required string School { get; set; }
    [Required]
    [MaxLength(500)]
    public required string Major { get; set; }
    [MaxLength(5000)]
    public string? Notes { get; set; }  
}