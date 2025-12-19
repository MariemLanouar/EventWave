using FrontEnd.DTOs;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrontEnd.Services
{
    public class EventService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public EventService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _jsonOptions.Converters.Add(new JsonStringEnumConverter());
        }

        private async Task AddAuthHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<EventResponseDTO>> GetEventsAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<EventResponseDTO>>("api/events", _jsonOptions) ?? new List<EventResponseDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<EventResponseDTO>();
            }
        }

        public async Task<List<EventResponseDTO>> SearchEventsAsync(string query)
        {
             try
            {
                return await _http.GetFromJsonAsync<List<EventResponseDTO>>($"api/events/search?q={query}", _jsonOptions) ?? new List<EventResponseDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<EventResponseDTO>();
            }
        }

        public async Task<EventResponseDTO?> GetEventByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<EventResponseDTO>($"api/events/{id}", _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> CreateEventAsync(EventDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/events", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventAsync(int id, UpdateEventDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PutAsJsonAsync($"api/events/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelEventAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PatchAsync($"api/events/{id}/cancel", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<EventResponseDTO>> GetMyEventsAsync()
        {
            await AddAuthHeaderAsync();
            try
            {
                return await _http.GetFromJsonAsync<List<EventResponseDTO>>("api/events/my-events", _jsonOptions) ?? new List<EventResponseDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<EventResponseDTO>();
            }
        }

        public async Task<List<VenueDTO>> GetVenuesAsync()
        {
             return await _http.GetFromJsonAsync<List<VenueDTO>>("api/venues", _jsonOptions) ?? new List<VenueDTO>();
        }

        public async Task<bool> AddVenueAsync(VenueDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/venues", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<WaitlistDTO>> GetWaitlistAsync(int eventId)
        {
            await AddAuthHeaderAsync();
            return await _http.GetFromJsonAsync<List<WaitlistDTO>>($"api/waitlist/event/{eventId}", _jsonOptions) ?? new List<WaitlistDTO>();
        }

        public async Task<bool> ApproveWaitlistAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsync($"api/waitlist/approve/{id}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RejectWaitlistAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _http.DeleteAsync($"api/waitlist/reject/{id}");
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> AddSpeakerAsync(SpeakerDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/speaker", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<SpeakerDTO>> GetAllSpeakersAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<SpeakerDTO>>("api/speaker", _jsonOptions) ?? new List<SpeakerDTO>();
            }
            catch
            {
                return new List<SpeakerDTO>();
            }
        }

        public async Task<List<SpeakerDTO>> GetAllSpeakersIncludingPendingAsync()
        {
            await AddAuthHeaderAsync();
            try
            {
                return await _http.GetFromJsonAsync<List<SpeakerDTO>>("api/speaker/all", _jsonOptions) ?? new List<SpeakerDTO>();
            }
            catch
            {
                return new List<SpeakerDTO>();
            }
        }



        public async Task<bool> RegisterEventAsync(RegistrationDTO dto)
        {
            await AddAuthHeaderAsync();
            var response = await _http.PostAsJsonAsync("api/registration", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<RegistrationResponseDTO>> GetMyRegistrationsAsync()
        {
            await AddAuthHeaderAsync();
            try
            {
                return await _http.GetFromJsonAsync<List<RegistrationResponseDTO>>("api/registration/my-registrations", _jsonOptions) ?? new List<RegistrationResponseDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<RegistrationResponseDTO>();
            }
        }
    }
}
