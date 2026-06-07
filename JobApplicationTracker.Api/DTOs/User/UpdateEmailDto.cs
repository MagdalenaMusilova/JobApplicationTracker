using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.DTOs;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
