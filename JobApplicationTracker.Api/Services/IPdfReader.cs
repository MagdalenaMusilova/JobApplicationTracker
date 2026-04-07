namespace JobApplicationTracker.Services;

public interface IPdfReader
{
    public string ReadText(IFormFile pdfFile);
}