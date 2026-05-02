using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Models.UserProfile;

namespace JobApplicationTracker.Models;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string PasswordHash { get; set; } 
    [Required]
    public DateTime CreatedAt { get; set; } 
    public DateTime? DeletedAt { get; set; } = null;
    public UserResume? UserResume { get; set; } = null!;
}