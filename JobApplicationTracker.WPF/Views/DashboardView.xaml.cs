using System.Windows.Controls;
using JobApplicationTracker.Wpf.ViewModels;

namespace JobApplicationTracker.Wpf.Views;

public partial class DashboardView : Page
{
    public DashboardView()
    {
        InitializeComponent();
        var vm = new DashboardViewModel();
        DataContext = vm;
        Loaded += async (_, _) => await vm.LoadAsync();
    }
}