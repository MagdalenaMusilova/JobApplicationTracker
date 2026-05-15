using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobApplicationTracker.Wpf.Models;

namespace JobApplicationTracker.Wpf.ViewModels;

public partial class CreateApplicationViewModel : ObservableObject
{
    [ObservableProperty] private string company = string.Empty;
    [ObservableProperty] private string position = string.Empty;
    [ObservableProperty] private string? note;
    [ObservableProperty] private string? jobDescription;

    [ObservableProperty] private StatusItem selectedStatus;
    [ObservableProperty] private string? statusNote;

    [ObservableProperty] private bool addEvent;
    [ObservableProperty] private string eventName = string.Empty;
    [ObservableProperty] private EventTypeItem selectedEventType;
    [ObservableProperty] private DateTime eventDate = DateTime.Today;
    [ObservableProperty] private bool isWholeDay;
    [ObservableProperty] private string? eventNote;

    // todo fetch from api
    public ObservableCollection<StatusItem> StatusTypes { get; } =
    [
        new(100, "Wishlist"),
        new(200, "Applied"),
        new(300, "Task"),
        new(400, "Interview"),
        new(500, "Offer"),
        new(1000, "Accepted"),
        new(2000, "Rejected"),
    ];

    public ObservableCollection<EventTypeItem> EventTypes { get; } =
    [
        new(0, "Other"),
        new(1, "Call"),
        new(2, "Interview"),
        new(3, "Deadline"),
    ];

    public bool Confirmed { get; private set; }

    public CreateApplicationViewModel()
    {
        selectedStatus = StatusTypes[0];
        selectedEventType = EventTypes[0];
    }

    [RelayCommand]
    private void Confirm()
    {
        if (string.IsNullOrWhiteSpace(Company) || string.IsNullOrWhiteSpace(Position))
            return;

        Confirmed = true;
        RequestClose?.Invoke();
    }

    [RelayCommand]
    private void Cancel() => RequestClose?.Invoke();

    public Action? RequestClose { get; set; }

    public CreateApplicationRequest BuildRequest() => new()
    {
        Company = Company,
        Position = Position,
        Note = string.IsNullOrWhiteSpace(Note) ? null : Note,
        JobDescription = string.IsNullOrWhiteSpace(JobDescription) ? null : JobDescription,
        InitialStatus = new CreateStatusEntryRequest
        {
            StatusType = SelectedStatus.Value,
            Note = string.IsNullOrWhiteSpace(StatusNote) ? null : StatusNote,
        },
        JAEvent = AddEvent ? new CreateEventRequest
        {
            EventName = EventName,
            EventType = SelectedEventType.Value,
            EventDate = EventDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            IsWholeDay = IsWholeDay,
            Note = string.IsNullOrWhiteSpace(EventNote) ? null : EventNote,
        } : null,
    };
}

public record StatusItem(int Value, string Label);
public record EventTypeItem(int Value, string Label);