using JobApplicationTracker.Models.Jobs;

namespace JobApplicationTracker.Models;

public class JobApplication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public JobListing JobListing { get; set; }
    public ICollection<JAStatusEntry> StatusHistory { get; set; } = new List<JAStatusEntry>();
    public string? Note { get; set; }
}