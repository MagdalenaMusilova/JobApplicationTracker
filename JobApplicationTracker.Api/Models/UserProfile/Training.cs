using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class Training : Experience
{
    [Required]
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [Required]
    [MaxLength(500)]
    public required string Name { get; set; }   
    [MaxLength(500)]
    public string? Type { get; set; }   // what's the training about 
    [MaxLength(2500)]
    public string? Certification { get; set; }
    [MaxLength(2500)]
    public string? Notes { get; set; } = string.Empty;   
}