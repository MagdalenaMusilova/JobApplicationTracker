using System.Windows;
using JobApplicationTracker.Wpf.Services;
using JobApplicationTracker.Wpf.Views;

namespace JobApplicationTracker.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new DashboardView());
    }

    private void Dashboard_Click(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(new DashboardView());

    private void Applications_Click(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(new ApplicationsView());

    private void Todo_Click(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(new TodoView());

    private void Profile_Click(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(new ProfileView());

    private void Match_Click(object sender, RoutedEventArgs e)
        => MainFrame.Navigate(new MatchJobDescriptionView());

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        SessionStore.Clear();
        var login = new Views.LoginWindow();
        login.Show();
        Close();
    }
}