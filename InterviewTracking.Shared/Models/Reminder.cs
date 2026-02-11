namespace InterviewTracking.Shared.Models;

public class Reminder
{
    public Guid Id { get; set; }
    public Guid InterviewId { get; set; }
    public DateTime ReminderTime { get; set; }
    public bool IsTriggered { get; set; }
    public string Message { get; set; } = string.Empty;
}
