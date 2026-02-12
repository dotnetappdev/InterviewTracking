using InterviewTracking.Shared.Models;
using System.Text.Json;

namespace InterviewTracking.Maui.Services;

public interface IDataExportImportService
{
    Task<string> ExportToJsonAsync(IEnumerable<Interview> interviews);
    Task<bool> ExportToFileAsync(IEnumerable<Interview> interviews, string filePath);
    Task<IEnumerable<Interview>> ImportFromJsonAsync(string json);
    Task<IEnumerable<Interview>> ImportFromFileAsync(string filePath);
}

public class DataExportImportService : IDataExportImportService
{
    public Task<string> ExportToJsonAsync(IEnumerable<Interview> interviews)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var json = JsonSerializer.Serialize(interviews, options);
        return Task.FromResult(json);
    }

    public async Task<bool> ExportToFileAsync(IEnumerable<Interview> interviews, string filePath)
    {
        try
        {
            var json = await ExportToJsonAsync(interviews);
            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Export to file failed: {ex.Message}");
            return false;
        }
    }

    public Task<IEnumerable<Interview>> ImportFromJsonAsync(string json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var interviews = JsonSerializer.Deserialize<List<Interview>>(json, options);
            return Task.FromResult<IEnumerable<Interview>>(interviews ?? new List<Interview>());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Import from JSON failed: {ex.Message}");
            return Task.FromResult<IEnumerable<Interview>>(new List<Interview>());
        }
    }

    public async Task<IEnumerable<Interview>> ImportFromFileAsync(string filePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            return await ImportFromJsonAsync(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Import from file failed: {ex.Message}");
            return new List<Interview>();
        }
    }
}
