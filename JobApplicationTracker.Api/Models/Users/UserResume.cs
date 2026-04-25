namespace JobApplicationTracker.Models.Users;

public class UserResume
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ICollection<WorkExperience> WorkExperiences { get; set; } = new List<WorkExperience>();
    public ICollection<Education> Education { get; set; } = new List<Education>();
    public ICollection<Training> Trainings { get; set; } = new List<Training>();
    public ICollection<JobSkill> Skills { get; set; } = new List<JobSkill>();
    public ICollection<SkillUsage> UncategorizedSkillUsages { get; set; } = new List<SkillUsage>();
}