using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class JAEventMinimalDto
{
    Guid Id { get; set; }
    string EventName { get; set; }
    public JAEventType EventType { get; set; }
    public DateTime EventDate  { get; set; }
    public bool IsWholeDay { get; set; }
}