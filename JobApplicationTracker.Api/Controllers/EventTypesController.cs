using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/statuses")]
[Authorize]
public class EventTypesController: ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<JaEventTypeDto>> GetStatuses()
    {
        var statuses = Enum.GetValues<JAEventType>()
            .OrderBy(s => (int)s)
            .Select(s => new JaStatusTypeDto() { Label = s.ToString(), Value = (int)s })
            .ToList();

        return Ok(statuses);
    }
}