using System.Text;
using JobApplicationTracker.Database;
using JobApplicationTracker.Mapper;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<JobApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<JAEventDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<UserResumeDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IJAStatusEntryService, JAStatusEntryService>();
builder.Services.AddScoped<IJAStatusEntryRepository, JAStatusEntryRepository>();
builder.Services.AddScoped<IAiAgentService, OpenAiAgentService>();
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IPdfReader, PdfReader>();
builder.Services.AddScoped<IResumeDataExtractor, AiResumeDataExtractor>();
builder.Services.AddScoped<IJobListingExtractor, AiJobListingExtractor>();
builder.Services.AddScoped<IJobListingService, JobListingService>();
builder.Services.AddScoped<IJobMatchingService, JobMatchingService>();
builder.Services.AddScoped<IJAEventService, JAEventService>();
builder.Services.AddScoped<IJAEventRepository, JAEventRepository>();
builder.Services.AddScoped<IUserResumeRepository, UserResumeRepository>();
builder.Services.AddScoped<IResumeMergeService, ResumeMergeService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddHttpClient<OpenAiAgentService>();

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile).Assembly);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();