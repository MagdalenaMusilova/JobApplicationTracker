# Job Application Tracker

A full-stack web application for managing and tracking job applications throughout the recruitment process. Built with .NET 8 and Next.js, featuring AI-powered resume matching and comprehensive application management.

## About

Job Application Tracker helps users organize their job search by tracking applications, managing recruitment events, and maintaining a professional profile. The system uses AI to evaluate candidate fit for specific positions based on resume analysis and job requirements.

## Features

- **Application Management**: Create, update, and delete job applications with status tracking
- **Status History**: Chronological tracking of application states with validation rules
- **Event Management**: Track recruitment events (interviews, tasks, etc.) linked to application statuses
- **User Profile**: Build a profile manually or by uploading a resume (CV)
- **AI Resume Matching**: Evaluate compatibility between user profile and job positions using OpenAI
- **Dashboard & Analytics**: View statistics and insights about your applications
- **Authentication**: Secure JWT-based authentication with refresh tokens

## Tech Stack

### Backend (.NET 8 / C#)
- **Framework**: ASP.NET Core Web API
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with JWT tokens
- **ORM**: Entity Framework Core with migrations
- **Documentation**: Swagger/OpenAPI
- **AI Integration**: OpenAI API via ChatGPT.Net
- **Mapping**: AutoMapper
- **Testing**: xUnit

**Key NuGet Packages:**
- Microsoft.EntityFrameworkCore (8.0.26)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.26)
- Npgsql.EntityFrameworkCore.PostgreSQL (8.0.11)
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.25)
- ChatGPT.Net (2.0.0)
- PdfPig (0.1.14)
- AutoMapper (16.1.1)

### Frontend (Next.js / TypeScript)
- **Framework**: Next.js 16 with React 19
- **Language**: TypeScript
- **Styling**: Tailwind CSS
- **UI Components**: Radix UI primitives with shadcn/ui
- **State Management**: TanStack Query (React Query)
- **Forms**: React Hook Form with Zod validation
- **HTTP Client**: Axios
- **Testing**: Jest + React Testing Library + Playwright (E2E)

> **Note**: Frontend was primarily designed using [v0 by Vercel](https://v0.dev) and tests were AI-generated.

## Project Structure

```
JobApplicationTracker/
├── JobApplicationTracker.Api/          # Backend API (.NET 8)
│   ├── Controllers/                    # API endpoints
│   │   ├── JobApplications/           # Job application controllers
│   │   ├── User/                      # User profile & resume controllers
│   │   ├── AuthController.cs          # Authentication
│   │   ├── DashboardController.cs     # Dashboard statistics
│   │   └── AiAgentController.cs       # AI matching
│   ├── Models/                        # Domain models
│   ├── DTOs/                          # Data Transfer Objects
│   ├── Repository/                    # Data access layer
│   ├── Services/                      # Business logic
│   │   ├── AIAgents/                 # AI integration
│   │   ├── Auth/                     # Authentication services
│   │   ├── DataExtractors/           # Resume parsing
│   │   ├── FileReaders/              # File handling
│   │   ├── JobApplications/          # Application logic
│   │   ├── ResumeMatching/           # AI matching logic
│   │   └── User/                     # User services
│   ├── Database/                      # DbContext configuration
│   ├── Migrations/                    # EF Core migrations
│   ├── Middleware/                    # Global exception handling
│   ├── Enums/                        # Application enums
│   └── Mapper/                       # AutoMapper profiles
├── JobApplicationTracker.Frontend/    # Frontend (Next.js)
│   ├── app/                          # Next.js app router pages
│   ├── components/                   # React components
│   ├── services/                     # API client services
│   ├── contexts/                     # React contexts
│   ├── hooks/                        # Custom React hooks
│   ├── types/                        # TypeScript types
│   └── __tests__/                    # Jest tests
└── JobApplicationTracker.Test/        # Backend tests (xUnit)
```

## Application Logic

### Job Application Status Flow

Each job application contains a chronologically ordered status history. Status transitions follow specific validation rules:

- Each status has a numeric value
- Generally, transitions require higher numeric values (forward progression)
- Exceptions exist (e.g., multiple interviews, returning to tasks)
- Every application must have at least one status
- Each status can have maximum one event attached
- Every event must be linked to a status

### Status Categories

Status numeric values are intentionally spaced for future extensibility:

- **≤ 100**: `Wishlist` - Saved opportunities without interaction
- **100-999**: Active recruitment process
- **≥ 1000**: Final states
  - **1000-1999**: Positive outcomes (Accepted)
  - **≥ 2000**: Negative outcomes (Rejected)

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL database
- Node.js 18+ (for frontend)
- OpenAI API key (for AI features)

### Backend Setup

1. Clone the repository:
```bash
git clone <repository-url>
cd JobApplicationTracker
```

2. Configure `appsettings.json` in `JobApplicationTracker.Api/`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=jobtracker;Username=youruser;Password=yourpass"
  },
  "JWT": {
    "Key": "your-secret-key-here",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  },
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  }
}
```

3. Run database migrations:
```bash
cd JobApplicationTracker.Api
dotnet ef database update
```

4. (Optional) Load test data from `InsertScript.sql`

5. Run the API:
```bash
dotnet run
```

6. Access Swagger UI:
- HTTP: `http://localhost:5131/swagger`
- HTTPS: `https://localhost:7002/swagger`

