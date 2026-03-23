namespace JobApplicationTracker.DTOs;

public class UpdateJobApplicationDto
{
    public int Id { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Note { get; set; }
}