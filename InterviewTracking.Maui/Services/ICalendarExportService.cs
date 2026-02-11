using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public interface ICalendarExportService
{
    Task<bool> ExportToDeviceCalendarAsync(Interview interview);
    Task<string> GenerateIcsFileAsync(Interview interview);
    Task<bool> SaveIcsFileAsync(Interview interview, string filePath);
}
