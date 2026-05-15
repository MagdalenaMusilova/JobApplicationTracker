using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobApplicationTracker.Wpf.Models;
using JobApplicationTracker.Wpf.Services;

namespace JobApplicationTracker.Wpf.ViewModels;

public partial class ApplicationDetailViewModel : ObservableObject
{
    private readonly ApplicationApiService _api = new(SessionStore.Token);

    //Detail fields
    [ObservableProperty] private Guid appId;
    [ObservableProperty] private string company = "";
    [ObservableProperty] private string position = "";
    [ObservableProperty] private string? note;
    [ObservableProperty] private string? jobDescription;
    [ObservableProperty] private DateTime createdAt;
    [ObservableProperty] private DateTime lastStatusUpdatedAt;
    [ObservableProperty] private ObservableCollection<StatusEntry> statusHistory = [];
    [ObservableProperty] private StatusEntry? lastStatus;
    [ObservableProperty] private AppEvent? upcomingEvent;

    // Edit mode
    [ObservableProperty] private bool isEditMode;
    [ObservableProperty] private string editCompany = "";
    [ObservableProperty] private string editPosition = "";
    [ObservableProperty] private string? editNote;
    [ObservableProperty] private string? editJobDescription;

    // Dialog visibility
    [ObservableProperty] private bool isHistoryOpen;
    [ObservableProperty] private bool isAddStatusOpen;
    [ObservableProperty] private bool isEditStatusOpen;
    [ObservableProperty] private bool isAddEventOpen;
    [ObservableProperty] private bool isEditEventOpen;

    // Status dialog fields
    [ObservableProperty] private int newStatusType;
    [ObservableProperty] private string? newStatusNote;
    [ObservableProperty] private int editStatusType;
    [ObservableProperty] private string? editStatusNote;

    // Event dialog fields
    [ObservableProperty] private string newEventName = "";
    [ObservableProperty] private int newEventType;
    [ObservableProperty] private DateTime newEventDate = DateTime.Now;
    [ObservableProperty] private bool newEventIsWholeDay;
    [ObservableProperty] private string? newEventNote;

