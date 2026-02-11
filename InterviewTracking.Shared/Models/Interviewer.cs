namespace InterviewTracking.Shared.Models;

public class Interviewer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    
    public Guid InterviewId { get; set; }
}
