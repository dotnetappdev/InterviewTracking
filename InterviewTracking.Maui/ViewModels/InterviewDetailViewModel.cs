using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.ViewModels;

[QueryProperty(nameof(InterviewId), "id")]
public partial class InterviewDetailViewModel : BaseViewModel
{
    private readonly IInterviewLocalService _interviewService;
    private readonly ICalendarExportService _calendarExportService;

    [ObservableProperty]
    private Interview? interview;

    [ObservableProperty]
    private string interviewId = string.Empty;

    public InterviewDetailViewModel(
        IInterviewLocalService interviewService,
        ICalendarExportService calendarExportService)
    {
        _interviewService = interviewService;
        _calendarExportService = calendarExportService;
        Title = "Interview Details";
    }

    partial void OnInterviewIdChanged(string value)
    {
        if (Guid.TryParse(value, out var id))
        {
            _ = LoadInterviewAsync(id);
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

    [RelayCommand]
    private async Task ExportToCalendarAsync()
    {
        if (Interview == null) return;

        try
        {
            var success = await _calendarExportService.ExportToDeviceCalendarAsync(Interview);
            
            if (success)
            {
                await Shell.Current.DisplayAlertAsync("Success", "Interview exported to calendar", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Error", "Failed to export to calendar", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to export: {ex.Message}", "OK");
        }
    }
}
