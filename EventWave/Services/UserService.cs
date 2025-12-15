using EventWave.Models;
using EventWave.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(User user, string role, string password) =>
            await _userRepository.AddUserAsync(user, role, password);

        public async Task<User?> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return user;
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role) =>
            await _userRepository.GetUsersByRoleAsync(role);

        public async Task<User> UpdateUserAsync(User user)
        {
           
            return await _userRepository.UpdateUserAsync(user);

        }
           

        public async Task<bool> DeleteUserAsync(string id)
        {
            /*bool hasEvents = await _context.Events
      .AnyAsync(e => e.OrganizerId == organizerId);

           if (hasEvents)
               throw new InvalidOperationException(
                   "Cannot delete organizer with existing events."
               );*/
            return await _userRepository.DeleteUserAsync(id);

        }

        public Task<User> GetByEmailAsync(string email)
        {
            return _userRepository.GetByEmailAsync(email);
        }

        public Task<bool> ValidatePasswordAsync(User user, string password)
        {
            return _userRepository.CheckPasswordAsync(user, password);
        }

        public async Task<List<User>> SearchUsersByRoleAsync(string role, string? search)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u =>
                    u.FullName.Contains(search) ||
                    u.Email.Contains(search)
                ).ToList();
            }

            return users;
        }

    }

}
