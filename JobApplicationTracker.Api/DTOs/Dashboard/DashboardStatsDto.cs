namespace JobApplicationTracker.DTOs;

public class DashboardStatsDto
{
    public int TotalApplications { get; set; }
    public int ActiveApplications { get; set; }
    public int InterviewsScheduled { get; set; }
    public int OffersReceived { get; set; }
    public Dictionary<int, int> ApplicationsByStatus { get; set; } = new();
    public int ApplicationsThisWeek { get; set; }
    public int ApplicationsThisMonth { get; set; }
}
