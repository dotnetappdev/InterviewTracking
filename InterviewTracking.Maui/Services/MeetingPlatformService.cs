using Microsoft.EntityFrameworkCore;
using InterviewTracking.Maui.Data;
using InterviewTracking.Shared.Models;

namespace InterviewTracking.Maui.Services;

public interface IMeetingPlatformService
{
    Task<IEnumerable<MeetingPlatformType>> GetAllPlatformsAsync();
    Task<MeetingPlatformType?> GetPlatformByIdAsync(int id);
    Task<MeetingPlatformType?> GetPlatformByTypeAsync(MeetingPlatform platformType);
}

public class MeetingPlatformService : IMeetingPlatformService
{
    private readonly LocalDbContext _context;

    public MeetingPlatformService(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MeetingPlatformType>> GetAllPlatformsAsync()
    {
        return await _context.MeetingPlatformTypes
            .Where(m => m.IsActive)
            .OrderBy(m => m.Id)
            .ToListAsync();
    }

    public async Task<MeetingPlatformType?> GetPlatformByIdAsync(int id)
    {
        return await _context.MeetingPlatformTypes
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<MeetingPlatformType?> GetPlatformByTypeAsync(MeetingPlatform platformType)
    {
        return await _context.MeetingPlatformTypes
            .FirstOrDefaultAsync(m => m.PlatformType == platformType);
    }
}
