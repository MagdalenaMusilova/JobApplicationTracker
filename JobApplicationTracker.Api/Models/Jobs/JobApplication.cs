using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models;

public class JobApplication
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Company { get; set; }
    [Required]
    [MaxLength(200)]
    public string Position { get; set; }
    [MaxLength(5000)]
    public string? Note { get; set; }
    public JobListing? JobListing { get; set; }
    public List<JAStatusEntry> StatusHistory { get; set; } = new();

}