INSERT INTO Users
(Id, UserName, Email, AccessFailedCount, CreatedAt, EmailConfirmed, PhoneNumberConfirmed)
VALUES
(
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'TomTom',
'thomas.anderson@example.com',
0,
GETUTCDATE(),
1,
0
);


INSERT INTO Users
(Id, UserName, Email, AccessFailedCount, CreatedAt, EmailConfirmed, PhoneNumberConfirmed)
VALUES
(
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Porter',
'tyler.porter@example.com',
0,
GETUTCDATE(),
1,
0
);


INSERT INTO Users
(Id, UserName, Email, AccessFailedCount, CreatedAt, EmailConfirmed, PhoneNumberConfirmed)
VALUES
(
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'sandra.brown@example.com',
'sandra.brown@example.com',
0,
GETUTCDATE(),
1,
0
);


INSERT INTO Users
(Id, UserName, Email, AccessFailedCount, CreatedAt, EmailConfirmed, PhoneNumberConfirmed)
VALUES
(
'1e7dfe56-2282-4042-869d-84cd9720df55',
'eric.c',
'eric.cameron@example.com',
0,
GETUTCDATE(),
1,
0
);


INSERT INTO Users
(Id, UserName, Email, AccessFailedCount, CreatedAt, EmailConfirmed, PhoneNumberConfirmed)
VALUES
(
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'dv',
'david.smith@example.com',
0,
GETUTCDATE(),
1,
0
);


INSERT INTO ResumeEntries
(Id, UserId, AboutMe, UncategorizedInfo)
VALUES
(
'527aa291-c523-4038-9746-54a5cb1cfa14',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Backend-focused engineer with experience building scalable APIs and cloud-native applications.',
'Actively applying for remote and hybrid software engineering opportunities.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'1a9f6282-5224-413d-baf5-9909ac028883',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'C#',
'Built REST APIs and background workers using ASP.NET Core.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'ebe3ab2c-30c8-44ff-9438-bef106bf178d',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'.NET',
'Developed enterprise applications with EF Core and clean architecture.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'488391dc-c40c-4a73-adcc-e9b6b26e91ee',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'SQL Server',
'Designed relational schemas and optimized query performance.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'c86a662f-9c6b-4230-8494-cbc3c48e0c27',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'Azure',
'Used App Services, Azure SQL, and Blob Storage in production.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'4890d8ca-2930-4965-a616-f8fe1f894e20',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'Docker',
'Containerized applications for local development and deployment.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'8c751c5e-d465-4eb4-8c64-fed51772d6b9',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'2021-01-10',
'2023-05-15',
'TechNova Solutions',
'Software Engineer',
'Worked on backend APIs, authentication flows, and CI/CD deployment pipelines.',
'Collaborated closely with frontend and DevOps teams.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'e6a4c63b-c2f7-47e9-a4c9-c7df7aa1f43f',
'527aa291-c523-4038-9746-54a5cb1cfa14',
'2023-06-01',
NULL,
'CloudPeak Systems',
'Senior Software Engineer',
'Designed scalable services, optimized database performance, and improved monitoring coverage.',
'Mentored junior developers and reviewed architecture proposals.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'd4e5ba86-d689-4ec8-9d94-c7242dc98e6e',
'8c751c5e-d465-4eb4-8c64-fed51772d6b9',
'1a9f6282-5224-413d-baf5-9909ac028883',
'Used C# regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'fcf957d1-b0df-47d2-8858-1e2222ab7966',
'8c751c5e-d465-4eb4-8c64-fed51772d6b9',
'ebe3ab2c-30c8-44ff-9438-bef106bf178d',
'Used .NET regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'3dfe2d4d-8128-4b03-96d8-7a1077693256',
'8c751c5e-d465-4eb4-8c64-fed51772d6b9',
'488391dc-c40c-4a73-adcc-e9b6b26e91ee',
'Used SQL Server regularly while developing and maintaining production systems.'
);


INSERT INTO ResumeEntries
(Id, UserId, AboutMe, UncategorizedInfo)
VALUES
(
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Full stack developer experienced in modern frontend frameworks and distributed systems.',
'Actively applying for remote and hybrid software engineering opportunities.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'f755040c-8ad4-4be9-95f7-2909818fb0e8',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'React',
'Built dashboard interfaces with reusable component architecture.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'bb796f8e-5247-4523-82b8-7cdeef793eeb',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'TypeScript',
'Used strict typing for maintainable frontend applications.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'145362f5-0015-424b-97bc-bb4142b54a0a',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'Node.js',
'Created internal APIs and automation tooling.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'b3c606a0-9829-4af5-ab7a-1f84f7bd8f57',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'PostgreSQL',
'Managed migrations and relational data models.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'7d58a302-af4a-47d8-8064-a5967f54a233',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'AWS',
'Worked with ECS, S3, and RDS services.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'238bf9bf-4d7e-4821-8578-771318a61bcc',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'2021-01-10',
'2023-05-15',
'TechNova Solutions',
'Software Engineer',
'Worked on backend APIs, authentication flows, and CI/CD deployment pipelines.',
'Collaborated closely with frontend and DevOps teams.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'86e6fb68-393d-43e7-8073-c76f8196691d',
'fb4ff99b-76b5-47b8-8cad-78a17468232f',
'2023-06-01',
NULL,
'CloudPeak Systems',
'Senior Software Engineer',
'Designed scalable services, optimized database performance, and improved monitoring coverage.',
'Mentored junior developers and reviewed architecture proposals.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'5bc8656f-59b5-4f6f-8879-ea547716317d',
'238bf9bf-4d7e-4821-8578-771318a61bcc',
'f755040c-8ad4-4be9-95f7-2909818fb0e8',
'Used React regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'47f13b2d-96e8-45c6-a623-9f1a74beca08',
'238bf9bf-4d7e-4821-8578-771318a61bcc',
'bb796f8e-5247-4523-82b8-7cdeef793eeb',
'Used TypeScript regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'8262598e-3ddb-4f0a-b6f0-809dfe0b4ac8',
'238bf9bf-4d7e-4821-8578-771318a61bcc',
'145362f5-0015-424b-97bc-bb4142b54a0a',
'Used Node.js regularly while developing and maintaining production systems.'
);


INSERT INTO ResumeEntries
(Id, UserId, AboutMe, UncategorizedInfo)
VALUES
(
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Software engineer focused on developer productivity, infrastructure automation, and platform reliability.',
'Actively applying for remote and hybrid software engineering opportunities.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'2d669548-d211-4bd0-86a0-403d3a2f4ce3',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'Python',
'Created automation scripts and backend services.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'83d624d5-3f54-483e-abf9-80a17d56d42d',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'FastAPI',
'Built async APIs with OpenAPI documentation.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'1daead15-f771-4a25-a5fd-e8a7cba7ef4b',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'Redis',
'Implemented caching and queue systems.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'3a1ec959-9ff6-4270-a3c3-55949d6930a4',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'Docker',
'Containerized applications for local development and deployment.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'4b2079c2-9f33-402e-aaac-714f2b899c32',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'CI/CD',
'Configured GitHub Actions deployment pipelines.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'e4a4a983-9075-4140-9cbf-de10b0d4f5f7',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'2021-01-10',
'2023-05-15',
'TechNova Solutions',
'Software Engineer',
'Worked on backend APIs, authentication flows, and CI/CD deployment pipelines.',
'Collaborated closely with frontend and DevOps teams.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'42b59994-fb88-4256-b141-f1c285ce2d0f',
'2d82321c-f84e-49c3-83b8-dac376ccadd5',
'2023-06-01',
NULL,
'CloudPeak Systems',
'Senior Software Engineer',
'Designed scalable services, optimized database performance, and improved monitoring coverage.',
'Mentored junior developers and reviewed architecture proposals.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'beab95c9-33c0-4fa8-93cd-49cd8434e22a',
'e4a4a983-9075-4140-9cbf-de10b0d4f5f7',
'2d669548-d211-4bd0-86a0-403d3a2f4ce3',
'Used Python regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'644aba6b-2a6a-417e-acc5-a78b61da0b87',
'e4a4a983-9075-4140-9cbf-de10b0d4f5f7',
'83d624d5-3f54-483e-abf9-80a17d56d42d',
'Used FastAPI regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'faf876f0-7ca5-4e64-91af-54e0495bf212',
'e4a4a983-9075-4140-9cbf-de10b0d4f5f7',
'1daead15-f771-4a25-a5fd-e8a7cba7ef4b',
'Used Redis regularly while developing and maintaining production systems.'
);


