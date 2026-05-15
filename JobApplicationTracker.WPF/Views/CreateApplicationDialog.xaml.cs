using System.Windows;
using JobApplicationTracker.Wpf.ViewModels;

namespace JobApplicationTracker.Wpf.Views;

public partial class CreateApplicationDialog : Window
{
    public CreateApplicationViewModel ViewModel { get; }

    public CreateApplicationDialog()
    {
        InitializeComponent();
        ViewModel = new CreateApplicationViewModel();
        ViewModel.RequestClose = Close;
        DataContext = ViewModel;
    }
}