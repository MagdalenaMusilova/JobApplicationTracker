namespace JobApplicationTracker.Dos;

public class JobApplicationDo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public IEnumerable<JAStatusEntryDo> StatusHistory { get; set; }
    public string? Note { get; set; }
}