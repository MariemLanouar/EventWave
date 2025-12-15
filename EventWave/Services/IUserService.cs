using EventWave.Models;

namespace EventWave.Services
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user, string role, string password);
        Task<User> GetByIdAsync(string id);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task<List<User>> SearchUsersByRoleAsync(string role, string? search);
    }
}
