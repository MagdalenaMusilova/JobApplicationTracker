using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Models;

[Keyless]
public class JAShortcut
{
    public Guid JAId { get; set; } 
    public string UserId { get; set; }
    public Guid StatusId { get; set; } 
    public Guid? EventId { get; set; }

}