using EventWave.Models;
using EventWave.Repositories;

namespace EventWave.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly ISpeakerRepository speakerRepository;

        public SpeakerService(ISpeakerRepository speakerRepository)
        {
            this.speakerRepository = speakerRepository;
        }

        public async Task<Speaker> CreateSpeakerAsync(Speaker speaker)
        {
            return await speakerRepository.AddAsync(speaker);
        }

        public async Task<List<Speaker>> GetAllSpeakersAsync()
        {
            return await speakerRepository.GetAllAsync();
        }

        public async Task<Speaker> GetSpeakerByIdAsync(int id)
        {
            return await speakerRepository.GetByIdAsync(id);
        }

        public async Task<Speaker> UpdateSpeakerAsync(Speaker speaker)
        {
            return await speakerRepository.UpdateAsync(speaker);
        }

        public async Task<bool> DeleteSpeakerAsync(int id)
        {
            return await speakerRepository.DeleteAsync(id);
        }
    }
}
