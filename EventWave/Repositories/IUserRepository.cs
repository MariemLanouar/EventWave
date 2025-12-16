using EventWave.Models;
using Microsoft.AspNetCore.Identity;

namespace EventWave.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user, string role, string password);
        Task<User?> GetByIdAsync(string id);
        Task<List<User>> GetUsersByRoleAsync(string role);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
