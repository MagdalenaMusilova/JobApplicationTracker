using System.Windows.Controls;
using JobApplicationTracker.Wpf.ViewModels;

namespace JobApplicationTracker.Wpf.Views;

public partial class ProfileView : Page
{
    private readonly ProfileViewModel _vm = new();

    public ProfileView()
    {
        InitializeComponent();
        DataContext = _vm;
        Loaded += async (_, _) => await _vm.LoadAsync();
    }
}