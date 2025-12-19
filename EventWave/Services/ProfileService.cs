using EventWave.DTOs;
using EventWave.Models;
using EventWave.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EventWave.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepo;
        private readonly UserManager<User> _userManager;

        public ProfileService(
            IProfileRepository profileRepo,
            UserManager<User> userManager)
        {
            _profileRepo = profileRepo;
            _userManager = userManager;
        }

        public async Task<ProfileResponseDTO?> GetMyProfileAsync(string userId)
        {
            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null) return null;

            var user = profile.User;
            var roles = await _userManager.GetRolesAsync(user);

            return new ProfileResponseDTO
            {
                FullName = user.FullName,
                Email = user.Email,
                Phone = profile.Phone,
                City = profile.City,
                Bio = roles.Contains("Organizer") ? profile.Bio : null,
                Roles = roles.ToList()
            };
        }

        public async Task CreateProfileAsync(string userId, ProfileDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var profile = new Profile
            {
                UserId = userId,
                Phone = dto.Phone,
                City = dto.City,
                Bio = roles.Contains("Organizer") ? dto.Bio : null
            };

            await _profileRepo.CreateAsync(profile);
        }

        public async Task UpdateProfileAsync(string userId, ProfileDTO dto)
        {
            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null)
                throw new Exception("Profile not found");

            var user = profile.User;
            var roles = await _userManager.GetRolesAsync(user);

            profile.Phone = dto.Phone;
            profile.City = dto.City;
            profile.Bio = roles.Contains("Organizer") ? dto.Bio : null;

            await _profileRepo.UpdateAsync(profile);
        }
        public async Task CreateProfileInternalAsync(Profile profile)
        {
            await _profileRepo.CreateAsync(profile);
        }

    }
}
