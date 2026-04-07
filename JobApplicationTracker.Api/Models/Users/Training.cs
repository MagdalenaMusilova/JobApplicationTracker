namespace JobApplicationTracker.Models.Users;

public class Training
{
    public Guid Id { get; set; }
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Name { get; set; }   
    public string Type { get; set; }   // what's the training about 
    public IEnumerable<string>? Certification { get; set; }
    public ICollection<SkillUsage> Skills { get; set; } = new List<SkillUsage>();
    public string? Notes { get; set; } = string.Empty;   
}