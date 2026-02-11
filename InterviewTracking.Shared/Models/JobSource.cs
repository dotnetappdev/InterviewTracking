namespace InterviewTracking.Shared.Models;

public class JobSource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Navigation property
    public List<Interview> Interviews { get; set; } = new();
}
