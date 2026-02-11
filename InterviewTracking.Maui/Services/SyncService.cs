namespace InterviewTracking.Maui.Services;

public interface ISyncService
{
    Task<bool> SyncAsync();
    Task<DateTime?> GetLastSyncTimeAsync();
}

public class SyncService : ISyncService
{
    private readonly IApiService _apiService;
    private readonly IInterviewLocalService _localService;
    private readonly IAuthLocalService _authService;
    private readonly IConnectivity _connectivity;
    private readonly IPreferences _preferences;
    private const string LastSyncKey = "last_sync";

    public SyncService(
        IApiService apiService,
        IInterviewLocalService localService,
        IAuthLocalService authService,
        IConnectivity connectivity,
        IPreferences preferences)
    {
        _apiService = apiService;
        _localService = localService;
        _authService = authService;
        _connectivity = connectivity;
        _preferences = preferences;
    }

    public async Task<bool> SyncAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            return false;

        var token = await _authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
            return false;

        try
        {
            // Get remote interviews
            var remoteInterviews = await _apiService.GetInterviewsAsync(token);
            var localInterviews = await _localService.GetInterviewsAsync();

            // Simple sync: just update local with remote data
            // In a real app, you'd want conflict resolution
            foreach (var remoteInterview in remoteInterviews)
            {
                var localInterview = localInterviews.FirstOrDefault(i => i.Id == remoteInterview.Id);
                if (localInterview == null)
                {
                    await _localService.CreateInterviewAsync(remoteInterview);
                }
                else
                {
                    await _localService.UpdateInterviewAsync(remoteInterview);
                }
            }

            // Push local unsync'd changes to remote
            var unsyncedInterviews = localInterviews.Where(i => !i.IsSynced);
            foreach (var interview in unsyncedInterviews)
            {
                await _apiService.CreateInterviewAsync(interview, token);
            }

            _preferences.Set(LastSyncKey, DateTime.UtcNow.ToString("o"));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<DateTime?> GetLastSyncTimeAsync()
    {
        var lastSyncStr = _preferences.Get(LastSyncKey, string.Empty);
        if (string.IsNullOrEmpty(lastSyncStr))
            return Task.FromResult<DateTime?>(null);

        if (DateTime.TryParse(lastSyncStr, out var lastSync))
            return Task.FromResult<DateTime?>(lastSync);

        return Task.FromResult<DateTime?>(null);
    }
}
