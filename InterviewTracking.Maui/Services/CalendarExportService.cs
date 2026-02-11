using InterviewTracking.Shared.Models;
using System.Text;

namespace InterviewTracking.Maui.Services;

public class CalendarExportService : ICalendarExportService
{
    public async Task<bool> ExportToDeviceCalendarAsync(Interview interview)
    {
        try
        {
            // Generate ICS content
            var icsContent = GenerateIcsContent(interview);
            
            // Save to temporary file
            var fileName = $"interview_{interview.Id}.ics";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            await File.WriteAllTextAsync(filePath, icsContent);
            
            // Open with default calendar app
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath),
                Title = "Add to Calendar"
            });
            
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Calendar export failed: {ex.Message}");
            return false;
        }
    }

    public Task<string> GenerateIcsFileAsync(Interview interview)
    {
        return Task.FromResult(GenerateIcsContent(interview));
    }

    public async Task<bool> SaveIcsFileAsync(Interview interview, string filePath)
    {
        try
        {
            var icsContent = GenerateIcsContent(interview);
            await File.WriteAllTextAsync(filePath, icsContent);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateIcsContent(Interview interview)
    {
        var sb = new StringBuilder();
        
        // ICS file header
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:-//Interview Tracking//Interview Scheduler//EN");
        sb.AppendLine("CALSCALE:GREGORIAN");
        sb.AppendLine("METHOD:PUBLISH");
        
        // Event
        sb.AppendLine("BEGIN:VEVENT");
        
        // Unique ID
        sb.AppendLine($"UID:{interview.Id}@interviewtracking.app");
        
        // Date/Time - Convert to UTC and format as YYYYMMDDTHHMMSSZ
        var startTime = interview.DateTime.ToUniversalTime();
        var endTime = startTime.AddHours(1); // Default 1 hour duration
        
        sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmss}Z");
        sb.AppendLine($"DTSTART:{startTime:yyyyMMddTHHmmss}Z");
        sb.AppendLine($"DTEND:{endTime:yyyyMMddTHHmmss}Z");
        
        // Summary (Title)
        sb.AppendLine($"SUMMARY:{EscapeIcsText(interview.Title)}");
        
        // Description with meeting details
        var description = BuildDescription(interview);
        sb.AppendLine($"DESCRIPTION:{EscapeIcsText(description)}");
        
        // Location (Meeting link)
        if (!string.IsNullOrWhiteSpace(interview.MeetingLink))
        {
            sb.AppendLine($"LOCATION:{EscapeIcsText(interview.MeetingLink)}");
        }
        
        // Attendees
        if (interview.Interviewers != null && interview.Interviewers.Any())
        {
            foreach (var interviewer in interview.Interviewers)
            {
                if (!string.IsNullOrWhiteSpace(interviewer.Email))
                {
                    sb.AppendLine($"ATTENDEE;CN={EscapeIcsText(interviewer.Name)}:mailto:{interviewer.Email}");
                }
            }
        }
        
        // Reminders/Alarms
        if (interview.Reminders != null && interview.Reminders.Any())
        {
            foreach (var reminder in interview.Reminders.Take(3)) // Limit to 3 reminders
            {
                var minutesBefore = (interview.DateTime - reminder.ReminderTime).TotalMinutes;
                if (minutesBefore > 0)
                {
                    sb.AppendLine("BEGIN:VALARM");
                    sb.AppendLine("ACTION:DISPLAY");
                    sb.AppendLine($"DESCRIPTION:{EscapeIcsText(reminder.Message ?? interview.Title)}");
                    sb.AppendLine($"TRIGGER:-PT{(int)minutesBefore}M");
                    sb.AppendLine("END:VALARM");
                }
            }
        }
        else
        {
            // Default reminder - 15 minutes before
            sb.AppendLine("BEGIN:VALARM");
            sb.AppendLine("ACTION:DISPLAY");
            sb.AppendLine($"DESCRIPTION:{EscapeIcsText(interview.Title)}");
            sb.AppendLine("TRIGGER:-PT15M");
            sb.AppendLine("END:VALARM");
        }
        
        sb.AppendLine("END:VEVENT");
        sb.AppendLine("END:VCALENDAR");
        
        return sb.ToString();
    }

    private string BuildDescription(Interview interview)
    {
        var sb = new StringBuilder();
        
        // Interview notes
        if (!string.IsNullOrWhiteSpace(interview.Notes))
        {
            sb.AppendLine(interview.Notes);
            sb.AppendLine();
        }
        
        // Meeting platform
        if (interview.MeetingPlatformType != null)
        {
            sb.AppendLine($"Platform: {interview.MeetingPlatformType.Name}");
        }
        
        // Meeting link
        if (!string.IsNullOrWhiteSpace(interview.MeetingLink))
        {
            sb.AppendLine($"Meeting Link: {interview.MeetingLink}");
        }
        
        // Interviewers
        if (interview.Interviewers != null && interview.Interviewers.Any())
        {
            sb.AppendLine();
            sb.AppendLine("Interviewers:");
            foreach (var interviewer in interview.Interviewers)
            {
                sb.AppendLine($"- {interviewer.Name}" + 
                    (!string.IsNullOrWhiteSpace(interviewer.Email) ? $" ({interviewer.Email})" : ""));
            }
        }
        
        return sb.ToString();
    }

    private string EscapeIcsText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        
        // Escape special characters for ICS format
        return text
            .Replace("\\", "\\\\")
            .Replace(",", "\\,")
            .Replace(";", "\\;")
            .Replace("\n", "\\n")
            .Replace("\r", "");
    }
}
