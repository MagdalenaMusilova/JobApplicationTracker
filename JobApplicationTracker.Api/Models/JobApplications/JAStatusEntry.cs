using System.ComponentModel.DataAnnotations;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Models;

public class JAStatusEntry
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid JobApplicationId { get; set; }
    [Required]
    public int OrderIndex { get; set; }
    [Required]
    public JAStatusType JaStatusType { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(2500)]
    public string? Note { get; set; }
    public JAEvent? JAEvent { get; set; }
}