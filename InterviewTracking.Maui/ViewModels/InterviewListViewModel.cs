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
    private readonly ICalendarExportService _calendarExportService;

    [ObservableProperty]
    private ObservableCollection<Interview> interviews = new();

    [ObservableProperty]
    private ObservableCollection<Interview> upcomingInterviews = new();

    [ObservableProperty]
    private ObservableCollection<Interview> filteredInterviews = new();

    [ObservableProperty]
    private DateTime? selectedDate = DateTime.Today;

    [ObservableProperty]
    private string interviewListTitle = "All Upcoming Interviews";

    [ObservableProperty]
    private bool isRefreshing;

    public InterviewListViewModel(
        IInterviewLocalService interviewService, 
        ISyncService syncService,
        ICalendarExportService calendarExportService)
    {
        _interviewService = interviewService;
        _syncService = syncService;
        _calendarExportService = calendarExportService;
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

            // Initialize filtered list with all upcoming interviews
            FilteredInterviews.Clear();
            foreach (var interview in upcoming)
            {
                FilteredInterviews.Add(interview);
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

    public void FilterInterviewsByDate(DateTime date)
    {
        SelectedDate = date;
        FilteredInterviews.Clear();

        // Filter interviews for the selected date
        var interviewsOnDate = UpcomingInterviews
            .Where(i => i.DateTime.Date == date.Date)
            .OrderBy(i => i.DateTime.TimeOfDay)
            .ToList();

        if (interviewsOnDate.Any())
        {
            InterviewListTitle = $"Interviews on {date:MMM dd, yyyy} ({interviewsOnDate.Count})";
            foreach (var interview in interviewsOnDate)
            {
                FilteredInterviews.Add(interview);
            }
        }
        else
        {
            InterviewListTitle = $"No interviews on {date:MMM dd, yyyy}";
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

    [RelayCommand]
    private async Task ExportToCalendarAsync(Interview interview)
    {
        if (interview == null) return;

        try
        {
            var success = await _calendarExportService.ExportToDeviceCalendarAsync(interview);
            
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

    public async Task InitializeAsync()
    {
        await LoadInterviewsAsync();
    }
}
