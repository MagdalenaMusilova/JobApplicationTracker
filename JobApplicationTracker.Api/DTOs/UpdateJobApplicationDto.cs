namespace JobApplicationTracker.DTOs;

public class UpdateJobApplicationDto
{
    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Note { get; set; }
    public string? JobDescription { get; set; }
}