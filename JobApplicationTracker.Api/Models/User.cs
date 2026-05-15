using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Models.UserProfile;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Models;

public class User : IdentityUser
{
    [Required]
    public DateTime CreatedAt { get; set; } 
    public DateTime? DeletedAt { get; set; } = null;
    public UserResume? UserResume { get; set; } = null!;
}