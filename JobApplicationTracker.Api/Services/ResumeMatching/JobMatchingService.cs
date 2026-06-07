using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public class JobMatchingService : IJobMatchingService
{
    private readonly IAiAgentService _aiAgentService;

    public JobMatchingService(IAiAgentService aiAgentService)
    {
        _aiAgentService = aiAgentService;
    }

    public async Task<string> EvaluateMatch(UserResumeDto resume, string jobListing)
    {
        string userResumeString = resume.ToString();
        string jobListingString = jobListing;
        string prompt = @"
You are an expert recruiting assistant evaluating how well a candidate matches a job description.

You will be given:
1) A candidate profile (plaintext)
2) A job description (plaintext)

Your task:
- Assess overall fit between the candidate and the job
- Identify strengths, gaps, and actionable improvements
- Be honest, specific, and recruiter-like
- Do NOT use percentages or numeric scores

OUTPUT FORMAT (strictly follow this structure):

## Verdict: <Strong Match | Good Match | Mixed Match | Limited Match>

**Key strengths**
- List all relevant strong matching areas. Use as many bullets as needed, but keep each bullet concise and meaningful.

**Main gaps**
- List all important missing or weak areas. Use as many bullets as needed, but focus only on meaningful gaps.

**Recommended focus**
- List actionable suggestions for improving the application. Use as many bullets as needed, but keep them practical and specific.

Then write exactly 3 paragraphs:

Paragraph 1:
Provide an overall assessment of fit. Be direct and recruiter-like. Do not repeat bullet points.

Paragraph 2:
Explain key strengths in more detail, adding context where helpful. Focus on why the candidate matches the role.

Paragraph 3:
Explain main gaps and their impact on hiring decision. Include clear, constructive guidance on how to improve alignment.

Rules:
- Keep language natural and professional (like a recruiter summary)
- Do not repeat job description text verbatim
- Do not include scores or percentages
- Do not add extra sections beyond the required format
- Prioritize clarity, usefulness, and specificity over length

CANDIDATE PROFILE:"
                        +
                        userResumeString
                        +
                        "JOB DESCRIPTION: "
                        +
                        jobListingString;


        var res = await _aiAgentService.MakeRequestAsync(prompt);
        return res;
    }
}