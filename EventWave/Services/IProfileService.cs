using EventWave.DTOs;

namespace EventWave.Services
{
    public interface IProfileService
    {
        Task<ProfileResponseDTO?> GetMyProfileAsync(string userId);
        Task CreateProfileAsync(string userId, ProfileDTO dto);
        Task UpdateProfileAsync(string userId, ProfileDTO dto);
    }
}
