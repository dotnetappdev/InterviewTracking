using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public interface IInterviewLocalService
{
    Task<IEnumerable<Interview>> GetInterviewsAsync();
    Task<Interview?> GetInterviewByIdAsync(Guid id);
    Task<Interview> CreateInterviewAsync(Interview interview);
    Task<Interview?> UpdateInterviewAsync(Interview interview);
    Task<bool> DeleteInterviewAsync(Guid id);
    Task<IEnumerable<Interview>> GetUpcomingInterviewsAsync();
}
