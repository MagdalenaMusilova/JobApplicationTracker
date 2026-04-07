namespace JobApplicationTracker.DTOs;

public class PdfUploadRequestDto
{
    public IFormFile File { get; set; } = default!;
}