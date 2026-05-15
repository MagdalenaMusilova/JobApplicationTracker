using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.DTOs;

public class UserDto : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; } = null;
    
}