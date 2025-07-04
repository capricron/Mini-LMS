using System.Net.Http.Json;

namespace Frontend.Services
{
    public class AssignmentService
    {
        private readonly HttpClient _http;

        public AssignmentService(HttpClient http) => _http = http;

        // Ambil semua assignment
        public async Task<IEnumerable<AssignmentDto>> GetAllAsync()
        {
            try
            {
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
                return await _http.GetFromJsonAsync<AssignmentDto>($"api/assignments/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignment by ID {id}: {ex.Message}");
                return null;
            }
        }

        // 🔥 Ambil assignment lengkap dengan soal-soal
        public async Task<AssignmentDto?> GetByIdWithQuestionsAsync(int id)
        {
            try
            {
                var assignment = await _http.GetFromJsonAsync<AssignmentDto>($"api/assignments/{id}/with-questions");

                // Tambahkan baris ini untuk log hasil ke konsol
                Console.WriteLine($"Assignment Title: {assignment?.Title}");
                Console.WriteLine($"Description: {assignment?.Description}");
                Console.WriteLine($"Number of Questions: {assignment?.Questions?.Count}");

                if (assignment?.Questions != null)
                {
                    foreach (var q in assignment.Questions)
                    {
                        Console.WriteLine($"- Question: {q.QuestionText}, Correct Answer: {q.CorrectAnswer}");
                    }
                }

                return assignment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching assignment with questions (ID {id}): {ex.Message}");
                return null;
            }
        }

        // Buat assignment tanpa soal
        public async Task CreateAsync(AssignmentDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/assignments", dto);
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Create failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating assignment: {ex.Message}");
            }
        }

        // Update assignment tanpa soal
        public async Task UpdateAsync(int id, AssignmentDto dto)
        {
            try
            {
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
                var response = await _http.DeleteAsync($"api/assignments/{id}");
                if (!response.IsSuccessStatusCode)
                    Console.WriteLine($"Delete failed with status code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting assignment {id}: {ex.Message}");
            }
        }
    }
}