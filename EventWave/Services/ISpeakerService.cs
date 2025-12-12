using EventWave.Models;

namespace EventWave.Services
{
    public interface ISpeakerService
    {
        Task<Speaker> CreateSpeakerAsync(Speaker speaker);
        Task<List<Speaker>> GetAllSpeakersAsync();
        Task<Speaker> GetSpeakerByIdAsync(int id);
        Task<Speaker> UpdateSpeakerAsync(Speaker speaker);
        Task<bool> DeleteSpeakerAsync(int id);
    }
}
