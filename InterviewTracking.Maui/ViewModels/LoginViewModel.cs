using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;
using InterviewTracking.Shared.DTOs;

namespace InterviewTracking.Maui.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthLocalService _authService;
    private readonly IPreferences _preferences;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public LoginViewModel(IApiService apiService, IAuthLocalService authService, IPreferences preferences)
    {
        _apiService = apiService;
        _authService = authService;
        _preferences = preferences;
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy) return;

        // Check if API is enabled
        var useApi = _preferences.Get("use_api", false);
        
        if (!useApi)
        {
            // Skip login when API is disabled, go directly to main page
            await Shell.Current.GoToAsync("///InterviewList");
            return;
        }

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter email and password";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var request = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var response = await _apiService.LoginAsync(request);

            if (response.Success)
            {
                await _authService.SaveTokenAsync(response.Token);
                await _authService.SaveUserIdAsync(response.UserId);
                
                // Navigate to main page
                await Shell.Current.GoToAsync("///InterviewList");
            }
            else
            {
                ErrorMessage = response.Message;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToRegisterAsync()
    {
        await Shell.Current.GoToAsync("//Register");
    }

    [RelayCommand]
    private async Task SkipLoginAsync()
    {
        // Allow users to skip login and use the app in offline mode
        await Shell.Current.GoToAsync("///InterviewList");
    }
}
