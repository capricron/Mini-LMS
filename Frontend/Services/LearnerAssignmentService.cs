// Services/LearnerAssignmentService.cs

using System.Net.Http.Json;
using System.Net.Http.Headers;
using Frontend.DTOs;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;

namespace Frontend.Services
{
    public class LearnerAssignmentService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;

        public LearnerAssignmentService(HttpClient http, IJSRuntime jsRuntime)
        {
            _http = http;
            _jsRuntime = jsRuntime;
        }

        // Ambil semua assignment (tanpa soal)
        public async Task<IEnumerable<AssignmentDto>> GetAllAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await _http.GetFromJsonAsync<IEnumerable<AssignmentDto>>("api/assignments");
                return result ?? new List<AssignmentDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignments: {ex.Message}");
                return new List<AssignmentDto>();
            }
        }

        // Ambil satu assignment beserta soal-soalnya
        public async Task<AssignmentDto?> GetWithQuestionsAsync(int id)
        {

            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _http.GetFromJsonAsync<AssignmentDto>($"api/assignments/{id}/with-questions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignment with questions (ID {id}): {ex.Message}");
                return null;
            }
        }

        // 🔥 Kirim jawaban learner ke server dengan JWT auth
        public async Task SubmitAnswersAsync(object payload)
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Serialisasi payload ke JSON
                var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = false });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _http.PostAsync("api/Learner/submit", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Submission failed: {error}");
                    throw new Exception($"Gagal mengirim jawaban: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error submitting answers: {ex.Message}");
                throw;
            }
        }

    }
}