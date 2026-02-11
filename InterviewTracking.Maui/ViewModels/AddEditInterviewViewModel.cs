using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InterviewTracking.Maui.Services;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.ViewModels;

[QueryProperty(nameof(InterviewId), "id")]
public partial class AddEditInterviewViewModel : BaseViewModel
{
    private readonly IInterviewLocalService _interviewService;
    private readonly INotificationService _notificationService;

    [ObservableProperty]
    private string interviewId = string.Empty;

    [ObservableProperty]
    private string interviewTitle = string.Empty;

    [ObservableProperty]
    private DateTime interviewDate = DateTime.Now.AddDays(1);

    [ObservableProperty]
    private TimeSpan interviewTime = TimeSpan.FromHours(9);

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private MeetingPlatform selectedPlatform = MeetingPlatform.Zoom;

    [ObservableProperty]
    private string meetingLink = string.Empty;

    [ObservableProperty]
    private bool isRecurring;

    [ObservableProperty]
    private RecurrencePattern? recurrencePattern;

    [ObservableProperty]
    private ObservableCollection<Interviewer> interviewers = new();

    [ObservableProperty]
    private bool isEditMode;

    private Interview? _originalInterview;

    public ObservableCollection<MeetingPlatform> Platforms { get; } = new()
    {
        MeetingPlatform.Zoom,
        MeetingPlatform.GoogleMeet,
        MeetingPlatform.MicrosoftTeams,
        MeetingPlatform.Other
    };

    public AddEditInterviewViewModel(IInterviewLocalService interviewService, INotificationService notificationService)
    {
        _interviewService = interviewService;
        _notificationService = notificationService;
        Title = "Add Interview";
    }

    partial void OnInterviewIdChanged(string value)
    {
        if (Guid.TryParse(value, out var id) && id != Guid.Empty)
        {
            IsEditMode = true;
            Title = "Edit Interview";
            LoadInterviewAsync(id).ConfigureAwait(false);
        }
    }

    private async Task LoadInterviewAsync(Guid id)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            _originalInterview = await _interviewService.GetInterviewByIdAsync(id);
            
            if (_originalInterview != null)
            {
                InterviewTitle = _originalInterview.Title;
                InterviewDate = _originalInterview.DateTime.Date;
                InterviewTime = _originalInterview.DateTime.TimeOfDay;
                Notes = _originalInterview.Notes;
                SelectedPlatform = _originalInterview.Platform;
                MeetingLink = _originalInterview.MeetingLink;
                IsRecurring = _originalInterview.IsRecurring;
                RecurrencePattern = _originalInterview.RecurrencePattern;
                
                Interviewers.Clear();
                foreach (var interviewer in _originalInterview.Interviewers)
                {
                    Interviewers.Add(interviewer);
                }
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
    private async Task SaveInterviewAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(InterviewTitle))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Please enter an interview title", "OK");
            return;
        }

        try
        {
            IsBusy = true;

            var interview = new Interview
            {
                Id = IsEditMode && _originalInterview != null ? _originalInterview.Id : Guid.NewGuid(),
                Title = InterviewTitle,
                DateTime = InterviewDate.Date + InterviewTime,
                Notes = Notes,
                Platform = SelectedPlatform,
                MeetingLink = MeetingLink,
                IsRecurring = IsRecurring,
                RecurrencePattern = RecurrencePattern,
                Interviewers = Interviewers.ToList()
            };

            if (IsEditMode && _originalInterview != null)
            {
                await _interviewService.UpdateInterviewAsync(interview);
            }
            else
            {
                await _interviewService.CreateInterviewAsync(interview);
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to save interview: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task AddInterviewerAsync()
    {
        string name = await Shell.Current.DisplayPromptAsync("Add Interviewer", "Enter interviewer name:");
        if (!string.IsNullOrWhiteSpace(name))
        {
            Interviewers.Add(new Interviewer { Id = Guid.NewGuid(), Name = name });
        }
    }

    [RelayCommand]
    private void RemoveInterviewer(Interviewer interviewer)
    {
        if (interviewer != null)
        {
            Interviewers.Remove(interviewer);
        }
    }
}
