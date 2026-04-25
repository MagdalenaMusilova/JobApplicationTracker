using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.DTOs;

public class UserDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; } = null;
    
}