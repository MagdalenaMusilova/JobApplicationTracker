using UglyToad.PdfPig;

namespace JobApplicationTracker.Services;

public class PdfReader : IPdfReader
{
    public string ReadText(IFormFile pdfFile)
    {
        if (pdfFile == null || pdfFile.Length == 0)
            return string.Empty;

        using var stream = pdfFile.OpenReadStream();
    
        using var pdf = PdfDocument.Open(stream);
        var text = new StringWriter();

        foreach (var page in pdf.GetPages())
        {
            text.WriteLine(page.Text); 
        }

        return text.ToString();
    }
}