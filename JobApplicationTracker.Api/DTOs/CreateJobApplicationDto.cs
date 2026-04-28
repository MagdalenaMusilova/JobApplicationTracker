using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class CreateJobApplicationDto
{
    // for job application
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    // for ja status
    public CreateJAStatusEntryDto JaStatusEntry { get; set; }
    // for job listing
    public CreateJobListingDto JobDescription { get; set; }
    // for ja event
    public CreateJAEventDto JAEvent { get; set; }
}