namespace InterviewTracking.Shared.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserSettings Settings { get; set; } = new();
}

public class UserSettings
{
    public bool DarkMode { get; set; }
    public bool NotificationsEnabled { get; set; }
    public List<int> DefaultReminderMinutes { get; set; } = new() { 1440, 60, 10 }; // 1 day, 1 hour, 10 minutes
    public bool SyncEnabled { get; set; }
    public bool AutoSync { get; set; }
    public MeetingPlatform PreferredPlatform { get; set; } = MeetingPlatform.Zoom;
    public bool OpenNativeApp { get; set; } = true;
    public int DefaultInterviewDuration { get; set; } = 60; // minutes
    public DayOfWeek WeekStartDay { get; set; } = DayOfWeek.Monday;
}
