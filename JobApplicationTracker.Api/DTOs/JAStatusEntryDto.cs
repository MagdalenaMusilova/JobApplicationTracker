using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class JAStatusEntryDto
{
    public Guid Id { get; set; }
    public Guid JobApplicationId { get; set; }
    public int OrderIndex { get; set; }
    public JAStatus JaStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public string? Note { get; set; }
}