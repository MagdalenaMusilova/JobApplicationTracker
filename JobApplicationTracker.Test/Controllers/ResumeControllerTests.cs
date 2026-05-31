using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Test.Controllers;

public class ResumeControllerTests
{
    private readonly Mock<IResumeService> _mockResumeService;
    private readonly ResumeController _controller;

    public ResumeControllerTests()
    {
        _mockResumeService = new Mock<IResumeService>();
        _controller = new ResumeController(_mockResumeService.Object);
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

    private static void AssertError(object? value, string expectedCode, string expectedMessage)
    {
        var error = value.Should().BeOfType<ErrorResponseDto>().Subject;
        error.Code.Should().Be(expectedCode);
        error.Message.Should().Be(expectedMessage);
    }

    private static IFormFile CreateMockFormFile(
        string fileName = "resume.pdf",
        string contentType = "application/pdf",
        long length = 1024)
    {
        var mockFile = new Mock<IFormFile>();
        var content = new byte[length];
        var ms = new MemoryStream(content);

        mockFile.Setup(f => f.FileName).Returns(fileName);
        mockFile.Setup(f => f.Length).Returns(length);
        mockFile.Setup(f => f.ContentType).Returns(contentType);
        mockFile.Setup(f => f.OpenReadStream()).Returns(ms);
        mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream, token));

