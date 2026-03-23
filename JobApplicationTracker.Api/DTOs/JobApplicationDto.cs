namespace JobApplicationTracker.DTOs;

public class JobApplicationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public IEnumerable<JAStatusEntryDto> StatusHistory { get; set; }
    public string? Note { get; set; }
}