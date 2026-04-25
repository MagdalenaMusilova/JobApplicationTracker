using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class CreateJobApplicationDto
{
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    public JAStatus JaStatus { get; set; }
    public string? JaStatusNote { get; set; }
}