using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Dos;

public class JAStatusEntryDo
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public int OrderIndex { get; set; }
    public JAStatus JaStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
    public string? Note { get; set; }
}