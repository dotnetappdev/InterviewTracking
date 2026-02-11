using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.ViewModels;

public partial class InterviewListViewModel : BaseViewModel
{
    private readonly IInterviewLocalService _interviewService;
    private readonly ISyncService _syncService;

    [ObservableProperty]
    private ObservableCollection<Interview> interviews = new();

    [ObservableProperty]
    private ObservableCollection<Interview> upcomingInterviews = new();

    [ObservableProperty]
    private bool isRefreshing;

    public InterviewListViewModel(IInterviewLocalService interviewService, ISyncService syncService)
    {
        _interviewService = interviewService;
        _syncService = syncService;
        Title = "Interviews";
    }

    [RelayCommand]
    private async Task LoadInterviewsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            
            var allInterviews = await _interviewService.GetInterviewsAsync();
            Interviews.Clear();
            foreach (var interview in allInterviews)
            {
                Interviews.Add(interview);
            }

            var upcoming = await _interviewService.GetUpcomingInterviewsAsync();
            UpcomingInterviews.Clear();
            foreach (var interview in upcoming)
            {
                UpcomingInterviews.Add(interview);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to load interviews: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    private async Task SyncInterviewsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            
            var success = await _syncService.SyncAsync();
            
            if (success)
            {
                await LoadInterviewsAsync();
                await Shell.Current.DisplayAlert("Success", "Sync completed successfully", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Info", "Sync failed. Check your internet connection.", "OK");
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
    private async Task NavigateToAddInterviewAsync()
    {
        await Shell.Current.GoToAsync("AddEditInterview");
    }

    [RelayCommand]
    private async Task NavigateToInterviewDetailAsync(Interview interview)
    {
        if (interview == null) return;
        
        await Shell.Current.GoToAsync($"InterviewDetail?id={interview.Id}");
    }

    [RelayCommand]
    private async Task DeleteInterviewAsync(Interview interview)
    {
        if (interview == null) return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Confirm Delete",
            $"Are you sure you want to delete '{interview.Title}'?",
            "Yes",
            "No");

        if (!confirm) return;

        try
        {
            await _interviewService.DeleteInterviewAsync(interview.Id);
            Interviews.Remove(interview);
            UpcomingInterviews.Remove(interview);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to delete interview: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task OpenMeetingLinkAsync(Interview interview)
    {
        if (interview == null || string.IsNullOrWhiteSpace(interview.MeetingLink))
            return;

        try
        {
            await Browser.Default.OpenAsync(interview.MeetingLink, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to open meeting link: {ex.Message}", "OK");
        }
    }

    public async Task InitializeAsync()
    {
        await LoadInterviewsAsync();
    }
}
