using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobApplicationTracker.Enums;

namespace JobApplicationTracker.Models;

public class JAEvent
{
    [Key] 
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid JAStatusEntryId {get; set;}
    public JAStatusEntry JAStatusEntry { get; set; }
    [Required]
    [MaxLength(200)]
    public string EventName { get; set; }
    [Required]
    public JAEventType EventType { get; set; }
    [Required]
    public DateTime EventDate { get; set; }
    [Required]
    public bool IsWholeDay { get; set; }
    [MaxLength(1500)]
    public string? Note { get; set; } 
}