using JobApplicationTracker.DTOs.Enums;

namespace JobApplicationTracker.DTOs.JAEvent;

public class MinimalJAEventDto
{
    Guid Id { get; set; }
    string EventName { get; set; }
    public JaEventTypeDto EventType { get; set; }
    public DateTime EventDate  { get; set; }
    public bool IsWholeDay { get; set; }
}