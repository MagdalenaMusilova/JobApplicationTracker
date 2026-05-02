using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class CreateJobApplicationDto
{
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    public CreateJAStatusEntryDto InitialStatus { get; set; }
    public string? JobDescription { get; set; }
    public CreateJAEventDto? JAEvent { get; set; }
}