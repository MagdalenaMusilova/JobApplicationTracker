using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Database;

public class JAEventDbContext : DbContext
{
    public DbSet<JAEvent> JAEventEntries { get; set; }

    public JAEventDbContext(DbContextOptions<JAEventDbContext> options) : base(options)
    {
    }
}