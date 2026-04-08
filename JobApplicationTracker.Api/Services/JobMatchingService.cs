using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models.Jobs;

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
      string prompt = """
                      You are an expert technical recruiter and hiring analyst.

                      Your task is to evaluate how well a candidate matches a job description.

                      You must:
                      - Be strict and realistic (do NOT overestimate candidates)
                      - Base your evaluation ONLY on provided data
                      - Do NOT assume missing skills
                      - Prefer underestimation over overestimation when uncertain

                      You MUST return ONLY valid JSON.
                      No explanations outside JSON.

                      Evaluate the candidate against the job description.

                      Use the following 5-level scale EXACTLY:
                      1 = No Match
                      2 = Weak Fit
                      3 = Potential Fit
                      4 = Strong Fit
                      5 = Excellent Fit

                      Evaluation rules:
                      - Focus primarily on required skills and responsibilities
                      - Penalize missing critical skills heavily
                      - Consider transferable skills as partial matches
                      - Experience relevance matters more than years alone
                      - Be conservative in assigning level 4 and 5

                      Return JSON in this exact structure:

                      {
                        "overall_match": {
                          "level": number,
                          "label": string,
                          "reasoning": string
                        },
                        "section_scores": {
                          "skills": {
                            "level": number,
                            "label": string,
                            "matched": [string],
                            "missing": [string],
                            "partial": [string],
                            "reasoning": string
                          },
                          "experience": {
                            "level": number,
                            "label": string,
                            "years_required": number,
                            "years_candidate": number,
                            "relevant_experience": [string],
                            "reasoning": string
                          },
                          "education": {
                            "level": number,
                            "label": string,
                            "required": string,
                            "candidate": string,
                            "reasoning": string
                          },
                          "responsibilities": {
                            "level": number,
                            "label": string,
                            "aligned": [string],
                            "gaps": [string],
                            "reasoning": string
                          }
                        },
                        "strengths": [string],
                        "gaps": [string],
                        "recommendations": [string],
                        "confidence": number
                      }

                      Label mapping:
                      1 → "No Match"
                      2 → "Weak Fit"
                      3 → "Potential Fit"
                      4 → "Strong Fit"
                      5 → "Excellent Fit"

                      IMPORTANT:
                      - "confidence" must be between 0 and 1
                      - Keep reasoning concise (1–2 sentences per section)
                      - Do NOT invent experience or skills
                      - If information is missing, reflect that in confidence

                      ---

                      CANDIDATE CV:


                      """
                      +
                      userResumeString
                      +
                      """
                      ---

                      JOB DESCRIPTION:
                      """
                      +
                      jobListingString;
      var res = await _aiAgentService.MakeRequestAsync(prompt);
      return res;
    }
}