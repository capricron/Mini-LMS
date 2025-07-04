using Frontend.DTOs;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Frontend.Dto;
using Microsoft.JSInterop;

namespace Frontend.Services
{
    public class AssignmentService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;

        public AssignmentService(HttpClient http, IJSRuntime jsRuntime)
        {
            _http = http;
            _jsRuntime = jsRuntime;

        }

        // Ambil semua assignment
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

        // Ambil satu assignment tanpa soal
        public async Task<AssignmentDto?> GetByIdAsync(int id)
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await _http.GetFromJsonAsync<AssignmentDto>($"api/assignments/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignment by ID {id}: {ex.Message}");
                return null;
            }
        }

        // Ambil assignment lengkap dengan soal-soal
        public async Task<AssignmentDto?> GetByIdWithQuestionsAsync(int id)
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

        // Buat assignment baru
        public async Task CreateAsync(AssignmentDto dto)
        {
            Console.WriteLine(dto);
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _http.PostAsJsonAsync("api/assignments", dto);
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Create failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating assignment: {ex.Message}");
            }
        }

        // Update assignment
        public async Task UpdateAsync(int id, AssignmentDto dto)
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _http.PutAsJsonAsync($"api/assignments/{id}", dto);
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Update failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating assignment {id}: {ex.Message}");
            }
        }

        // Hapus assignment
        public async Task DeleteAsync(int id)
        {
            try
            {

                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _http.DeleteAsync($"api/assignments/{id}");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Delete failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting assignment {id}: {ex.Message}");
            }
        }

        // Ambil semua submission untuk assignment tertentu
        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(int id)
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var submissions = await _http.GetFromJsonAsync<IEnumerable<SubmissionDto>>(
                    $"api/assignments/{id}/submissions");

                return submissions ?? new List<SubmissionDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching submissions for assignment ID {id}: {ex.Message}");
                return new List<SubmissionDto>();
            }
        }

        public async Task<AssignmentReviewDto?> GetReviewAsync(int assignmentId, string userId)
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("JWT token tidak ditemukan di localStorage.");

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await _http.GetFromJsonAsync<AssignmentReviewDto>($"api/assignments/review/{assignmentId}/{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting assignment {ex.Message}");
                return null;
            }
        }
    }
}