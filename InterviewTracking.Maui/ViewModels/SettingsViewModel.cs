using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;

namespace InterviewTracking.Maui.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IAuthLocalService _authService;
    private readonly ISyncService _syncService;
    private readonly IPreferences _preferences;

    [ObservableProperty]
    private bool darkMode;

    [ObservableProperty]
    private bool notificationsEnabled;

    [ObservableProperty]
    private bool syncEnabled;

    [ObservableProperty]
    private bool autoSync;

    [ObservableProperty]
    private DateTime? lastSyncTime;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string apiUrl = string.Empty;

    [ObservableProperty]
    private bool useApi;

    public SettingsViewModel(
        IAuthLocalService authService,
        ISyncService syncService,
        IPreferences preferences)
    {
        _authService = authService;
        _syncService = syncService;
        _preferences = preferences;
        Title = "Settings";
    }

    public async Task InitializeAsync()
    {
        DarkMode = _preferences.Get("dark_mode", false);
        NotificationsEnabled = _preferences.Get("notifications_enabled", true);
        SyncEnabled = _preferences.Get("sync_enabled", true);
        AutoSync = _preferences.Get("auto_sync", false);
        UseApi = _preferences.Get("use_api", false);
        ApiUrl = _preferences.Get("api_url", "https://localhost:7000/api/");
        LastSyncTime = await _syncService.GetLastSyncTimeAsync();
    }

    partial void OnDarkModeChanged(bool value)
    {
        _preferences.Set("dark_mode", value);
        // Apply theme change
        Application.Current!.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
    }

    partial void OnNotificationsEnabledChanged(bool value)
    {
        _preferences.Set("notifications_enabled", value);
    }

    partial void OnSyncEnabledChanged(bool value)
    {
        _preferences.Set("sync_enabled", value);
    }

    partial void OnAutoSyncChanged(bool value)
    {
        _preferences.Set("auto_sync", value);
    }

    partial void OnUseApiChanged(bool value)
    {
        _preferences.Set("use_api", value);
    }

    partial void OnApiUrlChanged(string value)
    {
        _preferences.Set("api_url", value);
    }

    [RelayCommand]
    private async Task SyncNowAsync()
    {
        if (IsBusy) return;

        if (!UseApi)
        {
            await Shell.Current.DisplayAlert("Info", "API is disabled. Enable API in settings to sync.", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            
            var success = await _syncService.SyncAsync();
            
            if (success)
            {
                LastSyncTime = await _syncService.GetLastSyncTimeAsync();
                await Shell.Current.DisplayAlert("Success", "Sync completed successfully", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Info", "Sync failed. Check your internet connection and API URL.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Sync failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Logout",
            "Are you sure you want to logout?",
            "Yes",
            "No");

        if (!confirm) return;

        await _authService.ClearAuthDataAsync();
        await Shell.Current.GoToAsync("//Login");
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        await Shell.Current.DisplayAlert("Info", "Change password feature coming soon", "OK");
    }

    [RelayCommand]
    private async Task DeleteAccountAsync()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Account Deletion",
            "Are you sure you want to delete your account? This action cannot be undone.",
            "Yes, Delete",
            "Cancel");

        if (!confirm) return;

        await Shell.Current.DisplayAlert("Info", "Account deletion feature coming soon", "OK");
    }
}
