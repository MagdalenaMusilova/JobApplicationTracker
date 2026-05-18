using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.DTOs.JAEvent;

namespace JobApplicationTracker.DTOs;

public class JobApplicationMinimalDto
{
    public Guid Id { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    public JaStatusTypeDto JAStatus { get; set; }
    public JAEventMinimalDto? JAEvent { get; set; }
}