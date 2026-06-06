using JobApplicationTracker.Enums;

namespace JobApplicationTracker.DTOs;

public class CreateInitJAStatusDto
{
    public int StatusType { get; set; }
    public string? Note { get; set; }
}