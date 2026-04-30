using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/statuses")]
[Authorize]
public class StatusController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<JAStatusDto>> GetStatuses()
    {
        var statuses = Enum.GetValues<JAStatus>()
            .OrderBy(s => (int)s)
            .Select(s => new JAStatusDto() { StatusName = s.ToString(), StatusValue = (int)s })
            .ToList();

        return Ok(statuses);
    }
}