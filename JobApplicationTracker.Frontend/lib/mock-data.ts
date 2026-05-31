import {
  JobApplicationDto,
  JobApplicationListDto,
  ApplicationEventDto,
  ApplicationStatusHistoryDto,
  UserAccountDto,
  UserResumeDto,
  DashboardStatsDto,
  JobMatchDto,
  ApplicationStatus,
  WorkMode,
  ApplicationEventType,
} from '@/types';

// Helper to generate dates
const daysAgo = (days: number) => {
  const date = new Date();
  date.setDate(date.getDate() - days);
  return date.toISOString();
};

const daysFromNow = (days: number) => {
  const date = new Date();
  date.setDate(date.getDate() + days);
  return date.toISOString();
};

// Mock User Resume
export const mockUserResume: UserResumeDto = {
  id: 'resume-1',
  userId: 'user-1',
  workExperiences: [
    {
      id: 'we-1',
      startDate: '2021-03-01',
      endDate: undefined,
      company: 'TechCorp Inc',
      position: 'Senior Frontend Developer',
      jobDescription: [
        'Lead development of React-based dashboard applications',
        'Mentor junior developers and conduct code reviews',
        'Implement CI/CD pipelines and testing strategies',
      ],
      skills: [
        { id: 'su-1', skill: { id: 'sk-1', name: 'React' }, description: 'Built complex component libraries' },
        { id: 'su-2', skill: { id: 'sk-2', name: 'TypeScript' }, description: 'Type-safe application development' },
      ],
      notes: 'Promoted from Frontend Developer after 1 year',
    },
    {
      id: 'we-2',
      startDate: '2019-06-01',
      endDate: '2021-02-28',
      company: 'StartupXYZ',
      position: 'Frontend Developer',
      jobDescription: [
        'Developed customer-facing web applications',
        'Collaborated with UX designers on user interfaces',
        'Integrated RESTful APIs and GraphQL endpoints',
      ],
      skills: [
        { id: 'su-3', skill: { id: 'sk-3', name: 'JavaScript' }, description: 'ES6+ development' },
        { id: 'su-4', skill: { id: 'sk-4', name: 'Vue.js' }, description: 'Built single-page applications' },
      ],
      notes: '',
    },
    {
      id: 'we-3',
      startDate: '2018-01-01',
      endDate: '2019-05-31',
      company: 'Digital Agency Co',
      position: 'Junior Web Developer',
      jobDescription: [
        'Created responsive websites for clients',
        'Maintained and updated existing WordPress sites',
      ],
      skills: [
        { id: 'su-5', skill: { id: 'sk-5', name: 'HTML/CSS' }, description: 'Semantic markup and styling' },
        { id: 'su-6', skill: { id: 'sk-6', name: 'WordPress' }, description: 'Theme development' },
      ],
      notes: 'First professional development role',
    },
  ],
  education: [
    {
      id: 'edu-1',
      degree: 'Bachelor of Science',
      isFinished: true,
      school: 'University of California, Berkeley',
      majors: ['Computer Science'],
      skills: [
        { id: 'su-7', skill: { id: 'sk-7', name: 'Algorithms' }, description: 'Data structures and algorithms coursework' },
      ],
      notes: 'Graduated with honors',
    },
  ],
  trainings: [
    {
      id: 'tr-1',
      startDate: '2022-01-15',
      endDate: '2022-02-15',
      name: 'AWS Certified Developer',
      type: 'Certification',
      certification: ['AWS Certified Developer - Associate'],
      skills: [
        { id: 'su-8', skill: { id: 'sk-8', name: 'AWS' }, description: 'Cloud infrastructure and services' },
      ],
      notes: 'Valid until 2025',
    },
    {
      id: 'tr-2',
      startDate: '2023-06-01',
      endDate: '2023-06-15',
      name: 'Advanced TypeScript Patterns',
      type: 'Course',
      certification: [],
      skills: [
        { id: 'su-9', skill: { id: 'sk-2', name: 'TypeScript' }, description: 'Advanced type system features' },
      ],
      notes: 'Online course from Frontend Masters',
    },
  ],
  skills: [
    { id: 'sk-1', name: 'React', aliases: ['React.js', 'ReactJS'], skills: [], notes: 'Primary framework' },
    { id: 'sk-2', name: 'TypeScript', aliases: ['TS'], skills: [], notes: '4+ years experience' },
    { id: 'sk-9', name: 'Next.js', aliases: ['NextJS'], skills: [], notes: 'App router and pages router' },
    { id: 'sk-10', name: 'Node.js', aliases: ['Node', 'NodeJS'], skills: [], notes: 'Backend development' },
    { id: 'sk-11', name: 'GraphQL', aliases: [], skills: [], notes: 'Apollo Client and Server' },
    { id: 'sk-12', name: 'Tailwind CSS', aliases: ['TailwindCSS'], skills: [], notes: 'Utility-first CSS' },
    { id: 'sk-13', name: 'PostgreSQL', aliases: ['Postgres'], skills: [], notes: 'Database design and queries' },
  ],
  uncategorizedSkillUsages: [],
};

