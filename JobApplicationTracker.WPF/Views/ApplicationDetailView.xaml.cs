using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using JobApplicationTracker.Wpf.Models;
using JobApplicationTracker.Wpf.ViewModels;

namespace JobApplicationTracker.Wpf.Views;

public partial class ApplicationDetailView : Page
{
    private readonly ApplicationDetailViewModel _vm = new();

    public ApplicationDetailView(JobApplicationMinimal minimal)
    {
        InitializeComponent();
        DataContext = _vm;
        Loaded += async (_, _) => await _vm.LoadAsync(minimal.Id);
    }

    private void Back_Click(object sender, RoutedEventArgs e)
        => NavigationService?.GoBack();
}