using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;
using InterviewTracking.Shared.DTOs;

namespace InterviewTracking.Maui.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthLocalService _authService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public RegisterViewModel(IApiService apiService, IAuthLocalService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "Register";
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            ErrorMessage = "Please fill in all fields";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match";
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var request = new RegisterRequest
            {
                Email = Email,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName
            };

            var response = await _apiService.RegisterAsync(request);

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
            ErrorMessage = $"Registration failed: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task NavigateToLoginAsync()
    {
        await Shell.Current.GoToAsync("//Login");
    }
}
