using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class CreateJAEventDto
{
    public Guid JAStatusEntryId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public int EventType { get; set; }
    public DateTime EventDate { get; set; }
    public bool IsWholeDay { get; set; }
    public string? Note { get; set; } = string.Empty;
}