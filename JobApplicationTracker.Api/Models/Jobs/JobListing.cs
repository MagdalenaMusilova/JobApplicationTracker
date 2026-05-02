using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationTracker.Models;

public class JobListing
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid JobApplicationId { get; set; }
    [Required]
    [MaxLength(20000)]
    public required string JobDescription { get; set; }
}