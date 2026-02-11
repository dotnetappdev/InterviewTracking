using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.ViewModels;

[QueryProperty(nameof(InterviewId), "id")]
public partial class InterviewDetailViewModel : BaseViewModel
{
    private readonly IInterviewLocalService _interviewService;

    [ObservableProperty]
    private Interview? interview;

    [ObservableProperty]
    private string interviewId = string.Empty;

    public InterviewDetailViewModel(IInterviewLocalService interviewService)
    {
        _interviewService = interviewService;
        Title = "Interview Details";
    }

    partial void OnInterviewIdChanged(string value)
    {
        if (Guid.TryParse(value, out var id))
        {
            LoadInterviewAsync(id).ConfigureAwait(false);
        }
    }

    [RelayCommand]
    private async Task LoadInterviewAsync(Guid id)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Interview = await _interviewService.GetInterviewByIdAsync(id);
            
            if (Interview != null)
            {
                Title = Interview.Title;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load interview: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditInterviewAsync()
    {
        if (Interview == null) return;
        
        await Shell.Current.GoToAsync($"AddEditInterview?id={Interview.Id}");
    }

    [RelayCommand]
    private async Task DeleteInterviewAsync()
    {
        if (Interview == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete '{Interview.Title}'?",
            "Yes",
            "No");

        if (!confirm) return;

        try
        {
            await _interviewService.DeleteInterviewAsync(Interview.Id);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to delete interview: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task OpenMeetingLinkAsync()
    {
        if (Interview == null || string.IsNullOrWhiteSpace(Interview.MeetingLink))
            return;

        try
        {
            await Browser.Default.OpenAsync(Interview.MeetingLink, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open meeting link: {ex.Message}", "OK");
        }
    }
}
