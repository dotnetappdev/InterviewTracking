using InterviewTracking.Shared.DTOs;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public interface IApiService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<IEnumerable<Interview>> GetInterviewsAsync(string token);
    Task<Interview?> GetInterviewAsync(Guid id, string token);
    Task<Interview> CreateInterviewAsync(Interview interview, string token);
    Task<Interview?> UpdateInterviewAsync(Interview interview, string token);
    Task<bool> DeleteInterviewAsync(Guid id, string token);
}
