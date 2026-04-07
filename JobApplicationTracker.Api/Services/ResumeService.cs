using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public class ResumeService : IResumeService
{
    private readonly IPdfReader _pdfReader;
    private readonly IResumeDataExtractor _resumeDataExtractor;
    
    public ResumeService(IPdfReader pdfReader, IResumeDataExtractor resumeDataExtractor)
    {
        _pdfReader = pdfReader;
        _resumeDataExtractor = resumeDataExtractor;
    }
    
    public async Task<UserResumeDto> ExtractFromPdf(IFormFile file)
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
}