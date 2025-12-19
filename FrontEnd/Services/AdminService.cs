using System.Net.Http.Json;
using System.Net.Http.Headers;
using FrontEnd.DTOs;

namespace FrontEnd.Services
{
    public class AdminService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;

        public AdminService(HttpClient http, AuthService authService)
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

        // Organizers
        public async Task<List<UserDTO>> GetOrganizersAsync()
        {
            await AddAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<UserDTO>>("api/admin/organizers") ?? new List<UserDTO>();
        }

        public async Task<bool> AddOrganizerAsync(RegisterDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/admin/add-organizer", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrganizerAsync(string id)
        {
            await AddAuthHeaderAsync();
            var response = await _http.DeleteAsync($"api/admin/organizers/{id}");
            return response.IsSuccessStatusCode;
        }

        // Speakers
        public async Task<List<SpeakerDTO>> GetPendingSpeakersAsync()
        {
            await AddAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<SpeakerDTO>>("api/admin/pending-speakers") ?? new List<SpeakerDTO>();
        }

        public async Task<bool> ApproveSpeakerAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PutAsync($"api/admin/approve-speaker/{id}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
