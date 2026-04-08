namespace JobApplicationTracker.DTOs;

public class MatchOutputDto
{
    public OverallMatchDto OverallMatch { get; set; }
    public MatchSectionScoresDto SectionScores { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> Gaps { get; set; }
    public List<string> Recommendations { get; set; }
    public double Confidence { get; set; }
}


public class OverallMatchDto
{
    public int Level { get; set; }
    public string Label { get; set; }
    public string Reasoning { get; set; }
}

public class MatchSectionScoresDto
{
    public MatchSkillsDto Skills { get; set; }
    public MatchExperienceDto Experience { get; set; }
    public MatchEducationDto Education { get; set; }
    public MatchResponsibilitiesDto Responsibilities { get; set; }
}

public class MatchSkillsDto
{
    public int Level { get; set; }
    public string Label { get; set; }
    public List<string> Matched { get; set; }
    public List<string> Missing { get; set; }
    public List<string> Partial { get; set; }
    public string Reasoning { get; set; }
}

public class MatchExperienceDto
{
    public int Level { get; set; }
    public string Label { get; set; }
    public int YearsRequired { get; set; }
    public int YearsCandidate { get; set; }
    public List<string> RelevantExperience { get; set; }
    public string Reasoning { get; set; }
}

public class MatchEducationDto
{
    public int Level { get; set; }
    public string Label { get; set; }
    public string Required { get; set; }
    public string Candidate { get; set; }
    public string Reasoning { get; set; }
}

public class MatchResponsibilitiesDto
{
    public int Level { get; set; }
    public string Label { get; set; }
    public List<string> Aligned { get; set; }
    public List<string> Gaps { get; set; }
    public string Reasoning { get; set; }
}