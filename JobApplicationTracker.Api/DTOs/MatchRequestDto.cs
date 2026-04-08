namespace JobApplicationTracker.DTOs;

public class MatchRequestDto
{
    public IFormFile ResumeFile { get; set; } = default!;
    public string JobListing { get; set; } = string.Empty;
}