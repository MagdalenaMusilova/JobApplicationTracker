using AutoMapper;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Models.UserProfile;

namespace JobApplicationTracker.Mapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UserResumeDto, UserResume>().ReverseMap();

        // Resume-related mappings
        CreateMap<WorkExperienceDto, WorkExperience>()
            .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src =>
                src.JobDescription != null ? string.Join("\n", src.JobDescription) : null))
            .ReverseMap()
            .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src =>
                !string.IsNullOrWhiteSpace(src.JobDescription) ? src.JobDescription.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>()))
            .ForMember(dest => dest.Skills, opt => opt.Ignore());

        CreateMap<EducationDto, Education>()
            .ForMember(dest => dest.Major, opt => opt.MapFrom(src =>
                src.Majors != null && src.Majors.Any() ? string.Join(", ", src.Majors) : string.Empty))
            .ReverseMap()
            .ForMember(dest => dest.Majors, opt => opt.MapFrom(src =>
                !string.IsNullOrWhiteSpace(src.Major) ? src.Major.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>()))
            .ForMember(dest => dest.Skills, opt => opt.Ignore());

        CreateMap<TrainingDto, Training>()
            .ForMember(dest => dest.Certification, opt => opt.MapFrom(src =>
                src.Certification != null && src.Certification.Any() ? string.Join(", ", src.Certification) : null))
            .ReverseMap()
            .ForMember(dest => dest.Certification, opt => opt.MapFrom(src =>
                !string.IsNullOrWhiteSpace(src.Certification) ? src.Certification.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>()))
            .ForMember(dest => dest.Skills, opt => opt.Ignore());

        CreateMap<JobSkillDto, ResumeSkill>()
            .ForMember(dest => dest.UserResumeId, opt => opt.Ignore())
            .ForMember(dest => dest.Usages, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Aliases, opt => opt.Ignore())
            .ForMember(dest => dest.Skills, opt => opt.Ignore());

        CreateMap<SkillUsageDto, SkillUsage>()
            .ForMember(dest => dest.ExperienceId, opt => opt.Ignore())
            .ForMember(dest => dest.SkillId, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.Skill, opt => opt.Ignore());

        CreateMap<JAStatusEntryDto, JAStatusEntry>().ReverseMap();
        CreateMap<JobApplicationDto, JobApplication>().ReverseMap();
        CreateMap<JAEventDto, JAEvent>()
            .ForMember(dest => dest.JAStatusEntry, opt => opt.Ignore())
            .ReverseMap()
            .ForMember(dest => dest.JobApplicationId, opt => opt.MapFrom(src => src.JAStatusEntry.JobApplicationId));
        CreateMap<JobListingDto, JobListing>().ReverseMap();

        CreateMap<JobApplicationMinimal, JobApplicationMinimalDto>()
            .ForSourceMember(src => src.UserId,
                opt => opt.DoNotValidate());
    }
}