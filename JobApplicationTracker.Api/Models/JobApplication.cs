namespace JobApplicationTracker.Models;

public class JobApplication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public IEnumerable<JAStatusEntry> StatusHistory { get; set; }
    public string? Note { get; set; }
}