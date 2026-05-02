using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class OtherExperience : Experience
{
    [MaxLength(5000)]
    public string? Notes { get; set; }  
}