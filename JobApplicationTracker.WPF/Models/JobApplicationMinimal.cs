using JobApplicationTracker.Wpf.Models.Enums;

namespace JobApplicationTracker.Wpf.Models;

public class JobApplicationMinimal
{
    public Guid Id { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public string? Note { get; set; }
    public JaStatusType JAStatus { get; set; }
    public JAEventMinimal? JAEvent { get; set; }
}