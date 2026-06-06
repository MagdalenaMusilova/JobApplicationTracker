using JobApplicationTracker.DTOs.JAEvent;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class JobApplicationMinimalDto
{
    public Guid JAId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public JAStatusType JAStatus { get; set; }
    public JAEventType? EventType { get; set; }
    public DateTime? EventDate  { get; set; }
    public bool? IsWholeDay { get; set; }
    public DateTime UpdatedAt { get; set; }
}