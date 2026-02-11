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
    public DbSet<MeetingPlatformType> MeetingPlatformTypes { get; set; }
    public DbSet<JobSource> JobSources { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Interview relationships
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

        // Configure Interview -> MeetingPlatformType relationship
        builder.Entity<Interview>()
            .HasOne(i => i.MeetingPlatformType)
            .WithMany(m => m.Interviews)
            .HasForeignKey(i => i.MeetingPlatformTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Interview -> JobSource relationship
        builder.Entity<Interview>()
            .HasOne(i => i.JobSource)
            .WithMany(j => j.Interviews)
            .HasForeignKey(i => i.JobSourceId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seed MeetingPlatformTypes
        builder.Entity<MeetingPlatformType>().HasData(
            new MeetingPlatformType
            {
                Id = 1,
                PlatformType = MeetingPlatform.Zoom,
                Name = "Zoom",
                Description = "Zoom video conferencing platform",
                IconUrl = "zoom_icon.png",
                IsActive = true
            },
            new MeetingPlatformType
            {
                Id = 2,
                PlatformType = MeetingPlatform.GoogleMeet,
                Name = "Google Meet",
                Description = "Google Meet video conferencing",
                IconUrl = "googlemeet_icon.png",
                IsActive = true
            },
            new MeetingPlatformType
            {
                Id = 3,
                PlatformType = MeetingPlatform.MicrosoftTeams,
                Name = "Microsoft Teams",
                Description = "Microsoft Teams collaboration platform",
                IconUrl = "teams_icon.png",
                IsActive = true
            },
            new MeetingPlatformType
            {
                Id = 4,
                PlatformType = MeetingPlatform.Other,
                Name = "Other",
                Description = "Other meeting platforms",
                IconUrl = "other_icon.png",
                IsActive = true
            }
        );

        // Seed JobSources
        builder.Entity<JobSource>().HasData(
            new JobSource
            {
                Id = 1,
                Name = "LinkedIn",
                Description = "LinkedIn job postings",
                IconUrl = "linkedin_icon.png",
                IsActive = true
            },
            new JobSource
            {
                Id = 2,
                Name = "Indeed",
                Description = "Indeed job search",
                IconUrl = "indeed_icon.png",
                IsActive = true
            },
            new JobSource
            {
                Id = 3,
                Name = "Glassdoor",
                Description = "Glassdoor job listings",
                IconUrl = "glassdoor_icon.png",
                IsActive = true
            },
            new JobSource
            {
                Id = 4,
                Name = "Company Website",
                Description = "Direct company career page",
                IconUrl = "company_icon.png",
                IsActive = true
            },
            new JobSource
            {
                Id = 5,
                Name = "Referral",
                Description = "Employee or professional referral",
                IconUrl = "referral_icon.png",
                IsActive = true
            },
            new JobSource
            {
                Id = 6,
                Name = "Recruiter",
                Description = "Contacted by recruiter",
                IconUrl = "recruiter_icon.png",
                IsActive = true
            },
            new JobSource
            {
                Id = 7,
                Name = "Other",
                Description = "Other job sources",
                IconUrl = "other_icon.png",
                IsActive = true
            }
        );

        // Ignore the Platform computed property for EF Core
        builder.Entity<Interview>()
            .Ignore(i => i.Platform);
    }
}

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
