using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.DTOs;

public class UserDto
{
    [Required]
    [MaxLength(100)]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
}