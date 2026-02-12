namespace InterviewTracking.Shared.Models;

public class InterviewAttachment
{
    public Guid Id { get; set; }
    public Guid InterviewId { get; set; }
    
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileType { get; set; } = string.Empty;
    public AttachmentType Type { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime UploadedAt { get; set; }
}

public enum AttachmentType
{
    Resume,
    CoverLetter,
    JobDescription,
    Other
}
