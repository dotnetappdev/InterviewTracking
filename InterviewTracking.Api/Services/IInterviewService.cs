using InterviewTracking.Shared.Models;

namespace InterviewTracking.Api.Services;

public interface IInterviewService
{
    Task<IEnumerable<Interview>> GetInterviewsAsync(string userId);
    Task<Interview?> GetInterviewByIdAsync(Guid id, string userId);
    Task<Interview> CreateInterviewAsync(Interview interview);
    Task<Interview?> UpdateInterviewAsync(Interview interview, string userId);
    Task<bool> DeleteInterviewAsync(Guid id, string userId);
}
