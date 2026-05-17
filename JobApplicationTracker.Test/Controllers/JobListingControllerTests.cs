using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Test.Controllers;

public class JobListingControllerTests
{
    private readonly Mock<IResumeService> _mockResumeService;
    private readonly Mock<IJobMatchingService> _mockJobMatchingService;
    private readonly JobListingController _controller;

    public JobListingControllerTests()
    {
        _mockResumeService = new Mock<IResumeService>();
        _mockJobMatchingService = new Mock<IJobMatchingService>();
        _controller = new JobListingController(
            _mockResumeService.Object,
            _mockJobMatchingService.Object);
    }

    private void SetupUser(string userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    private static IFormFile CreateMockFormFile(string fileName = "resume.pdf", string contentType = "application/pdf")
    {
        var mockFile = new Mock<IFormFile>();
        var content = "fake pdf content"u8.ToArray();
        var ms = new MemoryStream(content);

        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(ms.Length);
        mockFile.Setup(f => f.ContentType).Returns(contentType);
        mockFile.Setup(f => f.OpenReadStream()).Returns(ms);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream, token));

        return mockFile.Object;
    }

    [Fact]
    public async Task Create_ReturnsOk_WithMatchingResult()
    {
        // Arrange
        var userId = "user123";
        SetupUser(userId);

        var mockFile = CreateMockFormFile();
        var jobListing = "Senior Developer position requiring C# and .NET experience";
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = jobListing
        };

        var extractedResume = new UserResumeDto
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Skills = new List<JobSkillDto>
            {
                new() { Name = "C#" },
                new() { Name = ".NET" }
            }
        };

        var matchResult = "85% match - Strong candidate. Skills align well with job requirements.";

        _mockResumeService
            .Setup(s => s.ExtractFromPdfAsync(mockFile))
            .ReturnsAsync(extractedResume);

        _mockJobMatchingService
            .Setup(s => s.EvaluateMatch(extractedResume, jobListing))
            .ReturnsAsync(matchResult);

        // Act
        var result = await _controller.Create(matchRequest);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(matchResult);

        _mockResumeService.Verify(s => s.ExtractFromPdfAsync(mockFile), Times.Once);
        _mockJobMatchingService.Verify(s => s.EvaluateMatch(extractedResume, jobListing), Times.Once);
    }

    [Fact]
    public async Task Create_CallsResumeServiceWithCorrectFile()
    {
        // Arrange
        var mockFile = CreateMockFormFile("test-resume.pdf");
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = "Test job listing"
        };

        var extractedResume = new UserResumeDto();
        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);
        _mockJobMatchingService.Setup(s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), It.IsAny<string>()))
            .ReturnsAsync("match result");

        // Act
        await _controller.Create(matchRequest);

        // Assert
        _mockResumeService.Verify(
            s => s.ExtractFromPdfAsync(It.Is<IFormFile>(f => f.FileName == "test-resume.pdf")),
            Times.Once);
    }

    [Fact]
    public async Task Create_CallsJobMatchingServiceWithCorrectParameters()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var jobListing = "Looking for a skilled backend developer with 5+ years experience";
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = jobListing
        };

        var extractedResume = new UserResumeDto { Id = Guid.NewGuid() };
        
        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);
        _mockJobMatchingService.Setup(s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), It.IsAny<string>()))
            .ReturnsAsync("match result");

        // Act
        await _controller.Create(matchRequest);

        // Assert
        _mockJobMatchingService.Verify(
            s => s.EvaluateMatch(
                It.Is<UserResumeDto>(r => r.Id == extractedResume.Id),
                jobListing),
            Times.Once);
    }

    [Fact]
    public async Task Create_PropagatesExceptionFromResumeService()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = "Test job"
        };

        _mockResumeService
            .Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ThrowsAsync(new InvalidOperationException("Failed to parse PDF"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _controller.Create(matchRequest));

        _mockJobMatchingService.Verify(
            s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task Create_PropagatesExceptionFromJobMatchingService()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = "Test job"
        };

        var extractedResume = new UserResumeDto();
        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);

        _mockJobMatchingService
            .Setup(s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("AI service unavailable"));

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await _controller.Create(matchRequest));
    }

    [Fact]
    public async Task Create_HandlesEmptyJobListing()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = string.Empty
        };

        var extractedResume = new UserResumeDto();
        var matchResult = "Cannot evaluate match with empty job listing";

        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);
        _mockJobMatchingService.Setup(s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), string.Empty))
            .ReturnsAsync(matchResult);

        // Act
        var result = await _controller.Create(matchRequest);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(matchResult);
    }

    [Fact]
    public async Task Create_HandlesResumeWithNoSkills()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = "Senior Developer needed"
        };

        var extractedResume = new UserResumeDto
        {
            Skills = new List<JobSkillDto>()
        };

        var matchResult = "20% match - Resume lacks relevant skills";

        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);
        _mockJobMatchingService.Setup(s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), It.IsAny<string>()))
            .ReturnsAsync(matchResult);

        // Act
        var result = await _controller.Create(matchRequest);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().Be(matchResult);
        _mockJobMatchingService.Verify(
            s => s.EvaluateMatch(
                It.Is<UserResumeDto>(r => r.Skills.Count == 0),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task Create_WorksWithDifferentFileTypes()
    {
        // Arrange
        var mockFile = CreateMockFormFile("resume.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        var matchRequest = new MatchRequestDto
        {
            ResumeFile = mockFile,
            JobListing = "Test job"
        };

        var extractedResume = new UserResumeDto();
        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);
        _mockJobMatchingService.Setup(s => s.EvaluateMatch(It.IsAny<UserResumeDto>(), It.IsAny<string>()))
            .ReturnsAsync("match result");

        // Act
        var result = await _controller.Create(matchRequest);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        _mockResumeService.Verify(
            s => s.ExtractFromPdfAsync(It.Is<IFormFile>(f => f.ContentType.Contains("word"))),
            Times.Once);
    }
}