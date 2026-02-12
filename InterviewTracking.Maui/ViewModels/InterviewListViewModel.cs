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

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CompanyGroup> companiesGroupedAZ = new();

    [ObservableProperty]
    private bool isSearchMode = false;

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
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load interviews: {ex.Message}", "OK");
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
                await Shell.Current.DisplayAlertAsync("Success", "Sync completed successfully", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlertAsync("Info", "Sync failed. Check your internet connection.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Sync failed: {ex.Message}", "OK");
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

        bool confirm = await Shell.Current.DisplayAlertAsync(
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
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete interview: {ex.Message}", "OK");
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
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to open meeting link: {ex.Message}", "OK");
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

    [RelayCommand]
    private async Task CopyToClipboardAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            await Shell.Current.DisplayAlertAsync("Info", "Nothing to copy", "OK");
            return;
        }

        try
        {
            await Clipboard.Default.SetTextAsync(text);
            await Shell.Current.DisplayAlertAsync("Success", "Copied to clipboard", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to copy: {ex.Message}", "OK");
        }
    }

    public async Task InitializeAsync()
    {
        await LoadInterviewsAsync();
    }

    [RelayCommand]
    private void SearchCompany()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // Reset to filtered interviews
            IsSearchMode = false;
            return;
        }

        IsSearchMode = true;
        FilteredInterviews.Clear();

        var searchResults = UpcomingInterviews
            .Where(i => !string.IsNullOrEmpty(i.CompanyName) && 
                        i.CompanyName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(i => i.CompanyName)
            .ThenBy(i => i.DateTime)
            .ToList();

        foreach (var interview in searchResults)
        {
            FilteredInterviews.Add(interview);
        }

        InterviewListTitle = searchResults.Any() 
            ? $"Search Results ({searchResults.Count})"
            : "No Results Found";
    }

    [RelayCommand]
    private void ClearSearch()
    {
        SearchText = string.Empty;
        IsSearchMode = false;
        
        // Reset to all upcoming interviews
        FilteredInterviews.Clear();
        foreach (var interview in UpcomingInterviews)
        {
            FilteredInterviews.Add(interview);
        }
        InterviewListTitle = "All Upcoming Interviews";
    }

    [RelayCommand]
    private void LoadCompaniesAZ()
    {
        CompaniesGroupedAZ.Clear();

        // Get distinct companies from all interviews
        var companies = Interviews
            .Where(i => !string.IsNullOrEmpty(i.CompanyName))
            .Select(i => i.CompanyName)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        // Group by first letter
        var grouped = companies
            .GroupBy(c => char.ToUpper(c[0]))
            .OrderBy(g => g.Key);

        foreach (var group in grouped)
        {
            var companyGroup = new CompanyGroup(group.Key.ToString());
            
            foreach (var companyName in group)
            {
                var companyInterviews = Interviews
                    .Where(i => i.CompanyName == companyName)
                    .OrderBy(i => i.DateTime)
                    .ToList();

                companyGroup.Add(new CompanyItem
                {
                    CompanyName = companyName,
                    InterviewCount = companyInterviews.Count,
                    Interviews = companyInterviews
                });
            }

            CompaniesGroupedAZ.Add(companyGroup);
        }
    }

    [RelayCommand]
    private void FilterByCompany(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName)) return;

        IsSearchMode = true;
        FilteredInterviews.Clear();

        var companyInterviews = UpcomingInterviews
            .Where(i => i.CompanyName == companyName)
            .OrderBy(i => i.DateTime)
            .ToList();

        foreach (var interview in companyInterviews)
        {
            FilteredInterviews.Add(interview);
        }

        InterviewListTitle = $"{companyName} ({companyInterviews.Count})";
    }
}

// Helper classes for company grouping
public class CompanyGroup : ObservableCollection<CompanyItem>
{
    public string Letter { get; set; }

    public CompanyGroup(string letter)
    {
        Letter = letter;
    }
}

public class CompanyItem
{
    public string CompanyName { get; set; } = string.Empty;
    public int InterviewCount { get; set; }
    public List<Interview> Interviews { get; set; } = new();
}
