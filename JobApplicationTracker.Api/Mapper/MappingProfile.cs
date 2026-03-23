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
        
       
    }
}