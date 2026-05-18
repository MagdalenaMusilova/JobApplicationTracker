using AutoMapper;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models.UserProfile;
using JobApplicationTracker.Repository;

namespace JobApplicationTracker.Services;

public class ResumeService : IResumeService
{
    private readonly IPdfReader _pdfReader;
    private readonly IResumeDataExtractor _resumeDataExtractor;
    private readonly IUserResumeRepository _userResumeRepository;
    private readonly IResumeMergeService _mergeService;
    private readonly IMapper _mapper;
    
    public ResumeService(IPdfReader pdfReader, IResumeDataExtractor resumeDataExtractor, IUserResumeRepository userResumeRepository, IResumeMergeService mergeService, IMapper mapper)    
    {
        _pdfReader = pdfReader;
        _resumeDataExtractor = resumeDataExtractor;
        _userResumeRepository = userResumeRepository;
        _mergeService = mergeService;
        _mapper = mapper;   
    }

    public async Task<UserResumeDto> CreateAsync(UserResumeDto resume)
    {
        var entity = _mapper.Map<UserResume>(resume);
        entity = await _userResumeRepository.CreateAsync(entity);
        return _mapper.Map<UserResumeDto>(entity);
    }

    public async Task<UserResumeDto> ExtractFromPdfAsync(IFormFile file)
    {
        string resumePlainText = _pdfReader.ReadText(file);
        if (string.IsNullOrEmpty(resumePlainText))
        {
            throw new InvalidOperationException("Failed to read text from PDF");
        }
        UserResumeDto? userResume = await _resumeDataExtractor.ExtractFromPlaintextAsync(resumePlainText);
        if (userResume == null)
        {
            throw new InvalidOperationException("Failed to extract resume data from plaintext");
        }
        return userResume;
    }

    public async Task<UserResumeDto?> GetByIdAsync(Guid id)
    {
        var resume = await _userResumeRepository.GetByIdAsync(id);
        return resume is null ? null : _mapper.Map<UserResumeDto>(resume);  
    }

    public async Task<UserResumeDto?> GetByUserAsync(string userId)
    {
        var resume = await _userResumeRepository.GetByUserAsync(userId);
        return resume is null ? null : _mapper.Map<UserResumeDto>(resume);  
    }

    public async Task<UserResumeDto?> UpdateAsync(Guid id, UserResumeDto updated)
    {
        var existing = await _userResumeRepository.GetByIdAsync(id);
        if (existing is null) return null;

        var updatedEntity = _mapper.Map<UserResume>(updated);

        UserResume resume = new UserResume()
        {
            Id = existing.Id,
            UserId = existing.UserId,
            WorkExperiences = updatedEntity.WorkExperiences.Count > 0 ? updatedEntity.WorkExperiences : existing.WorkExperiences,
            Education = updatedEntity.Education.Count > 0 ? updatedEntity.Education : existing.Education,
            Trainings = updatedEntity.Trainings.Count > 0 ? updatedEntity.Trainings : existing.Trainings,
            Skills = updatedEntity.Skills.Count > 0 ? updatedEntity.Skills : existing.Skills,
            UncategorizedExperiences = updatedEntity.UncategorizedExperiences.Count > 0 ? updatedEntity.UncategorizedExperiences : existing.UncategorizedExperiences,
        };
        
        var result = await _userResumeRepository.UpdateAsync(resume);
        return result is null ? null : _mapper.Map<UserResumeDto>(result);
    }

    public async Task<UserResumeDto?> MergeAsync(Guid id, UserResumeDto newResume)
    {
        var existingResume = await _userResumeRepository.GetByIdAsync(id);
        if (existingResume is null)
        {
            return await CreateAsync(newResume);
        }
        else
        {
            UserResumeDto existingResumeDto = _mapper.Map<UserResumeDto>(existingResume);
            return await _mergeService.MergeAsync(existingResumeDto, newResume);
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _userResumeRepository.DeleteAsync(id);
        return true;
    }
}