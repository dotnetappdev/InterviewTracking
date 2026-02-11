namespace InterviewTracking.Maui.Services;

public interface IAuthLocalService
{
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetTokenAsync();
    Task SaveTokenAsync(string token);
    Task<string?> GetUserIdAsync();
    Task SaveUserIdAsync(string userId);
    Task ClearAuthDataAsync();
}

public class AuthLocalService : IAuthLocalService
{
    private readonly IPreferences _preferences;
    private const string TokenKey = "auth_token";
    private const string UserIdKey = "user_id";

    public AuthLocalService(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public Task<bool> IsAuthenticatedAsync()
    {
        var token = _preferences.Get(TokenKey, string.Empty);
        return Task.FromResult(!string.IsNullOrEmpty(token));
    }

    public Task<string?> GetTokenAsync()
    {
        var token = _preferences.Get(TokenKey, string.Empty);
        return Task.FromResult(string.IsNullOrEmpty(token) ? null : token);
    }

    public Task SaveTokenAsync(string token)
    {
        _preferences.Set(TokenKey, token);
        return Task.CompletedTask;
    }

    public Task<string?> GetUserIdAsync()
    {
        var userId = _preferences.Get(UserIdKey, string.Empty);
        return Task.FromResult(string.IsNullOrEmpty(userId) ? null : userId);
    }

    public Task SaveUserIdAsync(string userId)
    {
        _preferences.Set(UserIdKey, userId);
        return Task.CompletedTask;
    }

    public Task ClearAuthDataAsync()
    {
        _preferences.Remove(TokenKey);
        _preferences.Remove(UserIdKey);
        return Task.CompletedTask;
    }
}
