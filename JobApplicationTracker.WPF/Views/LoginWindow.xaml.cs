using System.Windows;
using JobApplicationTracker.Wpf.Services;

namespace JobApplicationTracker.Wpf.Views;

public partial class LoginWindow : Window
{
    private bool _isSignUp = false;

    public LoginWindow()
    {
        InitializeComponent();
        UpdateMode();
    }

    private void UpdateMode()
    {
        if (_isSignUp)
        {
            SubtitleText.Text = "Create an account to start tracking";
            PrimaryButton.Content = "Create account";
            ToggleModeButton.Content = "Already have an account? Log in";
        }
        else
        {
            SubtitleText.Text = "Welcome back — log in to continue";
            PrimaryButton.Content = "Log in";
            ToggleModeButton.Content = "Don't have an account? Sign up";
        }
    }

    private void ToggleModeButton_Click(object sender, RoutedEventArgs e)
    {
        _isSignUp = !_isSignUp;
        ErrorText.Visibility = Visibility.Collapsed;
        UpdateMode();
    }

    private async void PrimaryButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Visibility = Visibility.Collapsed;
        PrimaryButton.IsEnabled = false;
        PrimaryButton.Content = "Please wait...";

        var username = UsernameBox.Text.Trim();
        var password = PasswordBox.Password;

        try
        {
            var authService = new AuthApiService();

            if (_isSignUp)
                await authService.SignUpAsync(username, password);

            var token = await authService.SignInAsync(username, password);

            if (string.IsNullOrEmpty(token))
            {
                ShowError("Wrong username or password.");
                return;
            }

            SessionStore.SetToken(token);

            var main = new MainWindow();
            main.Show();
            Close();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            PrimaryButton.IsEnabled = true;
            UpdateMode(); 
        }
    }

    private void ShowError(string msg)
    {
        ErrorText.Text = msg;
        ErrorText.Visibility = Visibility.Visible;
    }
}