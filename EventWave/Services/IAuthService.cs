using EventWave.Models;

namespace EventWave.Services
{
    public interface IAuthService
    {
        Task<string> GenerateToken(User user);
    }
}
