using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Models;

public class JAEventDto
{
    public Guid Id { get; set; }
    public int JAStatusEntryId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public JAEventType EventType { get; set; }
    public DateTime EventDate { get; set; }
    public bool IsWholeDay { get; set; }
    public string? Note { get; set; } = string.Empty;
}