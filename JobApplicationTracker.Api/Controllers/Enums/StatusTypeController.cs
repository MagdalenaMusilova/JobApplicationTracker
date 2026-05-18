using JobApplicationTracker.DTOs.Enums;
using JobApplicationTracker.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Controllers;

[ApiController]
[Route("api/statuses/types")]
[Authorize]
public class StatusTypeController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<JaStatusTypeDto>> GetStatusTypes()
    {
        var statuses = Enum.GetValues<JAStatusType>()
            .OrderBy(s => (int)s)
            .Select(s => new JaStatusTypeDto() { Label = s.ToString(), Value = (int)s })
            .ToList();

        return Ok(statuses);
    }
}