using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using JobApplicationTracker.Wpf.Models;
using JobApplicationTracker.Wpf.Services;

namespace JobApplicationTracker.Wpf.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ApplicationApiService _api = new(SessionStore.Token);

    [ObservableProperty] private int total;
    [ObservableProperty] private int interviews;
    [ObservableProperty] private int offers;
    [ObservableProperty] private int rejected;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private ObservableCollection<JobApplicationMinimal> recentApplications = [];

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var apps = await _api.GetApplicationsAsync();
            Total = apps.Count;
            Interviews = apps.Count(a => a.JAStatus.Label.Contains("Interview", StringComparison.OrdinalIgnoreCase));
            Offers     = apps.Count(a => a.JAStatus.Label.Contains("Offer",     StringComparison.OrdinalIgnoreCase));
            Rejected   = apps.Count(a => a.JAStatus.Label.Contains("Reject",    StringComparison.OrdinalIgnoreCase));
            RecentApplications = new(apps.Take(5));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(HasError));
        }
        finally { IsLoading = false; }
    }
}