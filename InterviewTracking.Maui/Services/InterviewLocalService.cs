using Microsoft.EntityFrameworkCore;
using InterviewTracking.Maui.Data;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public class InterviewLocalService : IInterviewLocalService
{
    private readonly LocalDbContext _context;

    public InterviewLocalService(LocalDbContext context)
    {
        _context = context;
        InitializeDatabaseAsync().Wait();
    }

    private async Task InitializeDatabaseAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task<IEnumerable<Interview>> GetInterviewsAsync()
    {
        return await _context.Interviews
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .Include(i => i.MeetingPlatformType)
            .OrderBy(i => i.DateTime)
            .ToListAsync();
    }

    public async Task<Interview?> GetInterviewByIdAsync(Guid id)
    {
        return await _context.Interviews
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .Include(i => i.MeetingPlatformType)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Interview> CreateInterviewAsync(Interview interview)
    {
        interview.Id = Guid.NewGuid();
        interview.CreatedAt = DateTime.UtcNow;
        interview.UpdatedAt = DateTime.UtcNow;
        interview.IsSynced = false;

        // Ensure MeetingPlatformTypeId is set if not provided
        if (interview.MeetingPlatformTypeId == 0)
        {
            interview.MeetingPlatformTypeId = 1; // Default to Zoom
        }

        _context.Interviews.Add(interview);
        await _context.SaveChangesAsync();

        return interview;
    }

    public async Task<Interview?> UpdateInterviewAsync(Interview interview)
    {
        var existingInterview = await _context.Interviews
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .Include(i => i.MeetingPlatformType)
            .FirstOrDefaultAsync(i => i.Id == interview.Id);

        if (existingInterview == null)
            return null;

        existingInterview.Title = interview.Title;
        existingInterview.DateTime = interview.DateTime;
        existingInterview.Notes = interview.Notes;
        existingInterview.MeetingPlatformTypeId = interview.MeetingPlatformTypeId;
        existingInterview.MeetingLink = interview.MeetingLink;
        existingInterview.IsRecurring = interview.IsRecurring;
        existingInterview.RecurrencePattern = interview.RecurrencePattern;
        existingInterview.RecurrenceEndDate = interview.RecurrenceEndDate;
        existingInterview.UpdatedAt = DateTime.UtcNow;
        existingInterview.IsSynced = false;

        // Update interviewers
        _context.Interviewers.RemoveRange(existingInterview.Interviewers);
        existingInterview.Interviewers = interview.Interviewers;

        // Update reminders
        _context.Reminders.RemoveRange(existingInterview.Reminders);
        existingInterview.Reminders = interview.Reminders;

        await _context.SaveChangesAsync();

        return existingInterview;
    }

    public async Task<bool> DeleteInterviewAsync(Guid id)
    {
        var interview = await _context.Interviews.FindAsync(id);
        if (interview == null)
            return false;

        _context.Interviews.Remove(interview);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Interview>> GetUpcomingInterviewsAsync()
    {
        var now = DateTime.Now;
        return await _context.Interviews
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .Include(i => i.MeetingPlatformType)
            .Where(i => i.DateTime >= now)
            .OrderBy(i => i.DateTime)
            .ToListAsync();
    }
}