// Mock User Account (includes auth info)
export const mockUserAccount: UserAccountDto = {
  id: 'user-1',
  username: 'alexjohnson',
  email: 'alex.johnson@email.com',
  createdAt: '2023-06-15T10:30:00Z',
  resume: mockUserResume,
};

// Mock Applications (full details) - simplified structure
export const mockApplications: JobApplicationDto[] = [
  {
    id: 'app-1',
    companyName: 'Stripe',
    jobTitle: 'Senior Frontend Engineer',
    jobDescription: 'We are looking for a Senior Frontend Engineer to join our payments team. You will work on building beautiful, performant user interfaces for our dashboard and checkout products. Requirements: 5+ years of experience with React, TypeScript, and modern CSS. Experience with payment systems is a plus.',
    notes: 'Referred by Sarah from the payments team. Great company culture.',
    currentStatus: ApplicationStatus.Interview,
    createdAt: daysAgo(12),
    updatedAt: daysAgo(2),
    statusHistory: [
      { id: 'sh-1', applicationId: 'app-1', status: ApplicationStatus.Applied, changedAt: daysAgo(12), notes: 'Application submitted through referral' },
      { id: 'sh-2', applicationId: 'app-1', status: ApplicationStatus.PhoneScreen, changedAt: daysAgo(8), notes: 'Recruiter call scheduled' },
      { id: 'sh-3', applicationId: 'app-1', status: ApplicationStatus.Interview, changedAt: daysAgo(2), notes: 'Moving to technical interview round' },
    ],
    event: { id: 'ev-2', applicationId: 'app-1', eventType: ApplicationEventType.Interview, scheduledAt: daysFromNow(3), description: 'Technical interview with engineering team', location: 'Zoom', isCompleted: false, createdAt: daysAgo(2) },
  },
  {
    id: 'app-2',
    companyName: 'Vercel',
    jobTitle: 'Staff Engineer - Frontend Platform',
    jobDescription: 'Join our Frontend Platform team to build the future of web development. You will be working on Next.js, Turbopack, and our deployment infrastructure. We are looking for engineers with deep expertise in React, bundlers, and performance optimization.',
    notes: 'Dream job! Love their developer experience focus.',
    currentStatus: ApplicationStatus.TechnicalAssessment,
    createdAt: daysAgo(18),
    updatedAt: daysAgo(1),
    statusHistory: [
      { id: 'sh-4', applicationId: 'app-2', status: ApplicationStatus.Applied, changedAt: daysAgo(18) },
      { id: 'sh-5', applicationId: 'app-2', status: ApplicationStatus.PhoneScreen, changedAt: daysAgo(14) },
      { id: 'sh-6', applicationId: 'app-2', status: ApplicationStatus.Interview, changedAt: daysAgo(7) },
      { id: 'sh-7', applicationId: 'app-2', status: ApplicationStatus.TechnicalAssessment, changedAt: daysAgo(1), notes: 'Take-home project assigned' },
    ],
    event: { id: 'ev-3', applicationId: 'app-2', eventType: ApplicationEventType.TechnicalAssessment, scheduledAt: daysFromNow(5), description: 'Complete take-home project', notes: 'Build a mini deployment dashboard', isCompleted: false, createdAt: daysAgo(1) },
  },
  {
    id: 'app-3',
    companyName: 'Notion',
    jobTitle: 'Frontend Engineer',
    jobDescription: 'Help us build the future of productivity tools. As a Frontend Engineer at Notion, you will work on our block-based editor and collaborate with designers to create delightful user experiences. Strong React skills and attention to detail required.',
    notes: 'Great interview experience. Team seems collaborative.',
    currentStatus: ApplicationStatus.OfferReceived,
    createdAt: daysAgo(30),
    updatedAt: daysAgo(0),
    statusHistory: [
      { id: 'sh-8', applicationId: 'app-3', status: ApplicationStatus.Applied, changedAt: daysAgo(30) },
      { id: 'sh-9', applicationId: 'app-3', status: ApplicationStatus.PhoneScreen, changedAt: daysAgo(25) },
      { id: 'sh-10', applicationId: 'app-3', status: ApplicationStatus.Interview, changedAt: daysAgo(18) },
      { id: 'sh-11', applicationId: 'app-3', status: ApplicationStatus.FinalRound, changedAt: daysAgo(10) },
      { id: 'sh-12', applicationId: 'app-3', status: ApplicationStatus.OfferReceived, changedAt: daysAgo(0), notes: 'Offer: $185k base + equity' },
    ],
    event: { id: 'ev-4', applicationId: 'app-3', eventType: ApplicationEventType.FollowUp, scheduledAt: daysFromNow(7), description: 'Respond to offer', isCompleted: false, createdAt: daysAgo(0) },
  },
  {
    id: 'app-4',
    companyName: 'Linear',
    jobTitle: 'Product Engineer',
    jobDescription: 'Linear is looking for Product Engineers who can own features end-to-end. You should be comfortable with both frontend and backend development, and have a passion for building beautiful, fast software.',
    notes: 'Love their product. Applied through their website.',
    currentStatus: ApplicationStatus.PhoneScreen,
    createdAt: daysAgo(5),
    updatedAt: daysAgo(1),
    statusHistory: [
      { id: 'sh-13', applicationId: 'app-4', status: ApplicationStatus.Applied, changedAt: daysAgo(5) },
      { id: 'sh-14', applicationId: 'app-4', status: ApplicationStatus.PhoneScreen, changedAt: daysAgo(1), notes: 'Recruiter reached out' },
    ],
    event: { id: 'ev-5', applicationId: 'app-4', eventType: ApplicationEventType.PhoneScreen, scheduledAt: daysFromNow(2), description: 'Call with hiring manager', location: 'Google Meet', isCompleted: false, createdAt: daysAgo(1) },
  },
  {
    id: 'app-5',
    companyName: 'Figma',
    jobTitle: 'Senior Software Engineer',
    jobDescription: 'Join Figma to work on our collaborative design platform. You will tackle complex challenges around real-time collaboration, canvas rendering, and cross-platform development. Experience with WebGL and performance optimization is preferred.',
    notes: 'Didn\'t pass the system design round.',
    currentStatus: ApplicationStatus.Rejected,
    createdAt: daysAgo(45),
    updatedAt: daysAgo(15),
    statusHistory: [
      { id: 'sh-15', applicationId: 'app-5', status: ApplicationStatus.Applied, changedAt: daysAgo(45) },
      { id: 'sh-16', applicationId: 'app-5', status: ApplicationStatus.PhoneScreen, changedAt: daysAgo(38) },
      { id: 'sh-17', applicationId: 'app-5', status: ApplicationStatus.Interview, changedAt: daysAgo(28) },
      { id: 'sh-18', applicationId: 'app-5', status: ApplicationStatus.Rejected, changedAt: daysAgo(15), notes: 'Feedback: Need more experience with collaborative editing systems' },
    ],
    event: undefined,
  },
  {
    id: 'app-6',
    companyName: 'Airbnb',
    jobTitle: 'Frontend Engineer II',
    jobDescription: 'Airbnb is hiring Frontend Engineers to work on our guest and host experiences. You will build responsive, accessible web applications using React and work closely with our design systems team.',
    notes: 'Applied through LinkedIn. Excited about their design systems team.',
    currentStatus: ApplicationStatus.Applied,
    createdAt: daysAgo(2),
    updatedAt: daysAgo(2),
    statusHistory: [
      { id: 'sh-19', applicationId: 'app-6', status: ApplicationStatus.Applied, changedAt: daysAgo(2) },
    ],
    event: undefined,
  },
  {
    id: 'app-7',
    companyName: 'Shopify',
    jobTitle: 'Senior Frontend Developer',
    jobDescription: 'Work on Shopify\'s merchant-facing products and help millions of businesses sell online. We are looking for engineers experienced in React, GraphQL, and building scalable frontend architectures.',
    notes: 'Great remote culture. Interested in their Hydrogen framework.',
    currentStatus: ApplicationStatus.Interview,
    createdAt: daysAgo(20),
    updatedAt: daysAgo(3),
    statusHistory: [
      { id: 'sh-20', applicationId: 'app-7', status: ApplicationStatus.Applied, changedAt: daysAgo(20) },
      { id: 'sh-21', applicationId: 'app-7', status: ApplicationStatus.PhoneScreen, changedAt: daysAgo(14) },
      { id: 'sh-22', applicationId: 'app-7', status: ApplicationStatus.Interview, changedAt: daysAgo(3) },
    ],
    event: { id: 'ev-6', applicationId: 'app-7', eventType: ApplicationEventType.Interview, scheduledAt: daysFromNow(4), description: 'Technical interview - React deep dive', location: 'Zoom', isCompleted: false, createdAt: daysAgo(3) },
  },
  {
    id: 'app-8',
    companyName: 'Discord',
    jobTitle: 'Software Engineer - Web',
    jobDescription: 'Build features for Discord\'s web client used by millions of users. You will work on real-time communication features, UI components, and performance optimization. Experience with WebSocket and state management required.',
    notes: 'Interested in their real-time communication challenges.',
    currentStatus: ApplicationStatus.Applied,
    createdAt: daysAgo(1),
    updatedAt: daysAgo(1),
    statusHistory: [
      { id: 'sh-23', applicationId: 'app-8', status: ApplicationStatus.Applied, changedAt: daysAgo(1) },
    ],
    event: undefined,
  },
];

