using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models.UserProfile;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Test.Services;

public class ResumeServiceTests
{
    private readonly Mock<IPdfReader> _mockPdfReader;
    private readonly Mock<IResumeDataExtractor> _mockExtractor;
    private readonly Mock<IUserResumeRepository> _mockRepository;
    private readonly Mock<IResumeMergeService> _mockMergeService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ResumeService _service;

    public ResumeServiceTests()
    {
        _mockPdfReader = new Mock<IPdfReader>();
        _mockExtractor = new Mock<IResumeDataExtractor>();
        _mockRepository = new Mock<IUserResumeRepository>();
        _mockMergeService = new Mock<IResumeMergeService>();
        _mockMapper = new Mock<IMapper>();
        
        _service = new ResumeService(
            _mockPdfReader.Object,
            _mockExtractor.Object,
            _mockRepository.Object,
            _mockMergeService.Object,
            _mockMapper.Object);
    }

    private static IFormFile CreateMockFormFile()
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("resume.pdf");
        mockFile.Setup(f => f.Length).Returns(1024);
        return mockFile.Object;
    }

    private static UserResumeDto CreateSampleResumeDto()
    {
        return new UserResumeDto
        {
            Id = Guid.NewGuid(),
            Skills = new List<JobSkillDto> { new() { Name = "C#" } }
        };
    }

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_CreatesAndReturnsResume()
    {
        // Arrange
        var resumeDto = CreateSampleResumeDto();
        var resumeEntity = new UserResume { Id = resumeDto.Id };
        var createdEntity = new UserResume { Id = Guid.NewGuid() };
        var createdDto = CreateSampleResumeDto();

        _mockMapper.Setup(m => m.Map<UserResume>(resumeDto)).Returns(resumeEntity);
        _mockRepository.Setup(r => r.CreateAsync(resumeEntity)).ReturnsAsync(createdEntity);
        _mockMapper.Setup(m => m.Map<UserResumeDto>(createdEntity)).Returns(createdDto);

        // Act
        var result = await _service.CreateAsync(resumeDto);

        // Assert
        result.Should().Be(createdDto);
        _mockRepository.Verify(r => r.CreateAsync(resumeEntity), Times.Once);
    }

    #endregion

    #region ExtractFromPdfAsync Tests

    [Fact]
    public async Task ExtractFromPdfAsync_ExtractsAndReturnsResume()
    {
        // Arrange
        var file = CreateMockFormFile();
        var plainText = "John Doe\nSoftware Engineer";
        var extractedDto = CreateSampleResumeDto();

        _mockPdfReader.Setup(r => r.ReadText(file)).Returns(plainText);
        _mockExtractor.Setup(e => e.ExtractFromPlaintextAsync(plainText)).ReturnsAsync(extractedDto);

        // Act
        var result = await _service.ExtractFromPdfAsync(file);

        // Assert
        result.Should().Be(extractedDto);
        _mockPdfReader.Verify(r => r.ReadText(file), Times.Once);
        _mockExtractor.Verify(e => e.ExtractFromPlaintextAsync(plainText), Times.Once);
    }

    [Fact]
    public async Task ExtractFromPdfAsync_ThrowsException_WhenPdfReadFails()
    {
        // Arrange
        var file = CreateMockFormFile();
        _mockPdfReader.Setup(r => r.ReadText(file)).Returns(string.Empty);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.ExtractFromPdfAsync(file));
        
        exception.Message.Should().Be("Failed to read text from PDF");
        _mockExtractor.Verify(e => e.ExtractFromPlaintextAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ExtractFromPdfAsync_ThrowsException_WhenExtractionFails()
    {
        // Arrange
        var file = CreateMockFormFile();
        var plainText = "Some text";

        _mockPdfReader.Setup(r => r.ReadText(file)).Returns(plainText);
        _mockExtractor.Setup(e => e.ExtractFromPlaintextAsync(plainText)).ReturnsAsync((UserResumeDto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _service.ExtractFromPdfAsync(file));
        
        exception.Message.Should().Be("Failed to extract resume data from plaintext");
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ReturnsDto_WhenResumeExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new UserResume { Id = id };
        var dto = CreateSampleResumeDto();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        _mockMapper.Setup(m => m.Map<UserResumeDto>(entity)).Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().Be(dto);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenResumeDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((UserResume?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetByUserAsync Tests

    [Fact]
    public async Task GetByUserAsync_ReturnsDto_WhenResumeExists()
    {
        // Arrange
        var userId = "user123";
        var entity = new UserResume { UserId = userId };
        var dto = CreateSampleResumeDto();

        _mockRepository.Setup(r => r.GetByUserAsync(userId)).ReturnsAsync(entity);
        _mockMapper.Setup(m => m.Map<UserResumeDto>(entity)).Returns(dto);

        // Act
        var result = await _service.GetByUserAsync(userId);

        // Assert
        result.Should().Be(dto);
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsNull_WhenResumeDoesNotExist()
    {
        // Arrange
        var userId = "user123";
        _mockRepository.Setup(r => r.GetByUserAsync(userId)).ReturnsAsync((UserResume?)null);

        // Act
        var result = await _service.GetByUserAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_UpdatesAndReturnsResume()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new UserResume { Id = id, UserId = "user123" };
        var updateDto = CreateSampleResumeDto();
        var updatedEntity = new UserResume { Id = id };
        var resultDto = CreateSampleResumeDto();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map<UserResume>(updateDto)).Returns(updatedEntity);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<UserResume>())).ReturnsAsync(updatedEntity);
        _mockMapper.Setup(m => m.Map<UserResumeDto>(updatedEntity)).Returns(resultDto);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().Be(resultDto);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<UserResume>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenResumeDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = CreateSampleResumeDto();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((UserResume?)null);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<UserResume>()), Times.Never);
    }

    #endregion

    #region MergeAsync Tests

    [Fact]
    public async Task MergeAsync_MergesExistingResume_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existing = new UserResume { Id = id };
        var existingDto = CreateSampleResumeDto();
        var newDto = CreateSampleResumeDto();
        var mergedDto = CreateSampleResumeDto();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _mockMapper.Setup(m => m.Map<UserResumeDto>(existing)).Returns(existingDto);
        _mockMergeService.Setup(s => s.MergeAsync(existingDto, newDto)).ReturnsAsync(mergedDto);

        // Act
        var result = await _service.MergeAsync(id, newDto);

        // Assert
        result.Should().Be(mergedDto);
        _mockMergeService.Verify(s => s.MergeAsync(existingDto, newDto), Times.Once);
    }

    [Fact]
    public async Task MergeAsync_CreatesNewResume_WhenDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var newDto = CreateSampleResumeDto();
        var createdDto = CreateSampleResumeDto();

        _mockRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((UserResume?)null);
        _mockMapper.Setup(m => m.Map<UserResume>(newDto)).Returns(new UserResume());
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<UserResume>())).ReturnsAsync(new UserResume());
        _mockMapper.Setup(m => m.Map<UserResumeDto>(It.IsAny<UserResume>())).Returns(createdDto);

        // Act
        var result = await _service.MergeAsync(id, newDto);

        // Assert
        result.Should().NotBeNull();
        _mockMergeService.Verify(s => s.MergeAsync(It.IsAny<UserResumeDto>(), It.IsAny<UserResumeDto>()), Times.Never);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    #endregion
}