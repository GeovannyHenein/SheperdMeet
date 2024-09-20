using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<PriestAvailabilityInput> PriestAvailabilities { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
}
