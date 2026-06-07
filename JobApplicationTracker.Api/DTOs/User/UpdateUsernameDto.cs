using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.DTOs;

public class UpdateUsernameDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
}
