using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationTracker.DTOs;

[NotMapped]
public class JobListingDto
{
    public string jobDescription { get; set; } = string.Empty;  //full description of the job
    public IEnumerable<string> hardSkills { get; set; } = new List<string>();
    public IEnumerable<string> softSkills { get; set; } = new List<string>();
    public IEnumerable<string> requiredWorkExperience { get; set; } = new List<string>();
    public IEnumerable<string> requiredEducation { get; set; } = new List<string>();
    public IEnumerable<string> requiredCertification { get; set; } = new List<string>();
    public IEnumerable<string> otherRequirements { get; set; } = new List<string>();
    public string? thisFitsThisKindOfPerson;
    
    public override string ToString()
    {
        static string JoinOrNone(IEnumerable<string>? items)
        {
            var list = items?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct()
                .ToList();

            return list is { Count: > 0 }
                ? string.Join(", ", list)
                : "none specified";
        }

        static string Truncate(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim();
            return text.Length <= maxLength ? text : text[..maxLength].TrimEnd() + "...";
        }

        var roleSummary = Truncate(jobDescription, 180);
        var candidateProfile = string.IsNullOrWhiteSpace(thisFitsThisKindOfPerson)
            ? "a suitable candidate"
            : thisFitsThisKindOfPerson.Trim();

        return
            $"Role summary: {roleSummary}. " +
            $"Ideal for: {candidateProfile}. " +
            $"Looking for someone with {JoinOrNone(hardSkills)} in technical skills, " +
            $"{JoinOrNone(softSkills)} in soft skills, " +
            $"experience in {JoinOrNone(requiredWorkExperience)}, " +
            $"education in {JoinOrNone(requiredEducation)}, " +
            $"certifications such as {JoinOrNone(requiredCertification)}, " +
            $"and other requirements including {JoinOrNone(otherRequirements)}.";
    }
}