INSERT INTO ResumeEntries
(Id, UserId, AboutMe, UncategorizedInfo)
VALUES
(
'720a4604-d884-43f9-898c-659eb512d38d',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Cloud engineer with experience deploying secure services using containerized infrastructure.',
'Actively applying for remote and hybrid software engineering opportunities.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'9a97f1ae-4835-4d76-809c-4a29010e63d4',
'720a4604-d884-43f9-898c-659eb512d38d',
'Java',
'Developed backend microservices using Java 17.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'567daf4c-b502-48d6-bd65-98a254b05906',
'720a4604-d884-43f9-898c-659eb512d38d',
'Spring Boot',
'Built RESTful APIs with Spring Security.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'10134e73-19cd-4937-a9a8-03b35552a41a',
'720a4604-d884-43f9-898c-659eb512d38d',
'Kafka',
'Integrated event-driven messaging workflows.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'650a2350-65c5-454b-b512-06fda8b6cb36',
'720a4604-d884-43f9-898c-659eb512d38d',
'MySQL',
'Worked with transactional relational databases.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'99d0f723-1450-4e54-9f46-621c339d5bd3',
'720a4604-d884-43f9-898c-659eb512d38d',
'Kubernetes',
'Managed deployments and scaling policies.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'aaf267b0-93fc-4930-8cf6-cc9c15cc65f5',
'720a4604-d884-43f9-898c-659eb512d38d',
'2021-01-10',
'2023-05-15',
'TechNova Solutions',
'Software Engineer',
'Worked on backend APIs, authentication flows, and CI/CD deployment pipelines.',
'Collaborated closely with frontend and DevOps teams.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'3f653e54-469a-49d8-be2c-8fe91dda730a',
'720a4604-d884-43f9-898c-659eb512d38d',
'2023-06-01',
NULL,
'CloudPeak Systems',
'Senior Software Engineer',
'Designed scalable services, optimized database performance, and improved monitoring coverage.',
'Mentored junior developers and reviewed architecture proposals.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'5caeb2ae-077f-44ba-a5b8-56f30bd7feae',
'aaf267b0-93fc-4930-8cf6-cc9c15cc65f5',
'9a97f1ae-4835-4d76-809c-4a29010e63d4',
'Used Java regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'e6edaa61-c8d6-4b86-952c-178491a4c3dd',
'aaf267b0-93fc-4930-8cf6-cc9c15cc65f5',
'567daf4c-b502-48d6-bd65-98a254b05906',
'Used Spring Boot regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'3b4ef9b9-689f-4dcb-b33f-e4b8a0f02549',
'aaf267b0-93fc-4930-8cf6-cc9c15cc65f5',
'10134e73-19cd-4937-a9a8-03b35552a41a',
'Used Kafka regularly while developing and maintaining production systems.'
);


INSERT INTO ResumeEntries
(Id, UserId, AboutMe, UncategorizedInfo)
VALUES
(
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Backend-focused engineer with experience building scalable APIs and cloud-native applications.',
'Actively applying for remote and hybrid software engineering opportunities.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'fafde60c-0c87-45a1-b4c5-7a9b45874c11',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'C#',
'Built REST APIs and background workers using ASP.NET Core.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'6e6cf1ea-09e4-445e-9d78-c19d1377c605',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'.NET',
'Developed enterprise applications with EF Core and clean architecture.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'3543c566-63f7-4e18-8125-93703e84cb84',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'SQL Server',
'Designed relational schemas and optimized query performance.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'7875a812-612f-4f5b-9012-bf3de1be9314',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'Azure',
'Used App Services, Azure SQL, and Blob Storage in production.'
);


INSERT INTO ResumeSkill
(Id, UserResumeId, Name, Notes)
VALUES
(
'291733f8-91ab-4890-b3bb-0fbc222bc6fd',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'Docker',
'Containerized applications for local development and deployment.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'0ced52fb-6669-4e9f-9c8a-8080b87e2ed2',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'2021-01-10',
'2023-05-15',
'TechNova Solutions',
'Software Engineer',
'Worked on backend APIs, authentication flows, and CI/CD deployment pipelines.',
'Collaborated closely with frontend and DevOps teams.'
);


INSERT INTO WorkExperience
(Id, UserResumeId, StartDate, EndDate, Company, Position, JobDescription, Notes)
VALUES
(
'647f6b0e-f734-4275-b0ee-3ddbaf57ee11',
'0aefff7a-9a13-49a2-aa64-61e63625618f',
'2023-06-01',
NULL,
'CloudPeak Systems',
'Senior Software Engineer',
'Designed scalable services, optimized database performance, and improved monitoring coverage.',
'Mentored junior developers and reviewed architecture proposals.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'e9ce55dd-09f7-4139-b208-95f583b1623e',
'0ced52fb-6669-4e9f-9c8a-8080b87e2ed2',
'fafde60c-0c87-45a1-b4c5-7a9b45874c11',
'Used C# regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'6657b2ef-5d41-4755-88dd-6255f10d932e',
'0ced52fb-6669-4e9f-9c8a-8080b87e2ed2',
'6e6cf1ea-09e4-445e-9d78-c19d1377c605',
'Used .NET regularly while developing and maintaining production systems.'
);


