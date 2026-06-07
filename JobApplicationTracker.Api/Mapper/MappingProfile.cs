using AutoMapper;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Mapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        
        CreateMap<JAStatusEntryDto, JAStatusEntry>().ReverseMap();
        CreateMap<JobApplicationDto, JobApplication>().ReverseMap();
        CreateMap<JAEventDto, JAEvent>().ReverseMap();
        CreateMap<JobListingDto, JobListing>().ReverseMap();
        
        CreateMap<JobApplicationMinimal, JobApplicationMinimalDto>()
            .ForSourceMember(src => src.UserId,
                opt => opt.DoNotValidate());
    }
}