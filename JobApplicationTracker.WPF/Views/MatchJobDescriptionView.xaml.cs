using System.Windows;
using System.Windows.Controls;

namespace JobApplicationTracker.Wpf.Views;

public partial class MatchJobDescriptionView : Page
{
    public MatchJobDescriptionView() => InitializeComponent();

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        JobDescBox.Clear();
        ResultPanel.Visibility = Visibility.Collapsed;
        PlaceholderText.Visibility = Visibility.Visible;
    }

    private void Analyze_Click(object sender, RoutedEventArgs e)
    {
        var text = JobDescBox.Text.Trim();
        if (string.IsNullOrEmpty(text)) return;

        var keywords = new[] { "C#", ".NET", "SQL", "Azure", "REST", "API", "WPF", "LINQ" };
        var lowerText = text.ToLower();
        var matched = keywords.Where(k => lowerText.Contains(k.ToLower())).ToList();
        var missing  = keywords.Except(matched).ToList();
        var score    = keywords.Length == 0 ? 0 : (int)((double)matched.Count / keywords.Length * 100);

        ScoreText.Text = $"{score}%";
        ScoreBar.Value = score;
        SummaryText.Text = score >= 70
            ? "Great match! This role aligns well with your profile."
            : score >= 40
                ? "Moderate match. Some upskilling may help."
                : "Low match. Consider whether this role is the right fit.";
        MatchedSkillsText.Text = matched.Count > 0 ? string.Join(", ", matched) : "None found.";
        MissingSkillsText.Text = missing.Count > 0 ? string.Join(", ", missing) : "Nothing obvious missing!";

        ResultPanel.Visibility = Visibility.Visible;
        PlaceholderText.Visibility = Visibility.Collapsed;
    }
}