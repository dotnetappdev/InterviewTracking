using Microsoft.EntityFrameworkCore;
using InterviewTracking.Api.Data;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Api.Services;

public class InterviewService : IInterviewService
{
    private readonly ApplicationDbContext _context;

    public InterviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interview>> GetInterviewsAsync(string userId)
    {
        return await _context.Interviews
            .Where(i => i.UserId == userId)
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .OrderBy(i => i.DateTime)
            .ToListAsync();
    }

    public async Task<Interview?> GetInterviewByIdAsync(Guid id, string userId)
    {
        return await _context.Interviews
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
    }

    public async Task<Interview> CreateInterviewAsync(Interview interview)
    {
        interview.Id = Guid.NewGuid();
        interview.CreatedAt = DateTime.UtcNow;
        interview.UpdatedAt = DateTime.UtcNow;
        interview.IsSynced = true;

        _context.Interviews.Add(interview);
        await _context.SaveChangesAsync();

        return interview;
    }

    public async Task<Interview?> UpdateInterviewAsync(Interview interview, string userId)
    {
        var existingInterview = await _context.Interviews
            .Include(i => i.Interviewers)
            .Include(i => i.Reminders)
            .FirstOrDefaultAsync(i => i.Id == interview.Id && i.UserId == userId);

        if (existingInterview == null)
            return null;

        existingInterview.Title = interview.Title;
        existingInterview.DateTime = interview.DateTime;
        existingInterview.Notes = interview.Notes;
        existingInterview.Platform = interview.Platform;
        existingInterview.MeetingLink = interview.MeetingLink;
        existingInterview.IsRecurring = interview.IsRecurring;
        existingInterview.RecurrencePattern = interview.RecurrencePattern;
        existingInterview.RecurrenceEndDate = interview.RecurrenceEndDate;
        existingInterview.UpdatedAt = DateTime.UtcNow;

        // Update interviewers
        existingInterview.Interviewers.Clear();
        existingInterview.Interviewers.AddRange(interview.Interviewers);

        // Update reminders
        existingInterview.Reminders.Clear();
        existingInterview.Reminders.AddRange(interview.Reminders);

        await _context.SaveChangesAsync();

        return existingInterview;
    }

    public async Task<bool> DeleteInterviewAsync(Guid id, string userId)
    {
        var interview = await _context.Interviews
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

        if (interview == null)
            return false;

        _context.Interviews.Remove(interview);
        await _context.SaveChangesAsync();

        return true;
    }
}
