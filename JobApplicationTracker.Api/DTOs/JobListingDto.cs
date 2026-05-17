using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationTracker.DTOs;

[NotMapped]
public class JobListingDto
{
    public string JobDescription { get; set; } = string.Empty;  //full description of the job
}