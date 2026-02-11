namespace InterviewTracking.Shared.Models;

public class Interview
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public MeetingPlatform Platform { get; set; }
    public string MeetingLink { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    
    // Recurrence properties
    public bool IsRecurring { get; set; }
    public RecurrencePattern? RecurrencePattern { get; set; }
    public DateTime? RecurrenceEndDate { get; set; }
    
    // Navigation properties
    public List<Interviewer> Interviewers { get; set; } = new();
    public List<Reminder> Reminders { get; set; } = new();
    
    // Sync properties
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsSynced { get; set; }
}

public enum MeetingPlatform
{
    Zoom,
    GoogleMeet,
    MicrosoftTeams,
    Other
}

public enum RecurrencePattern
{
    Daily,
    Weekly,
    BiWeekly,
    Monthly,
    Custom
}
