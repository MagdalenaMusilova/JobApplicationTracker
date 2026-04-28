namespace JobApplicationTracker.Models;

public class JobApplication
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public ICollection<JAStatusEntry> StatusHistory { get; set; } = new List<JAStatusEntry>();
    public string? Note { get; set; }
}