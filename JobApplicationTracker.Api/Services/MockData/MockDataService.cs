using JobApplicationTracker.DTOs;
using JobApplicationTracker.Enums;
using JobApplicationTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace JobApplicationTracker.Services;

public class MockDataService : IMockDataService
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly IResumeService _resumeService;
    private readonly IJAEventService _jaEventService;

    public MockDataService(IJobApplicationService jobApplicationService, IAuthService authService, UserManager<User> userManager, IResumeService resumeService, IJAEventService jaEventService)
    {
        _jobApplicationService = jobApplicationService;
        _authService = authService;
        _userManager = userManager;
        _resumeService = resumeService;
        _jaEventService = jaEventService;
    }

    public async Task FillAccountWithMockDataAsync(string userId)
    {
        var existingResume = await _resumeService.GetByUserAsync(userId);
        var mockResume = GetMockResume(userId);
        if (existingResume == null)
        {
            await _resumeService.CreateAsync(mockResume);
        }
        else
        {
            await _resumeService.UpdateAsync(existingResume.Id, mockResume);
        }

        var mockApplications = new List<(CreateJobApplicationDto App, List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)> History)>
        {
            (
                new CreateJobApplicationDto
                {
                    Company = "Google",
                    Position = "Senior Software Engineer",
                    Note = "Applied via referral.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "First step." },
                    JobDescription = "Design and implement scalable distributed systems."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Recruiter screen passed. Technical round scheduled." },
                        new CreateJAEventDto
                        {
                            EventName = "Technical Interview",
                            EventType = (int)JAEventType.Interview,
                            EventDate = DateTime.UtcNow.AddDays(3).ToString("o"),
                            IsWholeDay = false,
                            Note = "Zoom link in calendar."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Microsoft",
                    Position = "Full Stack Developer",
                    Note = "Interesting project using .NET 8.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Wishlist, Note = "Considering applying." },
                    JobDescription = "Develop web applications using C#, React, and Azure."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>()
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Amazon",
                    Position = "Backend Engineer",
                    Note = "Focus on AWS services.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied on company site." },
                    JobDescription = "Optimize cloud infrastructure and microservices."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "System design interview." }, null),
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Offer, Note = "Got the offer!" },
                        new CreateJAEventDto
                        {
                            EventName = "Offer Call",
                            EventType = (int)JAEventType.Call,
                            EventDate = DateTime.UtcNow.AddDays(-1).ToString("o"),
                            IsWholeDay = false,
                            Note = "Discussing compensation."
                        }
                    ),
                    (new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Accepted, Note = "Signed the contract." }, null)
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Netflix",
                    Position = "Infrastructure Engineer",
                    Note = "High scale streaming platform.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Cold application." },
                    JobDescription = "Manage global content delivery network."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Rejected, Note = "Not a fit at this time." }, null)
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Meta",
                    Position = "Product Engineer",
                    Note = "Working on React.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied through LinkedIn." },
                    JobDescription = "Build user-facing features for social media platforms."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Behavioral interview." }, null),
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Task, Note = "Take-home coding challenge." },
                        new CreateJAEventDto
                        {
                            EventName = "Coding Task Deadline",
                            EventType = (int)JAEventType.Task,
                            EventDate = DateTime.UtcNow.AddDays(5).ToString("o"),
                            IsWholeDay = true,
                            Note = "React & TypeScript project."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "OpenAI",
                    Position = "AI Research Engineer",
                    Note = "Applying for the GPT-5 team.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "First step." },
                    JobDescription = "Research and develop next-generation large language models."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Task, Note = "Take-home assignment received." }, null)
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Apple",
                    Position = "iOS Developer",
                    Note = "Working on Swift and native apps.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied on careers page." },
                    JobDescription = "Develop features for Apple's flagship iOS applications."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Phone screen scheduled." },
                        new CreateJAEventDto
                        {
                            EventName = "Phone Screen",
                            EventType = (int)JAEventType.Call,
                            EventDate = DateTime.UtcNow.AddDays(2).ToString("o"),
                            IsWholeDay = false,
                            Note = "30-minute recruiter call."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Stripe",
                    Position = "Payment Systems Engineer",
                    Note = "Fintech opportunity.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Referred by colleague." },
                    JobDescription = "Build and scale payment processing infrastructure."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Initial interview completed." },
                        new CreateJAEventDto
                        {
                            EventName = "Onsite Interview",
                            EventType = (int)JAEventType.Interview,
                            EventDate = DateTime.UtcNow.AddDays(7).ToString("o"),
                            IsWholeDay = true,
                            Note = "Full day onsite - bring ID."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Shopify",
                    Position = "E-commerce Platform Developer",
                    Note = "Remote position.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Wishlist, Note = "Researching company culture." },
                    JobDescription = "Build and maintain Shopify's core e-commerce platform using Ruby and React."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>()
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Spotify",
                    Position = "Backend Developer",
                    Note = "Music streaming platform.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied via website." },
                    JobDescription = "Work on recommendation algorithms and backend services."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Passed recruiter screen." },
                        new CreateJAEventDto
                        {
                            EventName = "Coding Challenge",
                            EventType = (int)JAEventType.Task,
                            EventDate = DateTime.UtcNow.AddDays(-2).ToString("o"),
                            IsWholeDay = false,
                            Note = "Completed online assessment."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Airbnb",
                    Position = "Frontend Engineer",
                    Note = "Hybrid work model.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Direct application." },
                    JobDescription = "Build intuitive user interfaces for booking and hosting experiences."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>()
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Uber",
                    Position = "Distributed Systems Engineer",
                    Note = "Challenging technical work.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied through job board." },
                    JobDescription = "Design high-throughput, low-latency distributed systems."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Rejected, Note = "Position filled internally." }, null)
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Tesla",
                    Position = "Embedded Systems Engineer",
                    Note = "Automotive software.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied for autopilot team." },
                    JobDescription = "Develop embedded software for vehicle control systems."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Task, Note = "Technical assessment assigned." },
                        new CreateJAEventDto
                        {
                            EventName = "System Design Task",
                            EventType = (int)JAEventType.Task,
                            EventDate = DateTime.UtcNow.AddDays(4).ToString("o"),
                            IsWholeDay = true,
                            Note = "Design a real-time vehicle control system."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Salesforce",
                    Position = "Cloud Solutions Engineer",
                    Note = "Enterprise software focus.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Applied, Note = "Applied via LinkedIn." },
                    JobDescription = "Build scalable CRM solutions on Salesforce platform."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>
                {
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Recruiter call completed." },
                        new CreateJAEventDto
                        {
                            EventName = "Technical Interview",
                            EventType = (int)JAEventType.Interview,
                            EventDate = DateTime.UtcNow.AddDays(-3).ToString("o"),
                            IsWholeDay = false,
                            Note = "Discussed past projects."
                        }
                    ),
                    (
                        new CreateJAStatusEntryDto { StatusType = (int)JAStatusType.Interview, Note = "Moving to final round." },
                        new CreateJAEventDto
                        {
                            EventName = "Final Interview",
                            EventType = (int)JAEventType.Interview,
                            EventDate = DateTime.UtcNow.AddDays(1).ToString("o"),
                            IsWholeDay = false,
                            Note = "Meet with hiring manager."
                        }
                    )
                }
            ),
            (
                new CreateJobApplicationDto
                {
                    Company = "Slack",
                    Position = "Software Engineer - Messaging",
                    Note = "Collaboration tools.",
                    InitialStatus = new CreateInitJAStatusDto { StatusType = (int)JAStatusType.Wishlist, Note = "Interested but waiting for right time." },
                    JobDescription = "Develop real-time messaging and collaboration features."
                },
                new List<(CreateJAStatusEntryDto Status, CreateJAEventDto? Event)>()
            )
        };

        foreach (var mock in mockApplications)
        {
            var created = await _jobApplicationService.AddAsync(userId, mock.App);
            foreach (var historyItem in mock.History)
            {
                historyItem.Status.JobApplicationId = created.Id;
                var statusEntry = await _jobApplicationService.PushApplicationStatusAsync(historyItem.Status);

                if (historyItem.Event != null)
                {
                    historyItem.Event.JAStatusEntryId = statusEntry.StatusHistory.Last().Id;
                    await _jaEventService.AddAsync(historyItem.Event);
                }
            }
        }
    }

    private UserResumeDto GetMockResume(string userId)
    {
        var csharpSkill = new JobSkillDto { Name = "C#", Notes = "Advanced level - 6 years experience" };
        var reactSkill = new JobSkillDto { Name = "React", Notes = "Intermediate level - 4 years experience" };
        var dotNetSkill = new JobSkillDto { Name = ".NET", Notes = "Expert level - 7 years experience" };
        var sqlSkill = new JobSkillDto { Name = "SQL", Notes = "Proficient - 6 years experience" };
        var typescriptSkill = new JobSkillDto { Name = "TypeScript", Notes = "Advanced level - 4 years experience" };
        var azureSkill = new JobSkillDto { Name = "Azure", Notes = "Intermediate level - 3 years experience" };
        var dockerSkill = new JobSkillDto { Name = "Docker", Notes = "Proficient - 4 years experience" };
        var gitSkill = new JobSkillDto { Name = "Git", Notes = "Expert level - 7 years experience" };
        var nodeSkill = new JobSkillDto { Name = "Node.js", Notes = "Intermediate level - 3 years experience" };
        var pythonSkill = new JobSkillDto { Name = "Python", Notes = "Intermediate level - 2 years experience" };

        return new UserResumeDto
        {
            UserId = userId,
            Skills = new List<JobSkillDto>
            {
                csharpSkill, reactSkill, dotNetSkill, sqlSkill, typescriptSkill,
                azureSkill, dockerSkill, gitSkill, nodeSkill, pythonSkill
            },
            WorkExperiences = new List<WorkExperienceDto>
            {
                new WorkExperienceDto
                {
                    Company = "Tech Solutions Inc.",
                    Position = "Senior Software Engineer",
                    StartDate = new DateOnly(2020, 1, 1),
                    EndDate = null,
                    JobDescription = new List<string>
                    {
                        "Led a team of 5 developers in building a cloud-based SaaS platform serving 50,000+ users.",
                        "Architected and implemented microservices architecture using .NET Core and Docker.",
                        "Optimized database queries and indexing strategies, reducing API latency by 40%.",
                        "Implemented CI/CD pipelines using GitHub Actions, reducing deployment time by 60%.",
                        "Mentored junior developers and conducted code reviews to maintain code quality.",
                        "Collaborated with product managers to define technical requirements and roadmap."
                    },
                    Skills = new List<SkillUsageDto>
                    {
                        new SkillUsageDto { Skill = csharpSkill, Description = "Primary language for backend microservices and APIs." },
                        new SkillUsageDto { Skill = dotNetSkill, Description = "Core framework for all backend development and infrastructure." },
                        new SkillUsageDto { Skill = azureSkill, Description = "Deployed and managed services on Azure cloud platform." },
                        new SkillUsageDto { Skill = dockerSkill, Description = "Containerized all microservices for consistent deployments." },
                        new SkillUsageDto { Skill = sqlSkill, Description = "Designed database schemas and optimized complex queries." }
                    }
                },
                new WorkExperienceDto
                {
                    Company = "Web Apps Co.",
                    Position = "Full Stack Developer",
                    StartDate = new DateOnly(2017, 6, 1),
                    EndDate = new DateOnly(2019, 12, 31),
                    JobDescription = new List<string>
                    {
                        "Developed responsive frontend components using React and TypeScript.",
                        "Built and maintained RESTful APIs using Node.js and Express.",
                        "Integrated third-party payment gateways (Stripe, PayPal) into e-commerce platform.",
                        "Implemented real-time features using WebSockets and SignalR.",
                        "Collaborated with UX designers to improve user experience and accessibility.",
                        "Wrote comprehensive unit and integration tests achieving 85% code coverage."
                    },
                    Skills = new List<SkillUsageDto>
                    {
                        new SkillUsageDto { Skill = reactSkill, Description = "Built reusable UI components and state management with Redux." },
                        new SkillUsageDto { Skill = typescriptSkill, Description = "Ensured type safety across frontend and backend codebases." },
                        new SkillUsageDto { Skill = nodeSkill, Description = "Developed backend APIs and microservices." },
                        new SkillUsageDto { Skill = sqlSkill, Description = "Designed and maintained PostgreSQL database schemas." }
                    }
                },
                new WorkExperienceDto
                {
                    Company = "StartupHub",
                    Position = "Junior Developer",
                    StartDate = new DateOnly(2015, 3, 1),
                    EndDate = new DateOnly(2017, 5, 31),
                    JobDescription = new List<string>
                    {
                        "Developed features for internal management tools using C# and ASP.NET MVC.",
                        "Created data visualization dashboards using JavaScript libraries.",
                        "Participated in agile development process with daily standups and sprint planning.",
                        "Fixed bugs and performed maintenance on legacy codebases.",
                        "Assisted in database migrations and schema updates."
                    },
                    Skills = new List<SkillUsageDto>
                    {
                        new SkillUsageDto { Skill = csharpSkill, Description = "Developed web applications using ASP.NET MVC framework." },
                        new SkillUsageDto { Skill = dotNetSkill, Description = "Built enterprise applications on .NET Framework 4.5." },
                        new SkillUsageDto { Skill = sqlSkill, Description = "Wrote SQL queries and stored procedures for MSSQL Server." }
                    }
                }
            },
            Education = new List<EducationDto>
            {
                new EducationDto
                {
                    School = "State University of Technology",
                    Degree = "Bachelor of Science in Computer Science",
                    IsFinished = true,
                    Majors = new List<string> { "Software Engineering", "Data Structures & Algorithms", "Database Systems" },
                    Notes = "Graduated with honors. GPA: 3.8/4.0. Dean's List all semesters."
                },
                new EducationDto
                {
                    School = "Online Learning Platform",
                    Degree = "Professional Certificate in Cloud Architecture",
                    IsFinished = true,
                    Majors = new List<string> { "Azure Cloud Services", "Microservices Architecture" },
                    Notes = "Completed advanced cloud architecture certification program."
                }
            }
        };
    }
}