// Mock Applications List (summary view)
export const mockApplicationsList: JobApplicationListDto[] = mockApplications.map(app => ({
  id: app.id,
  companyName: app.companyName,
  jobTitle: app.jobTitle,
  currentStatus: app.currentStatus,
  appliedDate: app.createdAt,
  updatedAt: app.updatedAt,
  nextEventDate: app.event?.scheduledAt,
  nextEventType: app.event?.eventType,
}));

// Mock Events (upcoming and all)
export const mockEvents: ApplicationEventDto[] = mockApplications
  .filter(app => app.event)
  .map(app => ({
    ...app.event!,
    companyName: app.companyName,
    jobTitle: app.jobTitle,
  }))
  .sort((a, b) => new Date(a.scheduledAt).getTime() - new Date(b.scheduledAt).getTime());

export const mockUpcomingEvents = mockEvents.filter(e => !e.isCompleted && new Date(e.scheduledAt) > new Date());

// Mock Dashboard Stats
export const mockDashboardStats: DashboardStatsDto = {
  totalApplications: 8,
  activeApplications: 5,
  interviewsScheduled: 3,
  offersReceived: 1,
  applicationsByStatus: {
    [ApplicationStatus.Applied]: 2,
    [ApplicationStatus.PhoneScreen]: 1,
    [ApplicationStatus.Interview]: 2,
    [ApplicationStatus.TechnicalAssessment]: 1,
    [ApplicationStatus.FinalRound]: 0,
    [ApplicationStatus.OfferReceived]: 1,
    [ApplicationStatus.Accepted]: 0,
    [ApplicationStatus.Rejected]: 1,
    [ApplicationStatus.Withdrawn]: 0,
  },
  applicationsThisWeek: 3,
  applicationsThisMonth: 6,
};

