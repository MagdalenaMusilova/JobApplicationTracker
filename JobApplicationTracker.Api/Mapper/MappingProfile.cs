using AutoMapper;
using JobApplicationTracker.Dos;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Mapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, UserDo>().ReverseMap();
        CreateMap<UserDo, User>().ReverseMap();
        
        CreateMap<JAStatusEntryDto, JAStatusEntryDo>().ReverseMap();
        CreateMap<JAStatusEntryDo, JAStatusEntry>().ReverseMap();
        
        CreateMap<JobApplicationDto, JobApplicationDo>()
            .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.StatusHistory))
            .ReverseMap()
            .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.StatusHistory));
        CreateMap<JobApplicationDo, JobApplication>()
            .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.StatusHistory))
            .ReverseMap()
            .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => src.StatusHistory));
        
        CreateMap<JAEventDto, JAEventDo>().ReverseMap();
        CreateMap<JAEventDo, JAEvent>().ReverseMap();
    }
}