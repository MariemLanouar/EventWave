using EventWave.DTOs;
using EventWave.Models;

namespace EventWave.Services
{
    public interface ISpeakerService
    {
        Task<Speaker> CreateSpeakerAsync(Speaker speaker);
        Task<List<Speaker>> GetAllSpeakersAsync();
        Task<List<Speaker>> GetAllSpeakersIncludingPendingAsync();
        Task<Speaker?> GetSpeakerByIdAsync(int id);
        Task<Speaker> UpdateSpeakerAsync(Speaker speaker);
        Task<bool> DeleteSpeakerAsync(int id);
        Task<List<Speaker>> GetPendingSpeakersAsync();
        Task<bool> ApproveSpeakerAsync(int id);
        Task<bool> RejectSpeakerAsync(int id);
        Task<List<Speaker>> SearchSpeakersAsync(string? search);
        Task<SpeakerStatsDTO?> GetSpeakerStatsAsync(int speakerId);
    }
}
