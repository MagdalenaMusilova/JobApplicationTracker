using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using JobApplicationTracker.Wpf.ViewModels;

namespace JobApplicationTracker.Wpf.Views;

public partial class TodoView : Page
{
    private readonly TodoViewModel _vm;

    public TodoView()
    {
        InitializeComponent();
        _vm = new TodoViewModel();
        Loaded += async (_, _) =>
        {
            await _vm.LoadAsync();
            RenderCalendar();
            RenderEvents();
        };
    }

    private void PrevMonth_Click(object sender, RoutedEventArgs e)
    {
        _vm.PrevMonth();
        RenderCalendar();
    }

    private void NextMonth_Click(object sender, RoutedEventArgs e)
    {
        _vm.NextMonth();
        RenderCalendar();
    }

    private void RenderCalendar()
    {
        MonthLabel.Text = _vm.MonthLabel;
        var grid = new UniformGrid { Columns = 7 };

        foreach (var cell in _vm.CalendarCells)
        {
            var bg = cell.IsToday ? new SolidColorBrush(Color.FromRgb(37, 99, 235))
                : cell.HasEvent ? new SolidColorBrush(Color.FromRgb(219, 234, 254))
                : Brushes.Transparent;

            var fg = cell.IsToday ? Brushes.White
                : cell.IsCurrentMonth ? Brushes.Black
                : Brushes.LightGray;

            var border = new Border
            {
                Height = 36, Margin = new Thickness(2),
                CornerRadius = new CornerRadius(6),
                Background = bg,
                Child = new TextBlock
                {
                    Text = cell.Day.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = fg, FontSize = 13
                }
            };
            grid.Children.Add(border);
        }

        CalendarGrid.Content = grid;
    }

    private void RenderEvents()
    {
        var events = _vm.UpcomingEvents;
        NoEventsText.Visibility = events.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        EventsList.ItemsSource = events;
    }
}