using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.DTOs.JAEvent;

namespace JobApplicationTracker.DTOs;

public class MinimalJobApplicationDto
{
    public Guid Id { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    public JaStatusTypeDto JAStatus { get; set; }
    public MinimalJAEventDto? JAEvent { get; set; }
}