using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/statuses")]
[Authorize]
public class StatusTypeController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<JaStatusTypeDto>> GetStatuses()
    {
        var statuses = Enum.GetValues<JAStatusType>()
            .OrderBy(s => (int)s)
            .Select(s => new JaStatusTypeDto() { Label = s.ToString(), Value = (int)s })
            .ToList();

        return Ok(statuses);
    }
}