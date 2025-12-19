using System.Net.Http.Json;
using System.Net.Http.Headers;
using FrontEnd.DTOs;

namespace FrontEnd.Services
{
    public class ProfileService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;

        public ProfileService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
        }

        private async Task AddAuthHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ProfileDTO?> GetMyProfileAsync()
        {
            await AddAuthHeaderAsync();
            try
            {
                return await _http.GetFromJsonAsync<ProfileDTO>("api/profile/me");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateProfileAsync(ProfileDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/profile", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProfileAsync(ProfileDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PutAsJsonAsync("api/profile", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
