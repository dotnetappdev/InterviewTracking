using System.Net.Http.Json;
using InterviewTracking.Shared.DTOs;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IPreferences _preferences;

    public ApiService(HttpClient httpClient, IPreferences preferences)
    {
        _httpClient = httpClient;
        _preferences = preferences;
    }

    private void EnsureBaseAddress()
    {
        var apiUrl = _preferences.Get("api_url", "https://localhost:7000/api/");
        if (!string.IsNullOrEmpty(apiUrl) && !apiUrl.EndsWith("/"))
        {
            apiUrl += "/";
        }
        
        // Only update if different
        if (_httpClient.BaseAddress?.ToString() != apiUrl)
        {
            _httpClient.BaseAddress = new Uri(apiUrl);
        }
    }

    private bool IsApiEnabled()
    {
        return _preferences.Get("use_api", false);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (!IsApiEnabled())
        {
            return new AuthResponse { Success = false, Message = "API is disabled in settings" };
        }

        try
        {
            EnsureBaseAddress();
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
        if (!IsApiEnabled())
        {
            return new AuthResponse { Success = false, Message = "API is disabled in settings" };
        }

        try
        {
            EnsureBaseAddress();
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
        if (!IsApiEnabled())
        {
            return Array.Empty<Interview>();
        }

        try
        {
            EnsureBaseAddress();
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
        if (!IsApiEnabled())
        {
            return null;
        }

        try
        {
            EnsureBaseAddress();
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
        if (!IsApiEnabled())
        {
            return interview;
        }

        EnsureBaseAddress();
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PostAsJsonAsync("interviews", interview);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Interview>() 
            ?? interview;
    }

    public async Task<Interview?> UpdateInterviewAsync(Interview interview, string token)
    {
        if (!IsApiEnabled())
        {
            return interview;
        }

        EnsureBaseAddress();
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await _httpClient.PutAsJsonAsync($"interviews/{interview.Id}", interview);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Interview>();
    }

    public async Task<bool> DeleteInterviewAsync(Guid id, string token)
    {
        if (!IsApiEnabled())
        {
            return false;
        }

        try
        {
            EnsureBaseAddress();
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