INSERT INTO SkillUsage
(Id, ExperienceId, SkillId, Description)
VALUES
(
'2ce2c582-e34c-43bb-9a6d-99af05be2969',
'0ced52fb-6669-4e9f-9c8a-8080b87e2ed2',
'3543c566-63f7-4e18-8125-93703e84cb84',
'Used SQL Server regularly while developing and maintaining production systems.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'2bfe2a7c-c985-4a11-acc7-fd6a1a65261d',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Wise',
'Full Stack Engineer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'3f985ec2-98da-447b-b238-2bffaee5429a',
'2bfe2a7c-c985-4a11-acc7-fd6a1a65261d',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'd2a3890c-08d6-4011-b61e-c9ec540c7b62',
'2bfe2a7c-c985-4a11-acc7-fd6a1a65261d',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'961ed47e-1833-43ea-ba7a-7803a349830b',
'2bfe2a7c-c985-4a11-acc7-fd6a1a65261d',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'5f4c16e0-4bdc-4226-b38b-72bdc3ab7df1',
'961ed47e-1833-43ea-ba7a-7803a349830b',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'feef5746-5471-44dc-bfb0-d81b5a44c8c8',
'2bfe2a7c-c985-4a11-acc7-fd6a1a65261d',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1e8bbe2d-f799-4e17-8c9e-651c75b521e7',
'feef5746-5471-44dc-bfb0-d81b5a44c8c8',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'45712f4f-5bd9-49f6-91b1-c6190c2381bc',
'2bfe2a7c-c985-4a11-acc7-fd6a1a65261d',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'77793cfa-dc68-4d5d-bfd1-d89918f0791a',
'45712f4f-5bd9-49f6-91b1-c6190c2381bc',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'2a261c36-7ce3-4dda-8425-e80700fe882c',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Stripe',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'96a8868c-3153-4911-86f4-2f5a066b5a89',
'2a261c36-7ce3-4dda-8425-e80700fe882c',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'9ac26213-ea6a-477e-9ac2-e4f6dbb8242a',
'2a261c36-7ce3-4dda-8425-e80700fe882c',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'195b3fc1-3094-417d-aab0-80c463c3048a',
'2a261c36-7ce3-4dda-8425-e80700fe882c',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'df8fa315-901c-4f96-b5ab-9ad06261c0f8',
'195b3fc1-3094-417d-aab0-80c463c3048a',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'2b20459d-b386-4e6c-a7c8-0e939a96ab3c',
'2a261c36-7ce3-4dda-8425-e80700fe882c',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'ad1d03bb-389a-428c-a408-9a25031c9f40',
'2b20459d-b386-4e6c-a7c8-0e939a96ab3c',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4ae24a16-07d0-447c-b99d-230b71fac7b2',
'2a261c36-7ce3-4dda-8425-e80700fe882c',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'91601369-a78c-4046-8750-b1cc22024884',
'4ae24a16-07d0-447c-b99d-230b71fac7b2',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'475d711f-bb6e-4528-8973-a97321ed050b',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Wise',
'Backend Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'c1956fdb-ec81-460a-b1dd-f19e573f0df4',
'475d711f-bb6e-4528-8973-a97321ed050b',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'552d4603-6c03-4b1d-8045-15b9c1cff553',
'475d711f-bb6e-4528-8973-a97321ed050b',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'3b16ca49-6225-4cf4-966e-a5058d5d148c',
'475d711f-bb6e-4528-8973-a97321ed050b',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'0a56b315-99bd-48de-8b2f-4d0124ea1e4b',
'3b16ca49-6225-4cf4-966e-a5058d5d148c',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'722c0bf1-9561-498a-8f84-012dbdb48dce',
'475d711f-bb6e-4528-8973-a97321ed050b',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'0aa0368a-c51e-407f-a5cd-b7fd9f0ef8d1',
'722c0bf1-9561-498a-8f84-012dbdb48dce',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'2fc34462-fdfe-47a7-b784-a2f0bee45924',
'475d711f-bb6e-4528-8973-a97321ed050b',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'b6364b77-a642-454d-947a-d691e59ee72d',
'2fc34462-fdfe-47a7-b784-a2f0bee45924',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'8fc27ae5-e281-43eb-bc2d-f1aad9738d20',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Atlassian',
'Cloud Engineer',
'Position matches previous backend experience.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'91369a70-2e96-4491-9df5-863a0a95907d',
'8fc27ae5-e281-43eb-bc2d-f1aad9738d20',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'd7b25695-716e-49be-bb95-dabde5f8f506',
'8fc27ae5-e281-43eb-bc2d-f1aad9738d20',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6616a1e7-f444-46d4-b9e7-bde884e6c5c2',
'8fc27ae5-e281-43eb-bc2d-f1aad9738d20',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'7e62feb7-6b1d-4368-93c2-f0d9328d170e',
'6616a1e7-f444-46d4-b9e7-bde884e6c5c2',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4521d3f3-e4a5-46d6-b209-8da5ac06f739',
'8fc27ae5-e281-43eb-bc2d-f1aad9738d20',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'486a4897-eb80-4e65-8f59-4d7643731f07',
'4521d3f3-e4a5-46d6-b209-8da5ac06f739',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'1cc637a1-51bb-46db-8cfa-bc01ebcd088e',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Red Hat',
'Senior .NET Developer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'1b4b9d6b-5a6b-4685-a688-38fbb228bdd9',
'1cc637a1-51bb-46db-8cfa-bc01ebcd088e',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b883ab41-50e1-42b2-b2b5-9aecccc48905',
'1cc637a1-51bb-46db-8cfa-bc01ebcd088e',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'01bdafe7-061a-4593-89e2-733beb89265f',
'1cc637a1-51bb-46db-8cfa-bc01ebcd088e',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'0273f3d8-12cc-403b-b0d2-f08308468503',
'01bdafe7-061a-4593-89e2-733beb89265f',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'7c409d37-1a9d-48c8-8832-eeebb3579068',
'1cc637a1-51bb-46db-8cfa-bc01ebcd088e',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'7f62a303-345b-4044-a606-df9d26fada9b',
'7c409d37-1a9d-48c8-8832-eeebb3579068',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'fac5160c-8247-4e7f-8a71-01ba25c77a18',
'1cc637a1-51bb-46db-8cfa-bc01ebcd088e',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'20c46948-a342-4553-869b-68853cd2f3ef',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Datadog',
'Platform Engineer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'69aecba8-1665-4c7b-998e-0b1d17acfd2d',
'20c46948-a342-4553-869b-68853cd2f3ef',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ddde913d-c5d0-4c6c-b7cd-c8a9e4ebc7c3',
'20c46948-a342-4553-869b-68853cd2f3ef',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6f393976-2b48-4e8b-b08f-89aa646a484a',
'20c46948-a342-4553-869b-68853cd2f3ef',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'caaa6fb4-be96-4123-8934-97ede6057ce3',
'6f393976-2b48-4e8b-b08f-89aa646a484a',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4ead216d-edda-4d21-be39-7681909ba72a',
'20c46948-a342-4553-869b-68853cd2f3ef',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'000cc9ea-4c78-4fd1-8b26-2a14916b7a4c',
'4ead216d-edda-4d21-be39-7681909ba72a',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'7b1462b5-3bb4-4395-a580-8541d0a9931c',
'20c46948-a342-4553-869b-68853cd2f3ef',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'defa0a47-b7d4-4a28-9a2a-a4a61d0ee4ca',
'7b1462b5-3bb4-4395-a580-8541d0a9931c',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Microsoft',
'Backend Developer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'fa6fcdf8-24aa-4106-8401-a92341d292a2',
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4d953b78-59da-4dee-a546-9a50029f76ca',
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0e351d8c-1f19-4a17-80a2-3e7bc1436912',
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'0a9fc090-50bb-4cd0-9a1b-e72b09c2a2da',
'0e351d8c-1f19-4a17-80a2-3e7bc1436912',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'46a25175-9d8c-4f03-8aa2-8671153eebeb',
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'a052bf31-72e6-4544-bef1-c393cd773a02',
'46a25175-9d8c-4f03-8aa2-8671153eebeb',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'3104f2b7-6afd-4639-a18f-9dd37645fdaa',
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'c86b6b07-248f-4193-a714-bd4de8551fce',
'3104f2b7-6afd-4639-a18f-9dd37645fdaa',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'85e9afbf-1cd0-44c5-b510-3288c56532fe',
'27765ae7-8c0f-4352-b029-ce0f4ae33f37',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Wise',
'Platform Engineer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'e11bb400-f4b4-423d-a67a-19f20c89f668',
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f8bac83c-b700-4eb5-9b2a-74cfc32a70a5',
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'46642667-a06b-43aa-8752-dbba8316957f',
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'464e8930-5cba-4479-a58d-e55952090e67',
'46642667-a06b-43aa-8752-dbba8316957f',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'206a2d77-c940-4028-b477-9f1232495503',
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'7a293af0-cca2-462b-913b-75640078f682',
'206a2d77-c940-4028-b477-9f1232495503',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'77c02b90-0da4-479a-a845-e31455b0f75e',
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'09e826a6-52c5-4982-8fbd-c441ad05910b',
'77c02b90-0da4-479a-a845-e31455b0f75e',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6d601d0f-f644-48e2-bea7-ed09fa0b75e5',
'9cb9df8c-c64a-4595-9b96-af3d871b3f22',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'd581e918-f1b0-41c3-8990-bc903c254ce7',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Atlassian',
'Full Stack Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'4ba7457f-215e-4f8f-a086-90be39aede01',
'd581e918-f1b0-41c3-8990-bc903c254ce7',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0724f955-c970-48d6-9ab0-ed09a7d7acad',
'd581e918-f1b0-41c3-8990-bc903c254ce7',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'679c922c-91ee-41a9-a86b-67b3419d396a',
'd581e918-f1b0-41c3-8990-bc903c254ce7',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'67f52117-d01d-4a68-bd92-cc79aebc6b3e',
'679c922c-91ee-41a9-a86b-67b3419d396a',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ad4fc6e8-068e-448b-ab4f-7782bd767602',
'd581e918-f1b0-41c3-8990-bc903c254ce7',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'd9537828-9a24-4def-8073-a9109227864c',
'ad4fc6e8-068e-448b-ab4f-7782bd767602',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4a9f143c-3f18-4d38-ace3-7cab66153b0d',
'd581e918-f1b0-41c3-8990-bc903c254ce7',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'06fdb5be-e9e7-4bb9-8b21-983eddba4dcb',
'4a9f143c-3f18-4d38-ace3-7cab66153b0d',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'28128d02-f352-41fd-bea4-073b41b12f49',
'61209afd-850c-4100-b7a8-8d9a9d5e85fa',
'Datadog',
'Platform Engineer',
'Position matches previous backend experience.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'2efe8874-9b13-4af3-953d-93e47f7c610b',
'28128d02-f352-41fd-bea4-073b41b12f49',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f74cdf6f-11d6-4d9e-acf2-5329b3d80899',
'28128d02-f352-41fd-bea4-073b41b12f49',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'86369a13-e1fa-4a8c-9111-8794a1ed800a',
'28128d02-f352-41fd-bea4-073b41b12f49',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'7e2f9c3c-16ad-45f7-a0e9-3515ed78438a',
'86369a13-e1fa-4a8c-9111-8794a1ed800a',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0a82ad25-412d-462b-a34b-6360508cb558',
'28128d02-f352-41fd-bea4-073b41b12f49',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'8e7684cf-d8aa-45ab-a461-2bea10f396de',
'0a82ad25-412d-462b-a34b-6360508cb558',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'76b7f581-bcde-4518-852f-3e6ca7695883',
'28128d02-f352-41fd-bea4-073b41b12f49',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'f5eb6bb9-dd25-415a-af33-4272f1e5d12a',
'76b7f581-bcde-4518-852f-3e6ca7695883',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'16c49a32-5c75-45bd-b19c-2b45a64a3c90',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Datadog',
'Cloud Engineer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'98c27afc-5e17-4311-805c-f54a7d9af560',
'16c49a32-5c75-45bd-b19c-2b45a64a3c90',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'8b4d4812-6fcc-4437-a723-3142dd8970f2',
'16c49a32-5c75-45bd-b19c-2b45a64a3c90',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'fcc91226-8921-4714-ade2-69011c2ad995',
'16c49a32-5c75-45bd-b19c-2b45a64a3c90',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'42c7109e-686e-4276-bedc-c137f6095aa0',
'fcc91226-8921-4714-ade2-69011c2ad995',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'de5a90b9-9a0a-40c0-bbcc-d79e0de64d8a',
'16c49a32-5c75-45bd-b19c-2b45a64a3c90',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'd1d45a2c-941e-4d12-b7b6-4cf14d1bb906',
'de5a90b9-9a0a-40c0-bbcc-d79e0de64d8a',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0cd81e05-22d3-4bb1-93e5-ead7c879ec1e',
'16c49a32-5c75-45bd-b19c-2b45a64a3c90',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'8124180a-ad3b-4a32-8733-26d7af37a8b8',
'0cd81e05-22d3-4bb1-93e5-ead7c879ec1e',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'7fdbadca-59b0-444a-898a-8e1495ad787c',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Shopify',
'Senior .NET Developer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'cb05c3c4-27b9-4f61-ba51-a681c85f391f',
'7fdbadca-59b0-444a-898a-8e1495ad787c',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c000ed61-59e0-4263-8499-bc7507b4f7af',
'7fdbadca-59b0-444a-898a-8e1495ad787c',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ca0df933-afa4-482f-ac45-b9f3ab2831cb',
'7fdbadca-59b0-444a-898a-8e1495ad787c',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'eb1e2ae5-f1dc-4db5-a21f-67fcac95efe5',
'ca0df933-afa4-482f-ac45-b9f3ab2831cb',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'395e77ab-125f-40ce-a1d6-191baea3d660',
'7fdbadca-59b0-444a-898a-8e1495ad787c',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1e26ba5b-ab4a-40e1-a398-1b6a922892b2',
'395e77ab-125f-40ce-a1d6-191baea3d660',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4c726bc6-62bc-4a75-83c6-dfa6ea05ec08',
'7fdbadca-59b0-444a-898a-8e1495ad787c',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Cloudflare',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'3cceff9e-f918-43b2-ba38-00575c0b0210',
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'84cd262f-54ee-447e-b0ac-dd3cfa4f3291',
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'13ba30dd-e0ba-4389-9300-c032f8fb106f',
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'872def3a-081a-4da0-9934-85432e7c65d0',
'13ba30dd-e0ba-4389-9300-c032f8fb106f',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ac2b7900-3221-4653-adb0-77a1991610b1',
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'22eff9c2-f85e-4e58-a143-3eb691224936',
'ac2b7900-3221-4653-adb0-77a1991610b1',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'41f9ff3f-b0b9-4bdf-8c85-3f1091306123',
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'ca1c9451-ed44-4ff4-8adb-4bb618aa3891',
'41f9ff3f-b0b9-4bdf-8c85-3f1091306123',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ee8968ce-3467-47ba-9c88-654135fa7c0f',
'ac348cc2-cb1c-4d01-828e-431bc8489f41',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'ae689b47-9efc-47a8-861f-f7c143da09e7',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'GitLab',
'Platform Engineer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'61c43e01-4776-4b8f-b50c-f59f3d5ff0e2',
'ae689b47-9efc-47a8-861f-f7c143da09e7',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'1535fbc0-9ac0-46d5-89ab-106416bd0c1d',
'ae689b47-9efc-47a8-861f-f7c143da09e7',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'deb8d7de-d514-4990-89b9-d889b7259cd6',
'ae689b47-9efc-47a8-861f-f7c143da09e7',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'ee7fa9ff-0413-45f5-a085-96bd85d1b093',
'deb8d7de-d514-4990-89b9-d889b7259cd6',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b68c2159-e8f8-4e25-ae96-3e33e75d9930',
'ae689b47-9efc-47a8-861f-f7c143da09e7',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'465b1576-abfa-4564-b819-642b99e580a7',
'b68c2159-e8f8-4e25-ae96-3e33e75d9930',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'860e8f84-9e3c-4e4a-a8f6-42d2b4177732',
'ae689b47-9efc-47a8-861f-f7c143da09e7',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'2f474c55-679f-45c6-b5c0-b6b4f379c456',
'860e8f84-9e3c-4e4a-a8f6-42d2b4177732',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b3a8163e-12bc-4df8-8da0-9794c2d71921',
'ae689b47-9efc-47a8-861f-f7c143da09e7',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'3744cb3e-2008-48c3-95ff-0a6ce507cd98',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Cloudflare',
'Platform Engineer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'b5397bd7-002f-4ea8-a6f9-8f3f87149350',
'3744cb3e-2008-48c3-95ff-0a6ce507cd98',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'195aeb1b-4075-491a-9efe-7fc7ca3d5e8c',
'3744cb3e-2008-48c3-95ff-0a6ce507cd98',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4b2dad4e-1416-4ecc-912a-f452c4204503',
'3744cb3e-2008-48c3-95ff-0a6ce507cd98',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'72ba745c-888b-4a16-85b9-5bff2ea12d27',
'4b2dad4e-1416-4ecc-912a-f452c4204503',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'69f19708-f436-4eb0-b0ae-e6ea1449db12',
'3744cb3e-2008-48c3-95ff-0a6ce507cd98',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'47901f32-5e2f-496b-9c64-0132c3bd1cb9',
'69f19708-f436-4eb0-b0ae-e6ea1449db12',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'a112ba2b-193e-48ca-be18-0e1d60a28778',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Microsoft',
'Backend Developer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'759d3d4e-4948-4c13-a529-48929e8f0b37',
'a112ba2b-193e-48ca-be18-0e1d60a28778',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'8727a816-0d7b-4aae-89b9-1fa8e974787c',
'a112ba2b-193e-48ca-be18-0e1d60a28778',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'a9cb4ee5-911c-44e4-896e-d15a18ed6c27',
'a112ba2b-193e-48ca-be18-0e1d60a28778',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'f5742ed6-edd7-4d7b-8ea5-ac2fe219a018',
'a9cb4ee5-911c-44e4-896e-d15a18ed6c27',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ae46572b-4608-4193-b3d3-247d50bc3c7e',
'a112ba2b-193e-48ca-be18-0e1d60a28778',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'55a6c718-b6f6-43a7-8d05-3acb15a07380',
'ae46572b-4608-4193-b3d3-247d50bc3c7e',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'952a55d8-5d52-4e20-8a4a-159ff78ae6e2',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Datadog',
'Cloud Engineer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'cc20d843-5622-4391-bfb6-1982eae48480',
'952a55d8-5d52-4e20-8a4a-159ff78ae6e2',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0187e392-4b5a-4981-a014-eb443e4cf589',
'952a55d8-5d52-4e20-8a4a-159ff78ae6e2',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0aec3432-881f-4602-abc4-387db364588f',
'952a55d8-5d52-4e20-8a4a-159ff78ae6e2',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'a308007a-c04f-4e68-935d-a69cb4baa4e0',
'0aec3432-881f-4602-abc4-387db364588f',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'd6bc4572-b9db-406c-bd02-99b3d9e878d4',
'952a55d8-5d52-4e20-8a4a-159ff78ae6e2',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'a2293256-c0c0-43b8-8181-cd20b5467237',
'd6bc4572-b9db-406c-bd02-99b3d9e878d4',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'9e9b2ec0-3e38-4552-9a32-5937fc01c4ed',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Red Hat',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'788d8d7b-ffd9-4a82-874a-acce82b418ce',
'9e9b2ec0-3e38-4552-9a32-5937fc01c4ed',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'db05a4d5-0951-4807-98ed-7584aa492bef',
'9e9b2ec0-3e38-4552-9a32-5937fc01c4ed',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'67d221a5-0cae-48b2-b595-cf944f371e73',
'9e9b2ec0-3e38-4552-9a32-5937fc01c4ed',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'57d9ccda-660f-4d94-99ce-640af7481541',
'67d221a5-0cae-48b2-b595-cf944f371e73',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c5402ae0-c966-4a5a-8fb7-160146f99b11',
'9e9b2ec0-3e38-4552-9a32-5937fc01c4ed',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'82cc0a4e-3ea8-40ad-9ce6-32264dbc64eb',
'c5402ae0-c966-4a5a-8fb7-160146f99b11',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ef08e80f-533d-46a8-804b-d55b32ba5331',
'9e9b2ec0-3e38-4552-9a32-5937fc01c4ed',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1abd2fba-497b-47a5-975d-a0f7fdcd122f',
'ef08e80f-533d-46a8-804b-d55b32ba5331',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'de88368b-110d-408a-bc0d-154c830aaecf',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Microsoft',
'Backend Developer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'b82eb5e4-c03b-4681-ab70-dd235b513703',
'de88368b-110d-408a-bc0d-154c830aaecf',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'bdf0681f-23f1-48d7-860c-7308df557634',
'de88368b-110d-408a-bc0d-154c830aaecf',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'014bae69-5df5-4561-bffe-7294af59d2e2',
'de88368b-110d-408a-bc0d-154c830aaecf',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'a3337e3b-821d-40cb-be6e-681c1114f5a5',
'014bae69-5df5-4561-bffe-7294af59d2e2',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'10fc0f90-5ecb-43c4-930e-983d38e6fcd7',
'de88368b-110d-408a-bc0d-154c830aaecf',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'830d210a-b8de-4f8c-bcdc-474fba3b011f',
'10fc0f90-5ecb-43c4-930e-983d38e6fcd7',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ee9b357c-b4b6-4807-989f-d8292c73fb0d',
'de88368b-110d-408a-bc0d-154c830aaecf',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'6b5d1155-c163-4288-a888-48a5b30a0ebd',
'ee9b357c-b4b6-4807-989f-d8292c73fb0d',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'aa903cb0-ba0c-4c05-a822-76a5640c7fed',
'0cf8b4f9-eb5c-4af1-bf7b-581af8e9c4b2',
'Microsoft',
'Full Stack Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'f84b24b0-7ab4-4df0-9ecb-073a9d890481',
'aa903cb0-ba0c-4c05-a822-76a5640c7fed',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'e4ba54eb-f41e-4e8c-8c36-ad0cd7a521b3',
'aa903cb0-ba0c-4c05-a822-76a5640c7fed',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c0d9ff85-6dc2-4ced-a4d7-9be62c2c11ad',
'aa903cb0-ba0c-4c05-a822-76a5640c7fed',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'38e94296-0f52-4266-9ba0-a130c7569b68',
'c0d9ff85-6dc2-4ced-a4d7-9be62c2c11ad',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'2980813d-8aa4-4945-84de-ac1da57fbaf4',
'aa903cb0-ba0c-4c05-a822-76a5640c7fed',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'f2b374c2-5231-4f26-aad7-fc2695ecb986',
'2980813d-8aa4-4945-84de-ac1da57fbaf4',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'3e726808-9dc6-4d88-8fc8-d1290ad2bac4',
'aa903cb0-ba0c-4c05-a822-76a5640c7fed',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'9b3abf9e-b933-4c72-bf55-c51ddc0e4d48',
'3e726808-9dc6-4d88-8fc8-d1290ad2bac4',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'866f7467-3bda-4b6e-a332-1def441c0a28',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Shopify',
'Senior .NET Developer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'899a4001-6111-4d97-a242-f6fb8c2851d4',
'866f7467-3bda-4b6e-a332-1def441c0a28',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'260061a6-55e2-4c9d-b107-3c9ceed890db',
'866f7467-3bda-4b6e-a332-1def441c0a28',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'cdd55c25-77af-4abc-a28a-ac246efac694',
'866f7467-3bda-4b6e-a332-1def441c0a28',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'024a3bda-6beb-424c-89ba-aa10d40afad7',
'cdd55c25-77af-4abc-a28a-ac246efac694',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'82d62cd6-8d67-4205-9828-ea52586deef2',
'866f7467-3bda-4b6e-a332-1def441c0a28',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'c24440e7-4636-4cc3-89dc-e5fa7a5ece0d',
'82d62cd6-8d67-4205-9828-ea52586deef2',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'f129db6f-87d5-46cb-acc3-1c72cd94ca12',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Red Hat',
'Platform Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'5c31158d-ed8e-404e-a43c-438b8670de05',
'f129db6f-87d5-46cb-acc3-1c72cd94ca12',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0d20ed4b-0f88-4030-a868-f629d54e4410',
'f129db6f-87d5-46cb-acc3-1c72cd94ca12',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'35310362-9b40-4aa6-a829-5b6c4f220e43',
'f129db6f-87d5-46cb-acc3-1c72cd94ca12',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'e12f166c-51da-49c7-9fab-84e5faaf7946',
'35310362-9b40-4aa6-a829-5b6c4f220e43',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'02ef9889-0fe0-484a-a4e3-1ba95f46096a',
'f129db6f-87d5-46cb-acc3-1c72cd94ca12',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'35f24498-2934-403d-9d13-f9b4ddcf4715',
'02ef9889-0fe0-484a-a4e3-1ba95f46096a',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'32348abb-1476-4610-82fe-ddbc46509f76',
'f129db6f-87d5-46cb-acc3-1c72cd94ca12',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'db3ca522-6b8b-404d-8d21-58bb81950e62',
'32348abb-1476-4610-82fe-ddbc46509f76',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'eaae2ecc-863f-43c6-9da8-fab47cc21fc8',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Red Hat',
'Platform Engineer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'09b6c893-e46f-4b2b-b121-e3745e66327e',
'eaae2ecc-863f-43c6-9da8-fab47cc21fc8',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'9c702b6c-09ff-4b3b-8bdb-1233f2414a89',
'eaae2ecc-863f-43c6-9da8-fab47cc21fc8',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c26ad730-4bf1-499f-91a4-59ad407cf848',
'eaae2ecc-863f-43c6-9da8-fab47cc21fc8',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'4031c07f-4756-4945-abae-dba2a5c0c037',
'c26ad730-4bf1-499f-91a4-59ad407cf848',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'79a64a45-6a4f-404f-a6a3-a23b6531610d',
'eaae2ecc-863f-43c6-9da8-fab47cc21fc8',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'71544925-ae48-4e98-8e39-f1e2d132e27f',
'79a64a45-6a4f-404f-a6a3-a23b6531610d',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'e1b9655c-38e7-4387-ac7f-447ccd6878ca',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Microsoft',
'Cloud Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'3c0ebe20-a4ea-4760-9046-9fa3a9023cec',
'e1b9655c-38e7-4387-ac7f-447ccd6878ca',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'030419f5-5be6-43d4-ac3d-b3cd2ebf2c73',
'e1b9655c-38e7-4387-ac7f-447ccd6878ca',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'50c7ea7b-8aa1-4797-bd98-a830801fde92',
'e1b9655c-38e7-4387-ac7f-447ccd6878ca',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'beea0e4f-b743-4e6e-8d43-b670286b66e7',
'50c7ea7b-8aa1-4797-bd98-a830801fde92',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'891a3725-dfae-4371-840b-ad2ac9da7b1e',
'e1b9655c-38e7-4387-ac7f-447ccd6878ca',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1dadb3af-530a-4587-a043-6e0fc15006c9',
'891a3725-dfae-4371-840b-ad2ac9da7b1e',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'73ab1b46-0e9e-48ba-91a2-38825fc09a42',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Red Hat',
'Backend Developer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'3f85fc10-9059-4f06-aa93-7492ac7eee33',
'73ab1b46-0e9e-48ba-91a2-38825fc09a42',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'85ce426c-f1c5-49aa-8022-7e361b43a9a8',
'73ab1b46-0e9e-48ba-91a2-38825fc09a42',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6bd49ee2-7fae-4ad8-9b1d-fae2979d43ce',
'73ab1b46-0e9e-48ba-91a2-38825fc09a42',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1f77ece7-ac15-41d9-b2b5-fb4891c86e90',
'6bd49ee2-7fae-4ad8-9b1d-fae2979d43ce',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'8d22f891-93fd-4190-a66d-8dd7fa677594',
'73ab1b46-0e9e-48ba-91a2-38825fc09a42',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'0f146d2e-923f-40f7-84f6-62e009edd500',
'8d22f891-93fd-4190-a66d-8dd7fa677594',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f92a4ef6-9da5-4e92-bdb8-6d0667c76aee',
'73ab1b46-0e9e-48ba-91a2-38825fc09a42',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'931bb7cf-fe18-4b7b-b85d-196370ae2702',
'f92a4ef6-9da5-4e92-bdb8-6d0667c76aee',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'a309bab5-da00-4c63-9f39-f70366c16b14',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'GitLab',
'Software Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'959fca38-a8ef-4c40-aab7-34b133412af7',
'a309bab5-da00-4c63-9f39-f70366c16b14',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0900554d-0574-46a7-8717-8495dcab0e58',
'a309bab5-da00-4c63-9f39-f70366c16b14',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c5840c4d-b4bb-4113-8fb0-9727718fac71',
'a309bab5-da00-4c63-9f39-f70366c16b14',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'13ad2944-cb26-4f9b-a786-59dc1309be13',
'c5840c4d-b4bb-4113-8fb0-9727718fac71',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'8def2cda-8886-408b-b173-f54c754f8e76',
'a309bab5-da00-4c63-9f39-f70366c16b14',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'addbc87c-93e0-48c7-8244-8468d81915c6',
'8def2cda-8886-408b-b173-f54c754f8e76',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'3ef349a4-09cf-4c78-a477-ebfe8a06e63c',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Shopify',
'Software Engineer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'7cfc1cfc-86d8-432e-84c0-df7fa9a34c65',
'3ef349a4-09cf-4c78-a477-ebfe8a06e63c',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'81a16064-86d7-422f-b9c7-84e5a6b58708',
'3ef349a4-09cf-4c78-a477-ebfe8a06e63c',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'5021427c-b0dd-4cff-b25b-9d99c5612e57',
'3ef349a4-09cf-4c78-a477-ebfe8a06e63c',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'256ca53f-f6c0-485e-bab3-a50e0f33cbeb',
'5021427c-b0dd-4cff-b25b-9d99c5612e57',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'bd8771f0-b4cd-4536-9a9e-16e744f74f0d',
'3ef349a4-09cf-4c78-a477-ebfe8a06e63c',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'058f1b78-a7f7-4a9f-b74d-1e8e1e5a8209',
'bd8771f0-b4cd-4536-9a9e-16e744f74f0d',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'0606e83a-2d8a-481b-a4d1-f43c5be27d8a',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Wise',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'3740797b-aa8c-4537-964e-c8312b69dc11',
'0606e83a-2d8a-481b-a4d1-f43c5be27d8a',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'dfe102ee-ebcb-4892-a70f-ac1533901981',
'0606e83a-2d8a-481b-a4d1-f43c5be27d8a',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4538c1dd-87ff-4534-a36a-b03ec4e32243',
'0606e83a-2d8a-481b-a4d1-f43c5be27d8a',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'9cb317fc-2db6-408c-af2c-5a826ddccae1',
'4538c1dd-87ff-4534-a36a-b03ec4e32243',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'e53386f1-6cba-40ca-92cb-2c10b95c33d9',
'0606e83a-2d8a-481b-a4d1-f43c5be27d8a',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'9159ac58-cf56-47fd-8301-864ff9613bb5',
'e53386f1-6cba-40ca-92cb-2c10b95c33d9',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'a9887633-32c6-4ac5-9eec-fe632448f93e',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'GitLab',
'Software Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'6e8cd10f-1c80-4bce-91ff-37f627d75528',
'a9887633-32c6-4ac5-9eec-fe632448f93e',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6fd6d14b-ed97-4563-8c7b-6b91ae283a24',
'a9887633-32c6-4ac5-9eec-fe632448f93e',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c5a899c9-f72c-4bcf-9247-c55e33c4a411',
'a9887633-32c6-4ac5-9eec-fe632448f93e',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'7f3fda89-2be5-4c95-8907-09e0068e4987',
'c5a899c9-f72c-4bcf-9247-c55e33c4a411',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'abc97a0c-7f3a-4110-975a-a1414c7d6586',
'a9887633-32c6-4ac5-9eec-fe632448f93e',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1e6c1f7c-36b7-448e-800d-50ca40fa2f5d',
'abc97a0c-7f3a-4110-975a-a1414c7d6586',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'b51c42dc-c6ea-4a35-96ac-8c32862a3109',
'f7e8c5f2-5536-4959-8501-6336d60ecf5d',
'Stripe',
'Full Stack Engineer',
'Position matches previous backend experience.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'b578a4bb-a846-4344-8209-6f19b26eb946',
'b51c42dc-c6ea-4a35-96ac-8c32862a3109',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'64975daa-74c8-41ba-a778-a7163f60ead4',
'b51c42dc-c6ea-4a35-96ac-8c32862a3109',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'40677ab9-a3ab-4d1f-92ad-fd3a1e1c4267',
'b51c42dc-c6ea-4a35-96ac-8c32862a3109',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'2cfdcb3e-6e1c-4953-84ba-cb6ccc95f8aa',
'40677ab9-a3ab-4d1f-92ad-fd3a1e1c4267',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'76c65f34-afbe-40b4-a618-01eda1acdcd1',
'b51c42dc-c6ea-4a35-96ac-8c32862a3109',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'e5f71e49-a8e4-4ff0-a286-eca040f0d0d7',
'76c65f34-afbe-40b4-a618-01eda1acdcd1',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'bbfb2e88-2ffd-4680-ae3c-db5e75d37720',
'b51c42dc-c6ea-4a35-96ac-8c32862a3109',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'483fa484-7d64-4d39-8d50-3da356007b00',
'bbfb2e88-2ffd-4680-ae3c-db5e75d37720',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'8d5e137a-f017-41a3-9f82-99dd5b03a4bd',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Cloudflare',
'Cloud Engineer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'cce481cb-8858-4e90-a420-a86298c25365',
'8d5e137a-f017-41a3-9f82-99dd5b03a4bd',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'a90e64a6-3f6e-40e2-8167-194085832f28',
'8d5e137a-f017-41a3-9f82-99dd5b03a4bd',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6b7b72e3-6f74-4c37-b8dd-c6ae6dcec276',
'8d5e137a-f017-41a3-9f82-99dd5b03a4bd',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'8a85e901-4835-479d-ae02-b1ea19869057',
'6b7b72e3-6f74-4c37-b8dd-c6ae6dcec276',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'070f7b0c-4223-4325-9946-9a59dcc40b1e',
'8d5e137a-f017-41a3-9f82-99dd5b03a4bd',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'6fa15fbb-2af9-4ac1-a8d3-3f01f89a5838',
'070f7b0c-4223-4325-9946-9a59dcc40b1e',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'1ffb42eb-9399-4fe4-b7ad-d94892785b2d',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'GitLab',
'Full Stack Engineer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'98d00bbc-c249-4e7a-8eff-dfbd276e7bef',
'1ffb42eb-9399-4fe4-b7ad-d94892785b2d',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'065495f0-188e-459d-af8b-5b2c2c1ee7fb',
'1ffb42eb-9399-4fe4-b7ad-d94892785b2d',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'02558fe0-3de9-45cd-ae19-e13c7ef0524b',
'1ffb42eb-9399-4fe4-b7ad-d94892785b2d',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'1c58892f-4b23-4ca1-a67f-87d605540523',
'02558fe0-3de9-45cd-ae19-e13c7ef0524b',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'910754d0-bdd9-45f1-b45a-77706f9f41d4',
'1ffb42eb-9399-4fe4-b7ad-d94892785b2d',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'77f8d700-6406-4dd9-a398-9132cc0bdb1e',
'910754d0-bdd9-45f1-b45a-77706f9f41d4',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'80908243-48ad-44dd-a75f-cdfb1ccc8159',
'1ffb42eb-9399-4fe4-b7ad-d94892785b2d',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'eba5879f-27c3-452d-a1f9-275334c378f5',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Red Hat',
'Full Stack Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'e6c3ef5d-4033-45fc-ad7d-6adf840d06c2',
'eba5879f-27c3-452d-a1f9-275334c378f5',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f91bdb56-5293-4aa9-9949-c0479535c30d',
'eba5879f-27c3-452d-a1f9-275334c378f5',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0e12ef55-0db6-4a04-aa12-14b18a2b93c6',
'eba5879f-27c3-452d-a1f9-275334c378f5',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'313f317e-5dc7-4edf-b375-bdd81d172000',
'0e12ef55-0db6-4a04-aa12-14b18a2b93c6',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'a432237e-2f9e-4de3-9a08-07339202b8d6',
'eba5879f-27c3-452d-a1f9-275334c378f5',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'ee0737c1-c857-4cd4-906a-0baba6c939b4',
'a432237e-2f9e-4de3-9a08-07339202b8d6',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'295c9b0b-2fc6-4f2a-aaa3-4169abfb40d3',
'eba5879f-27c3-452d-a1f9-275334c378f5',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'09157fd5-3e69-4466-810c-6a730a35e63c',
'295c9b0b-2fc6-4f2a-aaa3-4169abfb40d3',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'a1780894-0495-4ba2-b2d0-c198da7ee23e',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Shopify',
'Cloud Engineer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'ab8ba9c4-9a02-40f5-baac-a6553f02ff05',
'a1780894-0495-4ba2-b2d0-c198da7ee23e',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f8e3106d-20a3-4e1c-887a-337aa818b023',
'a1780894-0495-4ba2-b2d0-c198da7ee23e',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'067c58de-40fb-4f0d-aa80-ba3e47a32145',
'a1780894-0495-4ba2-b2d0-c198da7ee23e',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'ae895fd2-09c1-4435-b72e-02f9512e6508',
'067c58de-40fb-4f0d-aa80-ba3e47a32145',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'063b5a12-6d40-49b0-8f9e-c77637b3e16d',
'a1780894-0495-4ba2-b2d0-c198da7ee23e',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'82550cce-88e6-4e2f-a77f-db80a33f105f',
'063b5a12-6d40-49b0-8f9e-c77637b3e16d',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'd273b35e-1254-4e26-9961-c9a22f98b7c2',
'a1780894-0495-4ba2-b2d0-c198da7ee23e',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'f4dd327e-d41c-4a07-8c28-c17489e05469',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Datadog',
'Backend Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'37bf5f98-560b-4f3d-9e19-c3d00966ed80',
'f4dd327e-d41c-4a07-8c28-c17489e05469',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'1c79966a-d254-47dd-9ae5-3759b865a474',
'f4dd327e-d41c-4a07-8c28-c17489e05469',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b91911ce-7445-4891-a01e-5a2fd0340e63',
'f4dd327e-d41c-4a07-8c28-c17489e05469',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'ff79cf77-607a-4d44-810f-e087342f7b31',
'b91911ce-7445-4891-a01e-5a2fd0340e63',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'dd2e821e-ea06-4f5f-9441-3bcd7ac2a90f',
'f4dd327e-d41c-4a07-8c28-c17489e05469',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'95ae49e8-b158-43ae-bfb9-0a4fb9830549',
'dd2e821e-ea06-4f5f-9441-3bcd7ac2a90f',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'fe196ec1-4f1a-4bfb-80e3-d66b817c8aec',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Microsoft',
'Full Stack Engineer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'67d94bdc-4216-47b5-8506-ebf5994db1ad',
'fe196ec1-4f1a-4bfb-80e3-d66b817c8aec',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'aab7019f-ffe8-4780-b89c-ab63197fd4ff',
'fe196ec1-4f1a-4bfb-80e3-d66b817c8aec',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'91056849-6465-4ebe-9287-bc6eb2463afc',
'fe196ec1-4f1a-4bfb-80e3-d66b817c8aec',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'9a00efb1-9fdc-4b85-b996-88441e778614',
'91056849-6465-4ebe-9287-bc6eb2463afc',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'83e03ac4-41ed-44cf-97b8-5ca58307a09f',
'fe196ec1-4f1a-4bfb-80e3-d66b817c8aec',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'bf980776-c660-4ada-b49f-65bd2f1911ca',
'83e03ac4-41ed-44cf-97b8-5ca58307a09f',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0f805e7b-0cc2-4fbc-947b-d34f24121ec4',
'fe196ec1-4f1a-4bfb-80e3-d66b817c8aec',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'03d1135f-5d1c-45de-bb55-cbb53e2f9934',
'0f805e7b-0cc2-4fbc-947b-d34f24121ec4',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'cfc866c3-2ca1-4eff-8203-6d29e70d6e0b',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Cloudflare',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'e99bb71e-406e-42ce-8b5e-8134c5fda45e',
'cfc866c3-2ca1-4eff-8203-6d29e70d6e0b',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f571460e-f751-4d80-b708-27b554e20643',
'cfc866c3-2ca1-4eff-8203-6d29e70d6e0b',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b84b4790-bc0a-4ab4-a4a4-b2a7c277ac6a',
'cfc866c3-2ca1-4eff-8203-6d29e70d6e0b',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'dcb97204-8382-458d-ae57-acf68129d837',
'b84b4790-bc0a-4ab4-a4a4-b2a7c277ac6a',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ad99bdd4-a5dd-43bb-a736-43f2231af5cc',
'cfc866c3-2ca1-4eff-8203-6d29e70d6e0b',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'98f6af94-1b0e-4b90-a997-91192c813b45',
'ad99bdd4-a5dd-43bb-a736-43f2231af5cc',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'7797cc80-ad9e-441d-81bf-2486b97a22ec',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Microsoft',
'Full Stack Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'40f6b557-e244-493f-aea9-efb9346e6b2e',
'7797cc80-ad9e-441d-81bf-2486b97a22ec',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'fe24cc63-acb7-4242-bc4b-693a9db594b3',
'7797cc80-ad9e-441d-81bf-2486b97a22ec',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'fe01da06-6bbc-452e-b6ea-c703df23a4d1',
'7797cc80-ad9e-441d-81bf-2486b97a22ec',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'cb0df50a-d787-41b2-a5af-797e50db0ef6',
'fe01da06-6bbc-452e-b6ea-c703df23a4d1',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'06091bbc-b721-4583-ac8c-39800b6f90e8',
'7797cc80-ad9e-441d-81bf-2486b97a22ec',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'9fca9bc0-f433-4937-b6ed-5b2418842b2d',
'06091bbc-b721-4583-ac8c-39800b6f90e8',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'7a311c29-e0f3-4552-8888-56826a15ee2a',
'7797cc80-ad9e-441d-81bf-2486b97a22ec',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'511eace9-0f0a-40cc-a187-1ef0a5b5a733',
'7a311c29-e0f3-4552-8888-56826a15ee2a',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'e847da89-890b-4abb-a9a6-bbbe3ebc1e9d',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'Stripe',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'2b4d0361-2a5e-4329-b59f-980f67810b46',
'e847da89-890b-4abb-a9a6-bbbe3ebc1e9d',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'6dff6f7f-1a91-44bf-b15f-b8d5e4d58a5a',
'e847da89-890b-4abb-a9a6-bbbe3ebc1e9d',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'24eb38b7-7cf7-42ec-9474-518f87b14e13',
'e847da89-890b-4abb-a9a6-bbbe3ebc1e9d',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'71e685d3-5dd3-4511-9fc2-f60f1a99eec4',
'24eb38b7-7cf7-42ec-9474-518f87b14e13',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'65aac159-87f5-4964-bc8a-e547964c5a4e',
'e847da89-890b-4abb-a9a6-bbbe3ebc1e9d',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'99e46fec-3b3c-48f2-bcc8-8394c2fec788',
'65aac159-87f5-4964-bc8a-e547964c5a4e',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b9a2f419-c3bd-4969-b754-2f2db560fdf9',
'e847da89-890b-4abb-a9a6-bbbe3ebc1e9d',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'38150299-940a-4197-9e41-bba57b41d678',
'b9a2f419-c3bd-4969-b754-2f2db560fdf9',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'cfb4cff9-e370-46b9-bd1d-279daca87fa4',
'1e7dfe56-2282-4042-869d-84cd9720df55',
'GitLab',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'407967f2-860a-40d3-aba6-4ea346decddd',
'cfb4cff9-e370-46b9-bd1d-279daca87fa4',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'067c1208-bc9e-42e4-8773-eb0d9cdda2b7',
'cfb4cff9-e370-46b9-bd1d-279daca87fa4',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'9372e172-00b7-4f6f-bb3e-01f8d7b627b0',
'cfb4cff9-e370-46b9-bd1d-279daca87fa4',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'2589f27b-aead-445d-97be-d4c35952fd17',
'9372e172-00b7-4f6f-bb3e-01f8d7b627b0',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'24df215a-2d0b-4af3-8d02-328fdd948781',
'cfb4cff9-e370-46b9-bd1d-279daca87fa4',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'06139356-a577-4718-8305-14647e59ef45',
'24df215a-2d0b-4af3-8d02-328fdd948781',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'b424d7e9-904a-492d-876a-9aa0cf014911',
'cfb4cff9-e370-46b9-bd1d-279daca87fa4',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'ca60298e-4197-4036-ae66-1a7afe8602f3',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Wise',
'Platform Engineer',
'Position matches previous backend experience.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'7263c45b-0985-488f-ab69-82b2d29cd25d',
'ca60298e-4197-4036-ae66-1a7afe8602f3',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'10b03867-e978-4b2f-a217-2e56425d559e',
'ca60298e-4197-4036-ae66-1a7afe8602f3',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ae881c29-aa4d-4007-b8c0-934ce20d1f0d',
'ca60298e-4197-4036-ae66-1a7afe8602f3',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'73d732f6-f0e6-42f0-9550-05944e502cfb',
'ae881c29-aa4d-4007-b8c0-934ce20d1f0d',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0330f473-7e43-4534-bcc5-d41062f7cf32',
'ca60298e-4197-4036-ae66-1a7afe8602f3',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'f06fe35b-c8c5-4697-b92f-8be19348fcc2',
'0330f473-7e43-4534-bcc5-d41062f7cf32',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'170e3934-0b24-4196-bc6a-24591aee1971',
'ca60298e-4197-4036-ae66-1a7afe8602f3',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'b58531c0-539b-4f45-b68f-1170fb6bbcb8',
'170e3934-0b24-4196-bc6a-24591aee1971',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'91a0f2ea-eb56-4697-b53b-1bfa0370ab81',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Atlassian',
'Senior .NET Developer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'7aa0f1ff-157c-472f-af66-513a1b93eb02',
'91a0f2ea-eb56-4697-b53b-1bfa0370ab81',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'e06afaea-59ff-4dc0-8dc5-373c8732927f',
'91a0f2ea-eb56-4697-b53b-1bfa0370ab81',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'be008e5c-c775-4803-b67e-dbb6393155f2',
'91a0f2ea-eb56-4697-b53b-1bfa0370ab81',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'669b3b4f-e27b-4246-a731-949bcbda020a',
'be008e5c-c775-4803-b67e-dbb6393155f2',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'313c778f-3b6a-42bd-a1b2-26d658fde011',
'91a0f2ea-eb56-4697-b53b-1bfa0370ab81',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'39f01876-cfe9-47eb-8544-076ec2dd142c',
'313c778f-3b6a-42bd-a1b2-26d658fde011',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'523f60c2-0bc6-49c6-a69f-fc94b28cf9a2',
'91a0f2ea-eb56-4697-b53b-1bfa0370ab81',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'd61cb87b-e4bd-4f03-999f-0deee9adb5aa',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Microsoft',
'Software Engineer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'0811277c-af78-4ad4-ba24-708be2add3bc',
'd61cb87b-e4bd-4f03-999f-0deee9adb5aa',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'fcedd796-bb41-4dd2-91e6-a5423d89a0df',
'd61cb87b-e4bd-4f03-999f-0deee9adb5aa',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'8260d980-2b90-42e3-9b2a-4eff025b16a0',
'd61cb87b-e4bd-4f03-999f-0deee9adb5aa',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'015ac3a6-3c1a-4feb-8649-38e9b3737b65',
'8260d980-2b90-42e3-9b2a-4eff025b16a0',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'784a5746-deab-4932-a557-d14ef5d7eab1',
'd61cb87b-e4bd-4f03-999f-0deee9adb5aa',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'7f24f00d-d1c6-4025-9cbe-a1a6f78006a1',
'784a5746-deab-4932-a557-d14ef5d7eab1',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ebc31a43-02d1-4dda-8786-962fcd1be111',
'd61cb87b-e4bd-4f03-999f-0deee9adb5aa',
4,
4,
'2026-05-02 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'a025f869-8824-4c63-8440-e082b73c6492',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Stripe',
'Senior .NET Developer',
'Position matches previous backend experience.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'36d12717-0c85-4a98-862e-8074a420e280',
'a025f869-8824-4c63-8440-e082b73c6492',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'fb0eecba-9223-4cdf-96cc-9e2354f339f8',
'a025f869-8824-4c63-8440-e082b73c6492',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'a0278f69-0481-4536-bbfc-7273a281fece',
'a025f869-8824-4c63-8440-e082b73c6492',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'57e39a89-f944-4765-b8e4-ee957f1fded3',
'a0278f69-0481-4536-bbfc-7273a281fece',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'1100aba7-9eba-4144-99f5-53e298c03f13',
'a025f869-8824-4c63-8440-e082b73c6492',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'494f6b11-1792-48e9-a48e-7c98d6a64f9f',
'1100aba7-9eba-4144-99f5-53e298c03f13',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'222f7e70-c924-4d20-9bf2-3c13482e2bee',
'a025f869-8824-4c63-8440-e082b73c6492',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'07ffa204-1923-4f9f-b82d-27e9898261f0',
'222f7e70-c924-4d20-9bf2-3c13482e2bee',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'5aba6e56-0523-426d-92d8-4a411e4f2142',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Red Hat',
'Platform Engineer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'b763bba3-be41-49ac-969b-d1de0ec7d070',
'5aba6e56-0523-426d-92d8-4a411e4f2142',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'e7c66235-5715-42dd-b4df-df5ad862b7f0',
'5aba6e56-0523-426d-92d8-4a411e4f2142',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'5ff67150-efa5-4b39-881c-bdf056178625',
'5aba6e56-0523-426d-92d8-4a411e4f2142',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'678359c3-2940-4c00-93af-a4d1c5d23e4c',
'5ff67150-efa5-4b39-881c-bdf056178625',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'e6f02526-d834-4d32-a503-9d04941f3576',
'5aba6e56-0523-426d-92d8-4a411e4f2142',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'416996c4-afa6-4d33-a6b1-e03ce60753e9',
'e6f02526-d834-4d32-a503-9d04941f3576',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Wise',
'Platform Engineer',
'Referral submitted by former colleague.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'08983e78-1894-4f88-b0c5-a9aa503323a4',
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'd684c966-514f-4cda-ac88-c3add05926b2',
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'4802f8f4-5011-4e39-812e-ae52390e7372',
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'2d1db0a4-8ad9-4d97-b7f7-261eeb7c610d',
'4802f8f4-5011-4e39-812e-ae52390e7372',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'69daebbd-1678-4a20-b2f0-f0b53f5e67e1',
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'4898b059-5b33-48c4-a23e-3129d4660d9d',
'69daebbd-1678-4a20-b2f0-f0b53f5e67e1',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'f0013b40-346e-4dab-8bec-3fa3a4d2cc34',
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'38887be3-3192-4053-967c-d75b3d184922',
'f0013b40-346e-4dab-8bec-3fa3a4d2cc34',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'42b9d0a5-a48b-4283-9fd6-bc7720f7cdd2',
'782efc7f-707f-46b7-9d3e-32a33960ba2f',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'65efdc22-1f3e-4ff7-8432-ec5a0564cd7b',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Shopify',
'Senior .NET Developer',
'Applied through LinkedIn Easy Apply.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'4a8c0279-6119-4191-aaee-20e371ef0860',
'65efdc22-1f3e-4ff7-8432-ec5a0564cd7b',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'37338289-2536-430c-8aae-51987bf2e8a7',
'65efdc22-1f3e-4ff7-8432-ec5a0564cd7b',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'0e896167-1dc6-44a3-9a0f-210195852575',
'65efdc22-1f3e-4ff7-8432-ec5a0564cd7b',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'8e2038b9-a309-44da-af3f-2d1a81c4b992',
'0e896167-1dc6-44a3-9a0f-210195852575',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'adb20bc5-4d39-4727-a1e6-eda9d97d18a7',
'65efdc22-1f3e-4ff7-8432-ec5a0564cd7b',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'b06373f0-a720-4675-a1e9-10cf5adde68d',
'adb20bc5-4d39-4727-a1e6-eda9d97d18a7',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'78c95ea9-6bbc-4b81-be26-b467e9b33b15',
'65efdc22-1f3e-4ff7-8432-ec5a0564cd7b',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'64259479-50b9-4cad-8b97-7d6b5c99a701',
'78c95ea9-6bbc-4b81-be26-b467e9b33b15',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'10855976-2645-4153-a711-a45a2c663f0d',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Wise',
'Software Engineer',
'Interesting engineering culture and remote-first team.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'47661908-88e6-482e-895d-989f4a27c743',
'10855976-2645-4153-a711-a45a2c663f0d',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'7a6e2c0a-9137-4c7b-baea-3e75161b9385',
'10855976-2645-4153-a711-a45a2c663f0d',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'ce7ae042-89f3-47e8-ad7a-9e9a06413dbc',
'10855976-2645-4153-a711-a45a2c663f0d',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'329672a6-fbd0-4c77-a24c-3076f87105c6',
'ce7ae042-89f3-47e8-ad7a-9e9a06413dbc',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'3d0012db-eca3-47e3-9a04-7be5a417ca27',
'10855976-2645-4153-a711-a45a2c663f0d',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'a1998310-2423-46fd-b616-ecc943fd9856',
'3d0012db-eca3-47e3-9a04-7be5a417ca27',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'7a8cc73b-7e5e-4f27-8060-7ca3dca9a823',
'10855976-2645-4153-a711-a45a2c663f0d',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'431c53c1-05df-4393-acbc-029d541c11a1',
'7a8cc73b-7e5e-4f27-8060-7ca3dca9a823',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'22bd8383-41f5-4540-a7e1-ba54c8244a70',
'10855976-2645-4153-a711-a45a2c663f0d',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'707cd340-a31a-44e6-b196-d1e49c08a9f7',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Datadog',
'Cloud Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'b0e37da7-869b-4420-850d-0b29c58995c5',
'707cd340-a31a-44e6-b196-d1e49c08a9f7',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'7efa37ee-24f7-4404-90a9-5f117abf2844',
'707cd340-a31a-44e6-b196-d1e49c08a9f7',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'27bfb90a-07ba-4fdf-9f53-5784e791efe4',
'707cd340-a31a-44e6-b196-d1e49c08a9f7',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'e44c7d6f-cb06-479b-b0af-ed02606b8fc0',
'27bfb90a-07ba-4fdf-9f53-5784e791efe4',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'11751452-7417-4370-ae03-e168cb363ff3',
'707cd340-a31a-44e6-b196-d1e49c08a9f7',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'be8b2abe-d466-4249-ae48-4112c9661e37',
'11751452-7417-4370-ae03-e168cb363ff3',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JobApplications
(Id, UserId, Company, Position, Note)
VALUES
(
'0e128014-9432-4eb0-b805-b147e702cd9d',
'513ae4a7-36e1-4b3b-80e1-ea36956018df',
'Shopify',
'Cloud Engineer',
'Strong alignment with cloud infrastructure background.'
);


INSERT INTO JobListing
(Id, JobApplicationId, JobDescription)
VALUES
(
'479e6b0b-ca11-49d9-bbfa-1d9506681719',
'0e128014-9432-4eb0-b805-b147e702cd9d',
'Looking for an engineer experienced with cloud services, REST APIs, CI/CD workflows, and modern software architecture practices.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'c4c10918-ca54-4feb-93eb-40c66dadb743',
'0e128014-9432-4eb0-b805-b147e702cd9d',
1,
0,
'2026-04-17 17:21:27',
'Application submitted through company careers portal.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'cb4fd750-0912-45b6-b3eb-06ee56a6fbc1',
'0e128014-9432-4eb0-b805-b147e702cd9d',
2,
1,
'2026-04-22 17:21:27',
'Recruiter reviewed application and scheduled intro call.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'33013369-e88f-4c03-b308-5d27303b1ca2',
'cb4fd750-0912-45b6-b3eb-06ee56a6fbc1',
'Recruiter Intro Call',
1,
DATEADD(day, 2, '2026-04-22 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'3a398251-8819-4123-9005-4bb9f5f453c6',
'0e128014-9432-4eb0-b805-b147e702cd9d',
3,
2,
'2026-04-27 17:21:27',
'Technical interview completed successfully.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'456251cb-caef-4bc1-a59a-ae127aa8c8eb',
'3a398251-8819-4123-9005-4bb9f5f453c6',
'Technical Interview',
1,
DATEADD(day, 2, '2026-04-27 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'15882b2e-4eb1-4725-8e8a-399d1ecd86de',
'0e128014-9432-4eb0-b805-b147e702cd9d',
4,
3,
'2026-05-02 17:21:27',
'Final round interview scheduled with engineering manager.'
);


INSERT INTO JAEventEntries
(Id, JAStatusEntryId, EventName, EventType, EventDate, IsWholeDay, Note)
VALUES
(
'b6ad3296-aefc-47b3-b4a3-3bddc66decf3',
'15882b2e-4eb1-4725-8e8a-399d1ecd86de',
'Final Team Interview',
1,
DATEADD(day, 2, '2026-05-02 17:21:27'),
0,
'Interview scheduled with hiring team.'
);


INSERT INTO JAStatusEntries
(Id, JobApplicationId, OrderIndex, JaStatusType, CreatedAt, Note)
VALUES
(
'736563f0-b38a-469c-8f55-64563c187b72',
'0e128014-9432-4eb0-b805-b147e702cd9d',
5,
4,
'2026-05-07 17:21:27',
'Offer received and compensation discussion started.'
);
