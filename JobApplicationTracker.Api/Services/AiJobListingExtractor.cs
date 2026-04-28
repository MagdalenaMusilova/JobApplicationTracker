using System.Text.Json;
using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public class AiJobListingExtractor : IJobListingExtractor
{
    private readonly IAiAgentService _aiAgentService;

    public AiJobListingExtractor(IAiAgentService aiAgentService)
    {
        _aiAgentService = aiAgentService;
    }
    
    public async Task<JobListingDto?> ExtractFromPlaintextAsync(string text)
    {
        string prompt = """
          You are extracting structured information from a job listing written in plain text.
          
          Return ONLY valid JSON, with no markdown, no commentary, and no surrounding code fences.
          
          Use this exact schema:
          {
            "jobDescription": "string",
            "hardSkills": ["string"],
            "softSkills": ["string"],
            "requiredWorkExperience": ["string"],
            "requiredEducation": ["string"],
            "requiredCertification": ["string"],
            "otherRequirements": ["string"],
            "thisFitsThisKindOfPerson": "string"
          }
          
          Rules:
          - jobDescription must contain the full job description rewritten or cleaned up from the input text, preserving meaning.
          - hardSkills must include technical skills, tools, frameworks, programming languages, platforms, and domain-specific technologies explicitly mentioned or clearly required.
          - softSkills must include interpersonal or behavioral traits required by the listing, such as communication, teamwork, leadership, problem-solving, ownership, adaptability, etc.
          - requiredWorkExperience must include explicit experience requirements, such as years of experience, types of roles, or specific past responsibilities.
          - requiredEducation must include required or preferred degrees, fields of study, or education levels.
          - requiredCertification must include required or preferred certifications, licenses, or professional credentials.
          - otherRequirements must include anything important that does not fit the other categories, such as location, work authorization, remote/hybrid/on-site rules, shift requirements, travel requirements, background checks, portfolio, clearance, language requirements, salary-related requirements if explicitly stated, or availability requirements.
          - thisFitsThisKindOfPerson must be a short summary of the ideal candidate profile inferred from the listing, for example: "A self-driven backend developer with strong communication skills and experience building scalable web services."
          - Do not invent requirements that are not supported by the text.
          - If a category is not present, return an empty array for lists or an empty string for strings.
          - Deduplicate similar items.
          - Keep each array item short and concise.
          - Convert bullet lists into arrays.
          - If the listing contains both required and preferred items, include both but make it clear in the wording when something is preferred.
          - Do not include company marketing text unless it helps describe the role requirements.
          
          Job listing text:
          """ + text;
        
        var resJson = await _aiAgentService.MakeRequestAsync(prompt);
        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        JobListingDto? result = JsonSerializer.Deserialize<JobListingDto>(resJson, options);
        return result;
    }
}