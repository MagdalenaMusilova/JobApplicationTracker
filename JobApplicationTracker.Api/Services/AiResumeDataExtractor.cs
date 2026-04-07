using System.Text.Json;
using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public class AiResumeDataExtractor : IResumeDataExtractor
{
    private readonly IAiAgentService _aiAgentService;
    
    public AiResumeDataExtractor(IAiAgentService aiAgentService)
    {
        _aiAgentService = aiAgentService;
    }
  
    public async Task<UserResumeDto?> ExtractFromPlaintextAsync(string text)
    {
        string prompt = """
          You are extracting structured resume data from raw resume text.
          
          Return ONLY valid JSON, with no markdown, no commentary, and no surrounding code fences.
          
          Produce JSON that is directly convertible into a UserResumeDto-like structure.
          
          Use this exact schema:
          {
            "workExperiences": [
              {
                "id": null,
                "startDate": "YYYY-MM-DD or null",
                "endDate": "YYYY-MM-DD or null",
                "company": "string or null",
                "position": "string or null",
                "jobDescription": ["string"],
                "skills": [
                  {
                    "id": null,
                    "skill": {
                      "id": null,
                      "name": "string or null",
                      "aliases": ["string"],
                      "level": "string or null",
                      "weight": "string or null",
                      "notes": "string or null"
                    },
                    "description": "string or null"
                  }
                ],
                "notes": "string or null"
              }
            ],
            "education": [
              {
                "id": null,
                "degree": "string or null",
                "isFinished": true or false,
                "school": "string or null",
                "majors": ["string"],
                "skills": [
                  {
                    "id": null,
                    "skill": {
                      "id": null,
                      "name": "string or null",
                      "aliases": ["string"],
                      "level": "string or null",
                      "weight": "string or null",
                      "notes": "string or null"
                    },
                    "description": "string or null"
                  }
                ],
                "notes": "string or null"
              }
            ],
            "trainings": [
              {
                "id": null,
                "startDate": "YYYY-MM-DD or null",
                "endDate": "YYYY-MM-DD or null",
                "name": "string or null",
                "type": "string or null",
                "certification": ["string"],
                "skills": [
                  {
                    "id": null,
                    "skill": {
                      "id": null,
                      "name": "string or null",
                      "aliases": ["string"],
                      "level": "string or null",
                      "weight": "string or null",
                      "notes": "string or null"
                    },
                    "description": "string or null"
                  }
                ],
                "notes": "string or null"
              }
            ],
            "skills": [
              {
                "id": null,
                "name": "string or null",
                "aliases": ["string"],
                "level": "string or null",
                "weight": "string or null",
                "notes": "string or null",
                "skills": [
                  {
                    "id": null,
                    "skill": {
                      "id": null,
                      "name": "string or null",
                      "aliases": ["string"],
                      "level": "string or null",
                      "weight": "string or null",
                      "notes": "string or null"
                    },
                    "description": "string or null"
                  }
                ]
              }
            ],
            "uncategorizedSkillUsages": [
              {
                "id": null,
                "skill": {
                  "id": null,
                  "name": "string or null",
                  "aliases": ["string"],
                  "level": "string or null",
                  "weight": "string or null",
                  "notes": "string or null"
                },
                "description": "string or null"
              }
            ],
            "notes": "string or null"
          }
          
          Rules:
          - Use null for missing or uncertain values.
          - Do not invent facts.
          - Extract dates in ISO format YYYY-MM-DD when possible.
          - If only month/year is present, convert safely only if you are confident; otherwise use null.
          - For every skill mention, create a skill object and link it to the correct section.
          - IMPORTANT: infer skills from descriptions, notes, and bullets.
          - Example: if text says "built C# applications", create a skill with name "C#" and description like "Built C# applications".
          - Example: if text says "developed SQL reports", create a skill with name "SQL" and description like "Developed SQL reports".
          - Put general profile/about summary into notes.
          - Keep jobDescription as short bullet-like strings.
          - Keep certification as a list of strings.
          - Keep majors as a list of strings.
          - Return only valid JSON and nothing else.
          
          Resume text:
          """ + text;
        var resJson = await _aiAgentService.MakeRequestAsync(prompt);
        var options = new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        };

        UserResumeDto? result = JsonSerializer.Deserialize<UserResumeDto>(resJson, options);
        return result;
    }
}