        return mockFile.Object;
    }

    private static UserResumeDto CreateSampleResume(Guid? id = null, string? userId = null)
    {
        return new UserResumeDto
        {
            Id = id ?? Guid.NewGuid(),
            UserId = userId ?? "userId",
            Skills = new List<JobSkillDto>
            {
                new() { Name = "C#" },
                new() { Name = ".NET" }
            },
            WorkExperiences = new List<WorkExperienceDto>
            {
                new()
                {
                    Company = "Tech Corp",
                    Position = "Senior Developer",
                    StartDate = new DateOnly(2020, 1, 1),
                    EndDate = new DateOnly(2024, 1, 1)
                }
            }
        };
    }

    [Fact]
    public async Task Create_ReturnsOk_WithCreatedResume()
    {
        // Arrange
        var userId = "user123";
        SetupUser(userId);

        var resumeDto = CreateSampleResume(userId: "differentUser");
        var createdResume = CreateSampleResume(id: Guid.NewGuid(), userId: userId);

        _mockResumeService
            .Setup(s => s.GetByUserAsync(userId))
            .ReturnsAsync((UserResumeDto?)null);

        _mockResumeService
            .Setup(s => s.CreateAsync(resumeDto))
            .ReturnsAsync(createdResume);

        // Act
        var result = await _controller.Create(resumeDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedResume = okResult.Value.Should().BeOfType<UserResumeDto>().Subject;
        returnedResume.Id.Should().Be(createdResume.Id);
        returnedResume.UserId.Should().Be(userId);

        resumeDto.UserId.Should().Be(userId);
        _mockResumeService.Verify(s => s.GetByUserAsync(userId), Times.Once);
        _mockResumeService.Verify(s => s.CreateAsync(resumeDto), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenResumeAlreadyExists()
    {
        // Arrange
        var userId = "user123";
        SetupUser(userId);

        var resumeDto = CreateSampleResume(userId: userId);
        var existingResume = CreateSampleResume(userId: userId);

        _mockResumeService
            .Setup(s => s.GetByUserAsync(userId))
            .ReturnsAsync(existingResume);

        // Act
        var result = await _controller.Create(resumeDto);

        // Assert
        var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
        AssertError(conflictResult.Value, "RESUME_ALREADY_EXISTS", "A resume already exists for this user.");

        _mockResumeService.Verify(s => s.CreateAsync(It.IsAny<UserResumeDto>()), Times.Never);
    }

    [Fact]
    public async Task Create_CallsServiceWithCurrentUserId()
    {
        // Arrange
        var userId = "user123";
        SetupUser(userId);

        var resumeDto = CreateSampleResume(userId: "otherUser");

        _mockResumeService
            .Setup(s => s.GetByUserAsync(userId))
            .ReturnsAsync((UserResumeDto?)null);

        _mockResumeService
            .Setup(s => s.CreateAsync(It.IsAny<UserResumeDto>()))
            .ReturnsAsync((UserResumeDto resume) => resume);

        // Act
        await _controller.Create(resumeDto);

        // Assert
        _mockResumeService.Verify(
            s => s.CreateAsync(It.Is<UserResumeDto>(r => r.UserId == userId)),
            Times.Once);
    }

    [Fact]
    public async Task ExtractFromPdf_ReturnsOk_WithExtractedResume()
    {
        // Arrange
        var mockFile = CreateMockFormFile();
        var request = new PdfUploadRequestDto { File = mockFile };
        var extractedResume = CreateSampleResume();

        _mockResumeService
            .Setup(s => s.ExtractFromPdfAsync(mockFile))
            .ReturnsAsync(extractedResume);

        // Act
        var result = await _controller.ExtractFromPdf(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedResume = okResult.Value.Should().BeOfType<UserResumeDto>().Subject;
        returnedResume.Should().BeEquivalentTo(extractedResume);

        _mockResumeService.Verify(s => s.ExtractFromPdfAsync(mockFile), Times.Once);
    }

    [Fact]
    public async Task ExtractFromPdf_ReturnsBadRequest_WhenFileIsNull()
    {
        // Arrange
        var request = new PdfUploadRequestDto { File = null! };

        // Act
        var result = await _controller.ExtractFromPdf(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "PDF_FILE_REQUIRED", "Please upload a PDF file.");

        _mockResumeService.Verify(
            s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()),
            Times.Never);
    }

    [Fact]
    public async Task ExtractFromPdf_ReturnsBadRequest_WhenFileIsEmpty()
    {
        // Arrange
        var mockFile = CreateMockFormFile(length: 0);
        var request = new PdfUploadRequestDto { File = mockFile };

        // Act
        var result = await _controller.ExtractFromPdf(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "PDF_FILE_REQUIRED", "Please upload a PDF file.");

        _mockResumeService.Verify(
            s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()),
            Times.Never);
    }

    [Fact]
    public async Task ExtractFromPdf_ReturnsBadRequest_WhenFileIsNotPdf()
    {
        // Arrange
        var mockFile = CreateMockFormFile("resume.docx", "application/vnd.openxmlformats");
        var request = new PdfUploadRequestDto { File = mockFile };

        // Act
        var result = await _controller.ExtractFromPdf(request);

        // Assert
        var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "INVALID_FILE_TYPE", "Only PDF files are allowed.");

        _mockResumeService.Verify(
            s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()),
            Times.Never);
    }

    [Fact]
    public async Task ExtractFromPdf_AcceptsPdfWithUppercaseExtension()
    {
        // Arrange
        var mockFile = CreateMockFormFile("resume.PDF");
        var request = new PdfUploadRequestDto { File = mockFile };
        var extractedResume = CreateSampleResume();

        _mockResumeService.Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .ReturnsAsync(extractedResume);

        // Act
        var result = await _controller.ExtractFromPdf(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _mockResumeService.Verify(s => s.ExtractFromPdfAsync(mockFile), Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenResumeExistsAndUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var resume = CreateSampleResume(id: resumeId, userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(resume);

        // Act
        var result = await _controller.GetById(resumeId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedResume = okResult.Value.Should().BeOfType<UserResumeDto>().Subject;
        returnedResume.Id.Should().Be(resumeId);

        _mockResumeService.Verify(s => s.GetByIdAsync(resumeId), Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenResumeDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync((UserResumeDto?)null);

        // Act
        var result = await _controller.GetById(resumeId);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.GetByIdAsync(resumeId), Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenUserDoesNotOwnResume()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var resume = CreateSampleResume(id: resumeId, userId: "otherUser");

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(resume);

        // Act
        var result = await _controller.GetById(resumeId);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");
    }

    [Fact]
    public async Task GetByUserId_ReturnsOk_WhenResumeExistsAndRequestedUserIsCurrentUser()
    {
        // Arrange
        var userId = "user123";
        var resume = CreateSampleResume(userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByUserAsync(userId))
            .ReturnsAsync(resume);

        // Act
        var result = await _controller.GetByUserId(userId);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedResume = okResult.Value.Should().BeOfType<UserResumeDto>().Subject;
        returnedResume.UserId.Should().Be(userId);

        _mockResumeService.Verify(s => s.GetByUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetByUserId_ReturnsNotFound_WhenRequestedUserIsNotCurrentUser()
    {
        // Arrange
        SetupUser("user123");

        // Act
        var result = await _controller.GetByUserId("otherUser");

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.GetByUserAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByUserId_ReturnsNotFound_WhenResumeDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByUserAsync(userId))
            .ReturnsAsync((UserResumeDto?)null);

        // Act
        var result = await _controller.GetByUserId(userId);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.GetByUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenResumeIsUpdatedAndUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var updateDto = CreateSampleResume(id: resumeId, userId: "otherUser");
        var existingResume = CreateSampleResume(id: resumeId, userId: userId);
        var updatedResume = CreateSampleResume(id: resumeId, userId: userId);
        updatedResume.Skills = new List<JobSkillDto>
        {
            new() { Name = "Python" }
        };

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        _mockResumeService
            .Setup(s => s.UpdateAsync(resumeId, updateDto))
            .ReturnsAsync(updatedResume);

        // Act
        var result = await _controller.Update(resumeId, updateDto);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedResume = okResult.Value.Should().BeOfType<UserResumeDto>().Subject;
        returnedResume.Id.Should().Be(resumeId);

        updateDto.Id.Should().Be(resumeId);
        updateDto.UserId.Should().Be(userId);

        _mockResumeService.Verify(s => s.GetByIdAsync(resumeId), Times.Once);
        _mockResumeService.Verify(s => s.UpdateAsync(resumeId, updateDto), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenResumeDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var updateDto = CreateSampleResume(id: resumeId, userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync((UserResumeDto?)null);

        // Act
        var result = await _controller.Update(resumeId, updateDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UserResumeDto>()), Times.Never);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenUserDoesNotOwnResume()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var updateDto = CreateSampleResume(id: resumeId, userId: userId);
        var existingResume = CreateSampleResume(id: resumeId, userId: "otherUser");

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        // Act
        var result = await _controller.Update(resumeId, updateDto);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UserResumeDto>()), Times.Never);
    }

    [Fact]
    public async Task Merge_ReturnsOk_WhenResumeIsMergedAndUserOwnsIt()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var mockFile = CreateMockFormFile();
        var request = new PdfUploadRequestDto { File = mockFile };

        var existingResume = CreateSampleResume(id: resumeId, userId: userId);
        var extractedResume = CreateSampleResume(userId: userId);
        var mergedResume = CreateSampleResume(id: resumeId, userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        _mockResumeService
            .Setup(s => s.ExtractFromPdfAsync(mockFile))
            .ReturnsAsync(extractedResume);

        _mockResumeService
            .Setup(s => s.MergeAsync(resumeId, extractedResume))
            .ReturnsAsync(mergedResume);

        // Act
        var result = await _controller.Merge(resumeId, request);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedResume = okResult.Value.Should().BeOfType<UserResumeDto>().Subject;
        returnedResume.Id.Should().Be(resumeId);

        _mockResumeService.Verify(s => s.GetByIdAsync(resumeId), Times.Once);
        _mockResumeService.Verify(s => s.ExtractFromPdfAsync(mockFile), Times.Once);
        _mockResumeService.Verify(s => s.MergeAsync(resumeId, extractedResume), Times.Once);
    }

    [Fact]
    public async Task Merge_ReturnsNotFound_WhenResumeDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var mockFile = CreateMockFormFile();
        var request = new PdfUploadRequestDto { File = mockFile };

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync((UserResumeDto?)null);

        // Act
        var result = await _controller.Merge(resumeId, request);

        // Assert
        var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()), Times.Never);
        _mockResumeService.Verify(s => s.MergeAsync(It.IsAny<Guid>(), It.IsAny<UserResumeDto>()), Times.Never);
    }

    [Fact]
    public async Task Merge_ReturnsBadRequest_WhenFileIsNotPdf()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var mockFile = CreateMockFormFile("resume.docx", "application/vnd.openxmlformats");
        var request = new PdfUploadRequestDto { File = mockFile };
        var existingResume = CreateSampleResume(id: resumeId, userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        // Act
        var result = await _controller.Merge(resumeId, request);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        AssertError(badRequestResult.Value, "INVALID_FILE_TYPE", "Only PDF files are allowed.");

        _mockResumeService.Verify(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()), Times.Never);
        _mockResumeService.Verify(s => s.MergeAsync(It.IsAny<Guid>(), It.IsAny<UserResumeDto>()), Times.Never);
    }

    [Fact]
    public async Task Merge_CallsExtractBeforeMerge()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var mockFile = CreateMockFormFile();
        var request = new PdfUploadRequestDto { File = mockFile };
        var existingResume = CreateSampleResume(id: resumeId, userId: userId);

        var callOrder = new List<string>();

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        _mockResumeService
            .Setup(s => s.ExtractFromPdfAsync(It.IsAny<IFormFile>()))
            .Callback(() => callOrder.Add("extract"))
            .ReturnsAsync(CreateSampleResume(userId: userId));

        _mockResumeService
            .Setup(s => s.MergeAsync(It.IsAny<Guid>(), It.IsAny<UserResumeDto>()))
            .Callback(() => callOrder.Add("merge"))
            .ReturnsAsync(CreateSampleResume(id: resumeId, userId: userId));

        // Act
        await _controller.Merge(resumeId, request);

        // Assert
        callOrder.Should().ContainInOrder("extract", "merge");
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenUserOwnsResume()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var existingResume = CreateSampleResume(id: resumeId, userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        _mockResumeService
            .Setup(s => s.DeleteAsync(resumeId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(resumeId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _mockResumeService.Verify(s => s.GetByIdAsync(resumeId), Times.Once);
        _mockResumeService.Verify(s => s.DeleteAsync(resumeId), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenUserDoesNotOwnResume()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var existingResume = CreateSampleResume(id: resumeId, userId: "otherUser");

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        // Act
        var result = await _controller.Delete(resumeId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        AssertError(notFoundResult.Value, "RESUME_NOT_FOUND", "Resume was not found.");

        _mockResumeService.Verify(s => s.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Delete_CallsServiceWithCorrectId()
    {
        // Arrange
        var userId = "user123";
        var resumeId = Guid.NewGuid();
        var existingResume = CreateSampleResume(id: resumeId, userId: userId);

        SetupUser(userId);

        _mockResumeService
            .Setup(s => s.GetByIdAsync(resumeId))
            .ReturnsAsync(existingResume);

        _mockResumeService.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        await _controller.Delete(resumeId);

        // Assert
        _mockResumeService.Verify(
            s => s.DeleteAsync(It.Is<Guid>(id => id == resumeId)),
            Times.Once);
    }
}