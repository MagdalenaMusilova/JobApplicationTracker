using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class UpdateJAEventDto
{
    public string? EventName { get; set; } = string.Empty;
    public JAEventType? EventType { get; set; }
    public DateTime? EventDate { get; set; }
    public bool? IsWholeDay { get; set; }
    public string? Note { get; set; } = string.Empty;
}