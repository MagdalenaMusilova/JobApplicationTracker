using CommunityToolkit.Mvvm.ComponentModel;
using JobApplicationTracker.Wpf.Models.Profile;
using JobApplicationTracker.Wpf.Services;

namespace JobApplicationTracker.Wpf.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly ProfileApiService _api = new(SessionStore.Token);

    [ObservableProperty] private string username = "—";
    [ObservableProperty] private string email = "—";
    [ObservableProperty] private string aboutMe = "No summary added yet.";
    [ObservableProperty] private string notes = "—";
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? errorMessage;

    [ObservableProperty] private List<WorkExperienceDto> workExperiences = [];
    [ObservableProperty] private List<EducationDto> educationItems = [];
    [ObservableProperty] private List<TrainingDto> trainings = [];
    [ObservableProperty] private List<JobSkillDto> skills = [];

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public string EducationCount => $"{EducationItems.Count} item{(EducationItems.Count == 1 ? "" : "s")}";
    public string WorkExpCount  => $"{WorkExperiences.Count} item{(WorkExperiences.Count == 1 ? "" : "s")}";
    public string TrainingsCount => $"{Trainings.Count} item{(Trainings.Count == 1 ? "" : "s")}";
    public string SkillsCount   => $"{Skills.Count} item{(Skills.Count == 1 ? "" : "s")}";

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var user = await _api.GetMeAsync();
            if (user is not null)
            {
                Username = user.UserName;
                Email = string.IsNullOrWhiteSpace(user.Email) ? "—" : user.Email;

                var resume = await _api.GetResumeByUserIdAsync(user.Id);
                if (resume is not null)
                {
                    AboutMe     = string.IsNullOrWhiteSpace(resume.AboutMe) ? "No summary added yet." : resume.AboutMe;
                    Notes       = string.IsNullOrWhiteSpace(resume.Notes)   ? "—" : resume.Notes;
                    WorkExperiences = resume.WorkExperiences;
                    EducationItems  = resume.Education;
                    Trainings       = resume.Trainings;
                    Skills          = resume.Skills;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            OnPropertyChanged(nameof(HasError));
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(EducationCount));
            OnPropertyChanged(nameof(WorkExpCount));
            OnPropertyChanged(nameof(TrainingsCount));
            OnPropertyChanged(nameof(SkillsCount));
        }
    }
}