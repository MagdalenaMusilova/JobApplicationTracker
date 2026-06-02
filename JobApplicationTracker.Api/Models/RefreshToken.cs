using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models;

public class RefreshToken
{
    public int Id { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt is not null;

    public bool IsActive => !IsExpired && !IsRevoked;
}