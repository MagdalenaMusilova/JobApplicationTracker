using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Models;

public class User : IdentityUser
{
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;
}