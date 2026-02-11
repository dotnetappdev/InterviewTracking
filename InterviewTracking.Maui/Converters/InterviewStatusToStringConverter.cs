using System.Globalization;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Converters;

public class InterviewStatusToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is InterviewStatus status)
        {
            return status switch
            {
                InterviewStatus.Scheduled => "Scheduled",
                InterviewStatus.Stage1 => "Stage 1",
                InterviewStatus.Stage2 => "Stage 2",
                InterviewStatus.Stage3 => "Stage 3",
                InterviewStatus.Stage4 => "Stage 4",
                InterviewStatus.Stage5 => "Stage 5",
                InterviewStatus.FinalRound => "Final Round",
                InterviewStatus.Completed => "Completed",
                InterviewStatus.NotInterviewed => "Not Interviewed",
                InterviewStatus.DidNotShowUp => "Didn't Show Up",
                InterviewStatus.Cancelled => "Cancelled",
                InterviewStatus.Rejected => "Rejected",
                InterviewStatus.OfferReceived => "Offer Received",
                _ => status.ToString()
            };
        }
        return value?.ToString() ?? string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
