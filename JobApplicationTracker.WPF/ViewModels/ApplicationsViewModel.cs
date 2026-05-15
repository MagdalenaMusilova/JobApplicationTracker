using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JobApplicationTracker.Wpf.Models;
using JobApplicationTracker.Wpf.Models.Enums;
using JobApplicationTracker.Wpf.Services;
using JobApplicationTracker.Wpf.Views;

namespace JobApplicationTracker.Wpf.ViewModels;

public partial class StatusFilterItem(string label, Func<JaStatusType, bool>? predicate = null)
    : ObservableObject
{
    public string Label { get; } = label;

    public Func<JaStatusType, bool>? Predicate { get; } = predicate;

    [ObservableProperty] private bool isSelected;
}

public partial class ApplicationsViewModel : ObservableObject
{
    private readonly ApplicationApiService _apiService;
    private readonly StatusApiService _statusApiService;
    private readonly Action<Page> _navigate;
    private List<JobApplicationMinimal> _allApplications = [];

    private static readonly HashSet<string> OpenedLabels =
        new(StringComparer.OrdinalIgnoreCase) { "Wishlist", "Applied" };

    private static readonly HashSet<string> InProgressLabels =
        new(StringComparer.OrdinalIgnoreCase) { "Task", "Interview" };

    [ObservableProperty] private ObservableCollection<JobApplicationMinimal> applications = [];
    [ObservableProperty] private ObservableCollection<StatusFilterItem> statusFilters = [];
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;

    public ApplicationsViewModel(Action<Page> navigate)
    {
        _navigate = navigate;
        _apiService = new ApplicationApiService(SessionStore.Token);
        _statusApiService = new StatusApiService(SessionStore.Token);
    }

    [RelayCommand]
    public async Task LoadApplicationsAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var (apps, statuses) = (
                await _apiService.GetApplicationsAsync(),
                await _statusApiService.GetStatusTypesAsync()
            );
            _allApplications = apps;

            BuildStatusFilters(statuses);
            ApplyFilter();
        }
        catch (HttpRequestException ex) { ErrorMessage = $"Could not connect to the API: {ex.Message}"; }
        catch (Exception ex)            { ErrorMessage = $"Unexpected error: {ex.Message}"; }
        finally                         { IsLoading = false; }
    }

    private void BuildStatusFilters(List<JaStatusType> statuses)
    {
        var items = new List<StatusFilterItem>
        {
            new("All statuses"),
            new("Opened",      s => OpenedLabels.Contains(s.Label)),
            new("In Progress", s => InProgressLabels.Contains(s.Label)),
        };

        foreach (var s in statuses)
            items.Add(new StatusFilterItem(s.Label, status => status.Value == s.Value));

        foreach (var item in items)
            item.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != nameof(StatusFilterItem.IsSelected)) return;
                if (sender is StatusFilterItem selected && selected.IsSelected)
                {
                    foreach (var other in StatusFilters)
                        if (!ReferenceEquals(other, selected))
                            other.IsSelected = false;
                    ApplyFilter();
                }
            };

        items[0].IsSelected = true;
        StatusFilters = new ObservableCollection<StatusFilterItem>(items);
    }

    private void ApplyFilter()
    {
        var active = StatusFilters.FirstOrDefault(f => f.IsSelected);
        var filtered = active?.Predicate is null
            ? _allApplications
            : _allApplications.Where(a => active.Predicate(a.JAStatus));

        Applications = new ObservableCollection<JobApplicationMinimal>(filtered);
    }

    [RelayCommand]
    public void OpenDetail(JobApplicationMinimal application)
        => _navigate(new ApplicationDetailView(application));

    [RelayCommand]
    public void OpenCreateDialog()
    {
        var owner = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        var dialog = new CreateApplicationDialog { Owner = owner };
        dialog.ShowDialog();

        if (dialog.ViewModel.Confirmed)
            LoadApplicationsCommand.ExecuteAsync(null);
    }
}