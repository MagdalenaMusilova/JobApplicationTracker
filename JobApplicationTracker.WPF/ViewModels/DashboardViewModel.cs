using CommunityToolkit.Mvvm.ComponentModel;

namespace JobApplicationTracker.Wpf.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private int totalApplications = 0;

    [ObservableProperty]
    private int interviews = 0;

    [ObservableProperty]
    private int offers = 0;
}