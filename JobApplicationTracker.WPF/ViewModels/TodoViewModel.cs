using CommunityToolkit.Mvvm.ComponentModel;
using JobApplicationTracker.Wpf.Services;

namespace JobApplicationTracker.Wpf.ViewModels;

public record CalendarCell(int Day, bool IsCurrentMonth, bool IsToday, bool HasEvent);

public record UpcomingEvent(string Company, string EventDescription, string EventDate);

public partial class TodoViewModel : ObservableObject
{
    private readonly ApplicationApiService _api = new(SessionStore.Token);
    private DateTime _viewDate = new(DateTime.Today.Year, DateTime.Today.Month, 1);

    public string MonthLabel =>
        _viewDate.ToString("MMMM yyyy");

    public List<CalendarCell> CalendarCells { get; private set; } = [];
    public List<UpcomingEvent> UpcomingEvents { get; private set; } = [];

    public async Task LoadAsync()
    {
        var apps = await _api.GetApplicationsAsync();

        var eventDates = apps
            .Where(a => a.JAEvent is not null)
            .Select(a => a.JAEvent!.EventDate.Date)
            .ToHashSet();

        BuildCalendar(eventDates);
        BuildUpcomingEvents(apps);
    }

    private void BuildCalendar(HashSet<DateTime> eventDates)
    {
        var cells = new List<CalendarCell>();
        var first = _viewDate;
        var last = new DateTime(_viewDate.Year, _viewDate.Month,
                                DateTime.DaysInMonth(_viewDate.Year, _viewDate.Month));
        var today = DateTime.Today;

        int startOffset = ((int)first.DayOfWeek + 6) % 7; // Mon=0
        for (int i = startOffset; i > 0; i--)
        {
            var d = first.AddDays(-i);
            cells.Add(new CalendarCell(d.Day, false, false, eventDates.Contains(d.Date)));
        }
        for (var d = first; d <= last; d = d.AddDays(1))
            cells.Add(new CalendarCell(d.Day, true, d.Date == today, eventDates.Contains(d.Date)));
        int endOffset = (7 - (int)last.DayOfWeek) % 7;
        for (int i = 1; i <= endOffset; i++)
        {
            var d = last.AddDays(i);
            cells.Add(new CalendarCell(d.Day, false, false, eventDates.Contains(d.Date)));
        }

        CalendarCells = cells;
    }

    private void BuildUpcomingEvents(List<Models.JobApplicationMinimal> apps)
    {
        var today = DateTime.Today;
        var cutoff = today.AddDays(14);
        UpcomingEvents = apps
            .Where(a => a.JAEvent is not null &&
                        a.JAEvent.EventDate.Date >= today &&
                        a.JAEvent.EventDate.Date <= cutoff)
            .OrderBy(a => a.JAEvent!.EventDate)
            .Select(a => new UpcomingEvent(
                a.Company,
                a.Position,
                a.JAEvent!.EventDate.ToString("ddd, MMM d")))
            .ToList();
    }

    public void PrevMonth()
    {
        _viewDate = _viewDate.AddMonths(-1);
        OnPropertyChanged(nameof(MonthLabel));
        BuildCalendar([]);
    }

    public void NextMonth()
    {
        _viewDate = _viewDate.AddMonths(1);
        OnPropertyChanged(nameof(MonthLabel));
        BuildCalendar([]);
    }
}