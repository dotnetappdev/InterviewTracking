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
    public DbSet<MeetingPlatformType> MeetingPlatformTypes { get; set; }
    public DbSet<JobSource> JobSources { get; set; }
    public DbSet<InterviewFeedback> InterviewFeedback { get; set; }
    public DbSet<InterviewAttachment> InterviewAttachments { get; set; }

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

        builder.Entity<Interview>()
            .HasMany(i => i.Feedback)
            .WithOne()
            .HasForeignKey(f => f.InterviewId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Interview>()
            .HasMany(i => i.Attachments)
            .WithOne()
            .HasForeignKey(a => a.InterviewId)
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

        // Seed sample interviews
        builder.Entity<Interview>().HasData(
            new Interview
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Senior Software Engineer Interview",
                CompanyName = "Acme Corporation",
                JobTitle = "Senior Software Engineer",
                DateTime = DateTime.UtcNow.AddDays(5),
                MeetingLink = "https://zoom.us/j/123456789",
                Notes = "Technical interview focused on system design and algorithms",
                MeetingPlatformTypeId = 1, // Zoom
                JobSourceId = 1, // LinkedIn
                Status = InterviewStatus.Scheduled,
                JobPortalUrl = "https://linkedin.com/jobs/12345",
                JobPortalUsername = "user@example.com",
                JobPortalPassword = "password123",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Product Manager Interview",
                CompanyName = "Beta Technologies",
                JobTitle = "Product Manager",
                DateTime = DateTime.UtcNow.AddDays(7),
                MeetingLink = "https://meet.google.com/abc-defg-hij",
                Notes = "Behavioral and product strategy interview",
                MeetingPlatformTypeId = 2, // Google Meet
                JobSourceId = 2, // Indeed
                Status = InterviewStatus.Stage1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "DevOps Engineer Interview",
                CompanyName = "Cloud Innovations Inc",
                JobTitle = "DevOps Engineer",
                DateTime = DateTime.UtcNow.AddDays(10),
                MeetingLink = "https://teams.microsoft.com/l/meetup-join/...",
                Notes = "Cloud architecture and CI/CD pipeline discussion",
                MeetingPlatformTypeId = 3, // Microsoft Teams
                JobSourceId = 3, // Glassdoor
                Status = InterviewStatus.Stage2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Title = "Data Scientist Interview",
                CompanyName = "DataViz Analytics",
                JobTitle = "Data Scientist",
                DateTime = DateTime.UtcNow.AddDays(12),
                MeetingLink = "https://zoom.us/j/987654321",
                Notes = "Machine learning and statistical modeling interview",
                MeetingPlatformTypeId = 1, // Zoom
                JobSourceId = 4, // Company Website
                Status = InterviewStatus.FinalRound,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Title = "UX Designer Interview",
                CompanyName = "Design Studios Ltd",
                JobTitle = "UX Designer",
                DateTime = DateTime.UtcNow.AddDays(15),
                MeetingLink = "https://meet.google.com/xyz-abcd-efg",
                Notes = "Portfolio review and design process discussion",
                MeetingPlatformTypeId = 2, // Google Meet
                JobSourceId = 5, // Referral
                Status = InterviewStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}