### Frontend Setup

1. Navigate to frontend directory:
```bash
cd JobApplicationTracker.Frontend
```

2. Install dependencies:
```bash
npm install
# or
pnpm install
```

3. Configure environment variables (create `.env.local`):
```env
NEXT_PUBLIC_API_URL=https://localhost:7002
```

4. Run the development server:
```bash
npm run dev
```

5. Open [http://localhost:3000](http://localhost:3000)

### Testing

#### Backend Tests
```bash
cd JobApplicationTracker.Test
dotnet test
```

#### Frontend Tests
```bash
cd JobApplicationTracker.Frontend

# Unit tests
npm run test

# E2E tests
npm run test:e2e

# All tests
npm run test:all
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Create new account
- `POST /api/auth/login` - Login and receive JWT token
- `POST /api/auth/refresh` - Refresh access token

### Job Applications
- `GET /api/applications` - Get all user applications
- `GET /api/applications/{id}` - Get application by ID
- `POST /api/applications` - Create new application
- `PUT /api/applications/{id}` - Update application
- `DELETE /api/applications/{id}` - Delete application
- `GET /api/applications/minimal` - Get minimal view of applications
- `GET /api/applications/notFinished` - Get applications not in final state

### User Profile & Resume
- `GET /api/user/profile` - Get user profile
- `POST /api/user/resume` - Upload resume
- `PUT /api/user/profile` - Update profile

### AI Matching
- `POST /api/ai/match` - Evaluate candidate fit for position

### Dashboard & Statistics
- `GET /api/dashboard` - Get dashboard statistics
- Various stats endpoints for analytics

## Database Schema

The application uses PostgreSQL with EF Core migrations. Key entities:

- **User**: User accounts and profiles
- **JobApplication**: Job application records
- **JobListing**: Job opportunity details
- **JAStatusEntry**: Status history entries
- **JAEventEntry**: Event records (interviews, tasks, etc.)
- **UserResume**: User skills and experience
- **RefreshToken**: JWT refresh tokens

Database views:
- `View_MinimalJA`: Simplified application overview
- `View_JAShortcut`: Quick access application data

## Development Notes

- The backend is implemented in .NET/C# following repository pattern, dependency injection, and clean architecture principles
- Frontend UI was designed using [v0 by Vercel](https://v0.dev)
- Frontend tests were AI-generated
- Authentication uses JWT with refresh tokens
- Database migrations are managed through Entity Framework Core
