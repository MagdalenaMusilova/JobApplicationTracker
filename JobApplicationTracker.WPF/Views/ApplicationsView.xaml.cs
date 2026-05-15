using System.Windows.Controls;
using JobApplicationTracker.Wpf.ViewModels;

namespace JobApplicationTracker.Wpf.Views;

public partial class ApplicationsView : Page
{
    private readonly ApplicationsViewModel _viewModel;

    public ApplicationsView()
    {
        InitializeComponent();
        _viewModel = new ApplicationsViewModel(NavigateTo);
        DataContext = _viewModel;
        Loaded += async (_, _) => await _viewModel.LoadApplicationsCommand.ExecuteAsync(null);
    }

    private void NavigateTo(Page page) => NavigationService?.Navigate(page);
}