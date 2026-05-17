using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/events/types")]
[Authorize]
public class EventTypesController: ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<JaEventTypeDto>> GetEventTypes()
    {
        var statuses = Enum.GetValues<JAEventType>()
            .OrderBy(s => (int)s)
            .Select(s => new JaEventTypeDto() { Label = s.ToString(), Value = (int)s })
            .ToList();

        return Ok(statuses);
    }
}