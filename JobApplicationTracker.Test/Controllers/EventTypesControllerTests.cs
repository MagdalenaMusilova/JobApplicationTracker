using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class EventTypesControllerTests
{
    private readonly EventTypesController _controller;

    public EventTypesControllerTests()
    {
        _controller = new EventTypesController();
    }

    [Fact]
    public void GetEventTypes_ReturnsAllEventTypes()
    {
        // Act
        var result = _controller.GetEventTypes();

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var eventTypes = okResult.Value.Should().BeAssignableTo<IEnumerable<JaStatusTypeDto>>().Subject;
        
        var enumCount = Enum.GetValues<JAEventType>().Length;
        eventTypes.Should().HaveCount(enumCount);
        
        // Verify they are ordered by value
        var values = eventTypes.Select(e => e.Value).ToList();
        values.Should().BeInAscendingOrder();
    }
}