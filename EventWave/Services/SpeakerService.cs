using EventWave.DTOs;
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

        public async Task<List<Speaker>> GetAllSpeakersIncludingPendingAsync()
        {
            return await speakerRepository.GetAllIncludingPendingAsync();
        }

        public async Task<Speaker?> GetSpeakerByIdAsync(int id)
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
        public Task<List<Speaker>> GetPendingSpeakersAsync()
        {
            return speakerRepository.GetPendingSpeakersAsync();
        }

        public Task<bool> ApproveSpeakerAsync(int id)
        {
            return speakerRepository.ApproveSpeakerAsync(id);
        }
        public async Task<bool> RejectSpeakerAsync(int id)
        {
            return await speakerRepository.RejectAsync(id);
        }

        public Task<List<Speaker>> SearchSpeakersAsync(string? search)
        {
            return speakerRepository.SearchAsync(search);
        }
        public async Task<SpeakerStatsDTO?> GetSpeakerStatsAsync(int speakerId)
        {
            return await speakerRepository.GetSpeakerStatsAsync(speakerId);
        }
    }
}
