using JobApplicationTracker.Wpf.Models.Enums;

namespace JobApplicationTracker.Wpf.Models;

public class ApplicationDetail
{
    public Guid Id { get; set; }
    public string Company { get; set; } = "";
    public string Position { get; set; } = "";
    public string? Note { get; set; }
    public string? JobDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<StatusEntry> StatusHistory { get; set; } = [];
}

public class StatusEntry
{
    public Guid Id { get; set; }
    public Guid JobApplicationId { get; set; }
    public int OrderIndex { get; set; }
    public JaStatusType JaStatusType { get; set; } = new() { Label = "", Value = 0 };
    public DateTime CreatedAt { get; set; }
    public string? Note { get; set; }
    public List<AppEvent> Events { get; set; } = [];
}

public class AppEvent
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = "";
    public JaEventType EventType { get; set; } = new() { Label = "", Value = 0 };
    public DateTime EventDate { get; set; }
    public bool IsWholeDay { get; set; }
    public string? Note { get; set; }
}

public class UpdateApplicationRequest
{
    public string? Company { get; set; }
    public string? Position { get; set; }
    public string? Note { get; set; }
    public string? JobDescription { get; set; }
}

public class CreateStatusRequest
{
    public Guid JobApplicationId { get; set; }
    public int StatusType { get; set; }
    public string? Note { get; set; }
}

public class UpdateStatusRequest
{
    public Guid JobApplicationId { get; set; }
    public int StatusType { get; set; }
    public string? Note { get; set; }
}

public class CreateAppEventRequest
{
    public Guid JAStatusEntryId { get; set; }
    public string EventName { get; set; } = "";
    public int EventType { get; set; }
    public string EventDate { get; set; } = "";
    public bool IsWholeDay { get; set; }
    public string? Note { get; set; }
}

public class UpdateAppEventRequest
{
    public string EventName { get; set; } = "";
    public int EventType { get; set; }
    public string EventDate { get; set; } = "";
    public bool IsWholeDay { get; set; }
    public string? Note { get; set; }
}