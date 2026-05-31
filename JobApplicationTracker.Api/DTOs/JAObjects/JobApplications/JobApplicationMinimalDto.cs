using JobApplicationTracker.DTOs.JAEvent;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class JobApplicationMinimalDto
{
    public Guid Id { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public JAEventType JAStatus { get; set; }
    public JAEventMinimalDto? JAEvent { get; set; }
}