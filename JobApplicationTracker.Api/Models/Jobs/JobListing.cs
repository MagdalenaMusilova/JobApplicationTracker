using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationTracker.Models;

public class JobListing
{
    public Guid Id { get; set; }
    public string JobDescription { get; set; } = string.Empty;
    public IEnumerable<string> HardSkills { get; set; } = new List<string>();
    public IEnumerable<string> SoftSkills { get; set; } = new List<string>();
    public IEnumerable<string> RequiredWorkExperience { get; set; } = new List<string>();
    public IEnumerable<string> RequiredEducation { get; set; } = new List<string>();
    public IEnumerable<string> RequiredCertification { get; set; } = new List<string>();
    public IEnumerable<string> OtherRequirements { get; set; } = new List<string>();
    public string ThisFitsThisKindOfPerson { get; set; }
    
    [ForeignKey("JobApplication")]
    public Guid JobApplicationId { get; set; }
}