using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Database;

public class JAStatusEntryDbContext : DbContext
{
    public DbSet<JAStatusEntry> JAStatusEntries { get; set; }

    public JAStatusEntryDbContext(DbContextOptions<JAStatusEntryDbContext> options) : base(options)
    {
    }
}