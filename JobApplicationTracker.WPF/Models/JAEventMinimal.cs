using JobApplicationTracker.Wpf.Models.Enums;

namespace JobApplicationTracker.Wpf.Models;

public class JAEventMinimal
{
    Guid Id { get; set; }
    string EventName { get; set; }
    public JaEventType EventType { get; set; }
    public DateTime EventDate  { get; set; }
    public bool IsWholeDay { get; set; }
}