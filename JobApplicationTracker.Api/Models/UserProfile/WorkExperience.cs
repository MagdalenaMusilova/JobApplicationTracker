using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models.UserProfile;

public class WorkExperience : Experience, IValidatableObject
{
    [Required]
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [Required]
    [MaxLength(200)]
    public required string Company { get; set; }
    [Required]
    [MaxLength(200)]
    public required string Position { get; set; }
    [MaxLength(2500)]
    public string? JobDescription { get; set; }
    [MaxLength(2500)]
    public string? Notes { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate >= EndDate)
        {
            yield return new ValidationResult(
                "StartDate must be earlier than EndDate",
                new[] { nameof(StartDate), nameof(EndDate) }
            );
        }
    }
}