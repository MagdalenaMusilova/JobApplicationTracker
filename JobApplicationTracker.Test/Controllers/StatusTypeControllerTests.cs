using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class StatusTypeControllerTests
{
    private readonly StatusTypeController _controller;

    public StatusTypeControllerTests()
    {
        _controller = new StatusTypeController();
    }

    [Fact]
    public void GetStatusTypes_ReturnsAllStatusTypes()
    {
        // Act
        var result = _controller.GetStatusTypes();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var statusTypes = okResult.Value.Should().BeAssignableTo<IEnumerable<JaStatusTypeDto>>().Subject;
        
        var enumCount = Enum.GetValues<JAStatusType>().Length;
        statusTypes.Should().HaveCount(enumCount);
        
        // Verify they are ordered by value
        var values = statusTypes.Select(s => s.Value).ToList();
        values.Should().BeInAscendingOrder();
    }
}