    [ObservableProperty] private string editEventName = "";
    [ObservableProperty] private int editEventType;
    [ObservableProperty] private DateTime editEventDate = DateTime.Now;
    [ObservableProperty] private bool editEventIsWholeDay;
    [ObservableProperty] private string? editEventNote;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public async Task LoadAsync(Guid id)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var detail = await _api.GetDetailAsync(id);
            if (detail is not null) Apply(detail);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(HasError));
        }
        finally { IsLoading = false; }
    }

    private void Apply(ApplicationDetail d)
    {
        AppId = d.Id;
        Company = d.Company;
        Position = d.Position;
        Note = d.Note;
        JobDescription = d.JobDescription;
        CreatedAt = d.CreatedAt;
        StatusHistory = new ObservableCollection<StatusEntry>(d.StatusHistory.OrderBy(s => s.OrderIndex));
        LastStatus = StatusHistory.LastOrDefault();
        LastStatusUpdatedAt = LastStatus?.CreatedAt ?? d.CreatedAt;
        UpcomingEvent = LastStatus?.Events
            .Where(e => e.EventDate >= DateTime.Now)
            .OrderBy(e => e.EventDate)
            .FirstOrDefault();
    }

    [RelayCommand]
    private void StartEdit()
    {
        EditCompany = Company;
        EditPosition = Position;
        EditNote = Note;
        EditJobDescription = JobDescription;
        IsEditMode = true;
    }

    [RelayCommand]
    private void CancelEdit() => IsEditMode = false;

    [RelayCommand]
    private async Task ConfirmSaveAsync()
    {
        if (MessageBox.Show("Save changes to this application?", "Confirm",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK) return;
        try
        {
            var updated = await _api.UpdateApplicationAsync(AppId, new UpdateApplicationRequest
            {
                Company = EditCompany,
                Position = EditPosition,
                Note = EditNote,
                JobDescription = EditJobDescription
            });
            if (updated is not null) Apply(updated);
            IsEditMode = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand] private void OpenHistory() => IsHistoryOpen = true;
    [RelayCommand] private void CloseHistory() => IsHistoryOpen = false;

    [RelayCommand]
    private void OpenAddStatus()
    {
        NewStatusType = 0;
        NewStatusNote = null;
        IsAddStatusOpen = true;
    }

    [RelayCommand] private void CloseAddStatus() => IsAddStatusOpen = false;

    [RelayCommand]
    private async Task ConfirmAddStatusAsync()
    {
        try
        {
            var updated = await _api.PushStatusAsync(new CreateStatusRequest
            {
                JobApplicationId = AppId,
                StatusType = NewStatusType,
                Note = NewStatusNote
            });
            if (updated is not null) Apply(updated);
            IsAddStatusOpen = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void OpenEditStatus()
    {
        if (LastStatus is null) return;
        EditStatusType = LastStatus.JaStatusType.Value;
        EditStatusNote = LastStatus.Note;
        IsEditStatusOpen = true;
    }

    [RelayCommand] private void CloseEditStatus() => IsEditStatusOpen = false;

    [RelayCommand]
    private async Task ConfirmEditStatusAsync()
    {
        if (LastStatus is null) return;
        if (MessageBox.Show("Save changes to this status entry?", "Confirm",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK) return;
        try
        {
            await _api.UpdateStatusEntryAsync(LastStatus.Id, new UpdateStatusRequest
            {
                JobApplicationId = AppId,
                StatusType = EditStatusType,
                Note = EditStatusNote
            });
            await LoadAsync(AppId);
            IsEditStatusOpen = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task DeleteLastStatusAsync()
    {
        if (LastStatus is null) return;
        if (MessageBox.Show("Delete this status entry? This cannot be undone.", "Confirm Delete",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK) return;
        try
        {
            var updated = await _api.DeleteStatusEntryAsync(LastStatus.Id);
            if (updated is not null) Apply(updated);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void OpenAddEvent()
    {
        NewEventName = "";
        NewEventType = 0;
        NewEventDate = DateTime.Now;
        NewEventIsWholeDay = false;
        NewEventNote = null;
        IsAddEventOpen = true;
    }

    [RelayCommand] private void CloseAddEvent() => IsAddEventOpen = false;

    [RelayCommand]
    private async Task ConfirmAddEventAsync()
    {
        if (LastStatus is null) return;
        try
        {
            await _api.CreateEventAsync(new CreateAppEventRequest
            {
                JAStatusEntryId = LastStatus.Id,
                EventName = NewEventName,
                EventType = NewEventType,
                EventDate = NewEventDate.ToString("o"),
                IsWholeDay = NewEventIsWholeDay,
                Note = NewEventNote
            });
            await LoadAsync(AppId);
            IsAddEventOpen = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private void OpenEditEvent()
    {
        if (UpcomingEvent is null) return;
        EditEventName = UpcomingEvent.EventName;
        EditEventType = UpcomingEvent.EventType.Value;
        EditEventDate = UpcomingEvent.EventDate;
        EditEventIsWholeDay = UpcomingEvent.IsWholeDay;
        EditEventNote = UpcomingEvent.Note;
        IsEditEventOpen = true;
    }

    [RelayCommand] private void CloseEditEvent() => IsEditEventOpen = false;

    [RelayCommand]
    private async Task ConfirmEditEventAsync()
    {
        if (UpcomingEvent is null) return;
        if (MessageBox.Show("Save changes to this event?", "Confirm",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK) return;
        try
        {
            await _api.UpdateEventAsync(UpcomingEvent.Id, new UpdateAppEventRequest
            {
                EventName = EditEventName,
                EventType = EditEventType,
                EventDate = EditEventDate.ToString("o"),
                IsWholeDay = EditEventIsWholeDay,
                Note = EditEventNote
            });
            await LoadAsync(AppId);
            IsEditEventOpen = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task DeleteEventAsync()
    {
        if (UpcomingEvent is null) return;
        if (MessageBox.Show("Delete this event? This cannot be undone.", "Confirm Delete",
                MessageBoxButton.OKCancel, MessageBoxImage.Warning) != MessageBoxResult.OK) return;
        try
        {
            await _api.DeleteEventAsync(UpcomingEvent.Id);
            await LoadAsync(AppId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}