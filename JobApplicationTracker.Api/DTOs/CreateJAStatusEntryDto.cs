using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class CreateJAStatusEntryDto
{
    public Guid JobApplicationId { get; set; }
    public JAStatus JaStatus { get; set; }
    public string? Note { get; set; }
}