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
            },
            new Interview
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                Title = "Frontend Developer - React",
                CompanyName = "TechStartup Inc",
                JobTitle = "Frontend Developer",
                DateTime = DateTime.UtcNow.AddDays(3),
                MeetingLink = "https://zoom.us/j/111222333",
                Notes = "Focus on React, TypeScript, and modern frontend practices",
                MeetingPlatformTypeId = 1, // Zoom
                JobSourceId = 1, // LinkedIn
                Status = InterviewStatus.Scheduled,
                JobPortalUrl = "https://linkedin.com/jobs/67890",
                JobPortalUsername = "developer@example.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                Title = "Backend Engineer - .NET",
                CompanyName = "Enterprise Solutions Corp",
                JobTitle = "Backend Engineer",
                DateTime = DateTime.UtcNow.AddDays(8),
                MeetingLink = "https://teams.microsoft.com/l/meetup-join/backend",
                Notes = "ASP.NET Core, microservices, and cloud architecture",
                MeetingPlatformTypeId = 3, // Microsoft Teams
                JobSourceId = 6, // Recruiter
                Status = InterviewStatus.Stage1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Title = "Full Stack Developer",
                CompanyName = "FinTech Innovations",
                JobTitle = "Full Stack Developer",
                DateTime = DateTime.UtcNow.AddDays(11),
                MeetingLink = "https://meet.google.com/fintech-interview",
                Notes = "Experience with React, Node.js, and financial systems",
                MeetingPlatformTypeId = 2, // Google Meet
                JobSourceId = 2, // Indeed
                Status = InterviewStatus.Stage2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                Title = "Mobile Developer - iOS",
                CompanyName = "Mobile Apps Studio",
                JobTitle = "iOS Developer",
                DateTime = DateTime.UtcNow.AddDays(6),
                MeetingLink = "https://zoom.us/j/444555666",
                Notes = "Swift, SwiftUI, and iOS app development",
                MeetingPlatformTypeId = 1, // Zoom
                JobSourceId = 3, // Glassdoor
                Status = InterviewStatus.Scheduled,
                JobPortalUrl = "https://glassdoor.com/job-listing/mobile-ios",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Title = "QA Engineer - Automation",
                CompanyName = "Quality Systems Inc",
                JobTitle = "QA Automation Engineer",
                DateTime = DateTime.UtcNow.AddDays(9),
                MeetingLink = "https://teams.microsoft.com/l/meetup-join/qa",
                Notes = "Selenium, Cypress, and test automation frameworks",
                MeetingPlatformTypeId = 3, // Microsoft Teams
                JobSourceId = 4, // Company Website
                Status = InterviewStatus.Stage1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Title = "Technical Lead - Java",
                CompanyName = "Global Tech Solutions",
                JobTitle = "Technical Lead",
                DateTime = DateTime.UtcNow.AddDays(13),
                MeetingLink = "https://meet.google.com/tech-lead-java",
                Notes = "Leadership experience with Java, Spring Boot, and team management",
                MeetingPlatformTypeId = 2, // Google Meet
                JobSourceId = 5, // Referral
                Status = InterviewStatus.FinalRound,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Title = "Database Administrator",
                CompanyName = "Data Systems Corp",
                JobTitle = "Senior DBA",
                DateTime = DateTime.UtcNow.AddDays(4),
                MeetingLink = "https://zoom.us/j/777888999",
                Notes = "SQL Server, PostgreSQL, and database optimization",
                MeetingPlatformTypeId = 1, // Zoom
                JobSourceId = 6, // Recruiter
                Status = InterviewStatus.Scheduled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Interview
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Title = "Security Engineer",
                CompanyName = "CyberSec Solutions",
                JobTitle = "Security Engineer",
                DateTime = DateTime.UtcNow.AddDays(14),
                MeetingLink = "https://teams.microsoft.com/l/meetup-join/security",
                Notes = "Penetration testing, security audits, and threat analysis",
                MeetingPlatformTypeId = 3, // Microsoft Teams
                JobSourceId = 1, // LinkedIn
                Status = InterviewStatus.Stage2,
                JobPortalUrl = "https://linkedin.com/jobs/security-engineer",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );

        // Seed sample interviewers (users/contacts)
        builder.Entity<Interviewer>().HasData(
            // Interviewers for Senior Software Engineer Interview
            new Interviewer
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Name = "John Smith",
                Email = "john.smith@acmecorp.com",
                Title = "Engineering Manager",
                InterviewId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            },
            new Interviewer
            {
                Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                Name = "Sarah Johnson",
                Email = "sarah.j@acmecorp.com",
                Title = "Senior Software Engineer",
                InterviewId = Guid.Parse("11111111-1111-1111-1111-111111111111")
            },
            // Interviewers for Product Manager Interview
            new Interviewer
            {
                Id = Guid.Parse("b1111111-1111-1111-1111-111111111111"),
                Name = "Michael Chen",
                Email = "m.chen@betatech.com",
                Title = "VP of Product",
                InterviewId = Guid.Parse("22222222-2222-2222-2222-222222222222")
            },
            // Interviewers for DevOps Engineer Interview
            new Interviewer
            {
                Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"),
                Name = "Emily Rodriguez",
                Email = "emily.r@cloudinnovations.com",
                Title = "DevOps Lead",
                InterviewId = Guid.Parse("33333333-3333-3333-3333-333333333333")
            },
            new Interviewer
            {
                Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"),
                Name = "David Park",
                Email = "d.park@cloudinnovations.com",
                Title = "Cloud Architect",
                InterviewId = Guid.Parse("33333333-3333-3333-3333-333333333333")
            },
            // Interviewers for Frontend Developer Interview
            new Interviewer
            {
                Id = Guid.Parse("f1111111-1111-1111-1111-111111111111"),
                Name = "Lisa Anderson",
                Email = "lisa@techstartup.com",
                Title = "Lead Frontend Developer",
                InterviewId = Guid.Parse("66666666-6666-6666-6666-666666666666")
            },
            // Interviewers for Technical Lead Interview
            new Interviewer
            {
                Id = Guid.Parse("g1111111-1111-1111-1111-111111111111"),
                Name = "Robert Williams",
                Email = "robert.w@globaltech.com",
                Title = "CTO",
                InterviewId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
            },
            new Interviewer
            {
                Id = Guid.Parse("g2222222-2222-2222-2222-222222222222"),
                Name = "Jennifer Lee",
                Email = "jennifer.l@globaltech.com",
                Title = "Senior Engineering Manager",
                InterviewId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
            }
        );
    }
}
