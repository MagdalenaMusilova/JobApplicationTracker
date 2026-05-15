namespace JobApplicationTracker.Wpf.Models;

public class CreateApplicationRequest
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? Note { get; set; }
    public CreateStatusEntryRequest InitialStatus { get; set; } = new();
    public string? JobDescription { get; set; }
    public CreateEventRequest? JAEvent { get; set; }
}

public class CreateStatusEntryRequest
{
    public int StatusType { get; set; }
    public string? Note { get; set; }
}

public class CreateEventRequest
{
    public string EventName { get; set; } = string.Empty;
    public int EventType { get; set; }
    public string EventDate { get; set; } = string.Empty;
    public bool IsWholeDay { get; set; }
    public string? Note { get; set; }
}