// Mock Job Matches
export const mockJobMatches: JobMatchDto[] = [
  {
    companyName: 'Anthropic',
    jobTitle: 'Senior Frontend Engineer',
    jobUrl: 'https://anthropic.com/careers/frontend',
    location: 'San Francisco, CA',
    workMode: WorkMode.Hybrid,
    salaryMin: 200000,
    salaryMax: 280000,
    matchScore: 95,
    matchReasons: ['React/TypeScript expertise matches', 'Salary range aligns with expectations', 'Hybrid work mode preferred'],
    source: 'LinkedIn',
    postedDate: daysAgo(2),
  },
  {
    companyName: 'OpenAI',
    jobTitle: 'Frontend Platform Engineer',
    jobUrl: 'https://openai.com/careers',
    location: 'San Francisco, CA',
    workMode: WorkMode.Hybrid,
    salaryMin: 220000,
    salaryMax: 310000,
    matchScore: 92,
    matchReasons: ['Strong TypeScript background', 'Experience with complex UIs', 'Location match'],
    source: 'Company Website',
    postedDate: daysAgo(1),
  },
  {
    companyName: 'Supabase',
    jobTitle: 'Product Engineer',
    jobUrl: 'https://supabase.com/careers',
    location: 'Remote',
    workMode: WorkMode.Remote,
    salaryMin: 150000,
    salaryMax: 200000,
    matchScore: 88,
    matchReasons: ['PostgreSQL experience', 'Remote-first culture', 'Full-stack capabilities'],
    source: 'LinkedIn',
    postedDate: daysAgo(3),
  },
  {
    companyName: 'Planetscale',
    jobTitle: 'Senior Software Engineer',
    jobUrl: 'https://planetscale.com/careers',
    location: 'Remote',
    workMode: WorkMode.Remote,
    salaryMin: 160000,
    salaryMax: 210000,
    matchScore: 85,
    matchReasons: ['Database experience', 'Remote work preference', 'Modern tech stack'],
    source: 'Indeed',
    postedDate: daysAgo(5),
  },
  {
    companyName: 'Raycast',
    jobTitle: 'Frontend Developer',
    jobUrl: 'https://raycast.com/careers',
    location: 'Remote (EU)',
    workMode: WorkMode.Remote,
    salaryMin: 140000,
    salaryMax: 180000,
    matchScore: 82,
    matchReasons: ['React expertise', 'Developer tools interest', 'Remote work'],
    source: 'LinkedIn',
    postedDate: daysAgo(4),
  },
];

// Helper function to get application by ID
export const getApplicationById = (id: string): JobApplicationDto | undefined => {
  return mockApplications.find(app => app.id === id);
};

// Helper to get recent applications (last 5)
export const getRecentApplications = (): JobApplicationListDto[] => {
  return [...mockApplicationsList]
    .sort((a, b) => new Date(b.appliedDate).getTime() - new Date(a.appliedDate).getTime())
    .slice(0, 5);
};
