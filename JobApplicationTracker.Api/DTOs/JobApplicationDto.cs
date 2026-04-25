namespace JobApplicationTracker.DTOs;

public class JobApplicationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    public IEnumerable<JAStatusEntryDto> StatusHistory { get; set; } = [];
}