using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.DTOs;

public class UpdateUserDto
{
    [Required]
    public int Id { get; set; }
    [MaxLength(100)]
    public string? Username { get; set; }

    public string? Password { get; set; }
}