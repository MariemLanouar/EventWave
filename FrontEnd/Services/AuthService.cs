using System.Net.Http.Json;
using FrontEnd.DTOs;
namespace FrontEnd.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> LoginAsync(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }


}
