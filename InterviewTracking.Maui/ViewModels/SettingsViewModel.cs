using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;

namespace InterviewTracking.Maui.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IAuthLocalService _authService;
    private readonly ISyncService _syncService;
    private readonly IPreferences _preferences;
    private readonly IInterviewLocalService _interviewService;
    private readonly IDataExportImportService _exportImportService;

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

    [ObservableProperty]
    private bool emailRemindersEnabled;

    [ObservableProperty]
    private string emailReminderRecipient = string.Empty;

    public SettingsViewModel(
        IAuthLocalService authService,
        ISyncService syncService,
        IPreferences preferences,
        IInterviewLocalService interviewService,
        IDataExportImportService exportImportService)
    {
        _authService = authService;
        _syncService = syncService;
        _preferences = preferences;
        _interviewService = interviewService;
        _exportImportService = exportImportService;
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
        EmailRemindersEnabled = _preferences.Get("email_reminders_enabled", false);
        EmailReminderRecipient = _preferences.Get("email_reminder_recipient", string.Empty);
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

    partial void OnEmailRemindersEnabledChanged(bool value)
    {
        _preferences.Set("email_reminders_enabled", value);
    }

    partial void OnEmailReminderRecipientChanged(string value)
    {
        _preferences.Set("email_reminder_recipient", value);
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

    [RelayCommand]
    private async Task ExportDataAsync()
    {
        try
        {
            IsBusy = true;
            
            var interviews = await _interviewService.GetInterviewsAsync();
            var fileName = $"interviews_export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            
            var success = await _exportImportService.ExportToFileAsync(interviews, filePath);
            
            if (success)
            {
                // Share the file
                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Export Interview Data",
                    File = new ShareFile(filePath)
                });
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to export data", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Export failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ImportDataAsync()
    {
        try
        {
            IsBusy = true;
            
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                    { DevicePlatform.WinUI, new[] { ".json" } },
                    { DevicePlatform.macOS, new[] { "json" } }
                }),
                PickerTitle = "Select Interview Data File"
            });

            if (result != null)
            {
                var interviews = await _exportImportService.ImportFromFileAsync(result.FullPath);
                
                int imported = 0;
                int skipped = 0;
                
                // Import interviews into local database
                // Check for existing interviews by ID to avoid duplicates
                foreach (var interview in interviews)
                {
                    var existing = await _interviewService.GetInterviewByIdAsync(interview.Id);
                    if (existing == null)
                    {
                        await _interviewService.CreateInterviewAsync(interview);
                        imported++;
                    }
                    else
                    {
                        skipped++;
                    }
                }
                
                await Shell.Current.DisplayAlert("Success", 
                    $"Imported {imported} interview(s), skipped {skipped} duplicate(s)", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Import failed: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ClearAllDataAsync()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Clear All Data",
            "Are you sure you want to delete ALL interview data? This action cannot be undone.",
            "Yes, Delete All",
            "Cancel");

        if (!confirm) return;

        try
        {
            IsBusy = true;
            
            var success = await _interviewService.ClearAllDataAsync();
            
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "All interview data has been cleared", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to clear data", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to clear data: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ResetToSeedDataAsync()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Reset to Test Data",
            "This will delete all current data and restore the original sample interviews. Continue?",
            "Yes, Reset",
            "Cancel");

        if (!confirm) return;

        try
        {
            IsBusy = true;
            
            var success = await _interviewService.ResetToSeedDataAsync();
            
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", 
                    "Database has been reset with sample data. Please restart the app to see changes.", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to reset database", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to reset database: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
