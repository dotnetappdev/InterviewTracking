using Microsoft.EntityFrameworkCore;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Data;

public class LocalDbContext : DbContext
{
    public LocalDbContext(DbContextOptions<LocalDbContext> options)
        : base(options)
    {
    }

    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Interviewer> Interviewers { get; set; }
    public DbSet<Reminder> Reminders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Interview>()
            .HasMany(i => i.Interviewers)
            .WithOne()
            .HasForeignKey(i => i.InterviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Interview>()
            .HasMany(i => i.Reminders)
            .WithOne()
            .HasForeignKey(r => r.InterviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
