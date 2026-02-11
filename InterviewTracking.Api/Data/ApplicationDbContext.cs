using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
