namespace InterviewTracking.Shared.Models;

public class InterviewFeedback
{
    public Guid Id { get; set; }
    public Guid InterviewId { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    
    // Scoring (1-5 scale)
    public int? TechnicalSkillsScore { get; set; }
    public int? CommunicationScore { get; set; }
    public int? ProblemSolvingScore { get; set; }
    public int? CulturalFitScore { get; set; }
    public int? OverallScore { get; set; }
    
    // Feedback comments
    public string Strengths { get; set; } = string.Empty;
    public string Weaknesses { get; set; } = string.Empty;
    public string GeneralComments { get; set; } = string.Empty;
    
    // Recommendation
    public FeedbackRecommendation Recommendation { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum FeedbackRecommendation
{
    StrongYes,
    Yes,
    Maybe,
    No,
    StrongNo
}
