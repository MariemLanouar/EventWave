using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Repositories
{
    public interface ISpeakerRepository
    {
        Task<Speaker> AddAsync(Speaker speaker);
        Task<List<Speaker>> GetAllAsync();
        Task<List<Speaker>> GetAllIncludingPendingAsync();
        Task<Speaker?> GetByIdAsync(int id);
        Task<Speaker> UpdateAsync(Speaker speaker);
        Task<bool> DeleteAsync(int id);
        Task<bool> ApproveSpeakerAsync(int id);
        Task<bool> RejectAsync(int id);
        Task<List<Speaker>> GetPendingSpeakersAsync();
        Task<List<Speaker>> SearchAsync(string? search);
        Task<SpeakerStatsDTO?> GetSpeakerStatsAsync(int speakerId);
    }
}
