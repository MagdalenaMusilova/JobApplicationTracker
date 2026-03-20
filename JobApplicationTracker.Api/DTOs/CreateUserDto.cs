using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.DTOs;

public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}