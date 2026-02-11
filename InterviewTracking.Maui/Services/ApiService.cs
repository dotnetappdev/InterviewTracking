using System.Net.Http.Json;
using InterviewTracking.Shared.DTOs;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>() 
                ?? new AuthResponse { Success = false, Message = "Failed to deserialize response" };
        }
        catch (Exception ex)
        {
            return new AuthResponse { Success = false, Message = ex.Message };
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>() 
                ?? new AuthResponse { Success = false, Message = "Failed to deserialize response" };
        }
        catch (Exception ex)
        {
            return new AuthResponse { Success = false, Message = ex.Message };
        }
    }

    public async Task<IEnumerable<Interview>> GetInterviewsAsync(string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.GetAsync("interviews");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<IEnumerable<Interview>>() 
                ?? Array.Empty<Interview>();
        }
        catch
        {
            return Array.Empty<Interview>();
        }
    }

    public async Task<Interview?> GetInterviewAsync(Guid id, string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.GetAsync($"interviews/{id}");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<Interview>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<Interview> CreateInterviewAsync(Interview interview, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsJsonAsync("interviews", interview);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Interview>() 
            ?? interview;
    }

    public async Task<Interview?> UpdateInterviewAsync(Interview interview, string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PutAsJsonAsync($"interviews/{interview.Id}", interview);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Interview>();
    }

    public async Task<bool> DeleteInterviewAsync(Guid id, string token)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.DeleteAsync($"interviews/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
