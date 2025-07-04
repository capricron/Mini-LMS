using Backend.DTOs;
using Backend.Repositories;

namespace Backend.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _repo;

        public AssignmentService(IAssignmentRepository repo) => _repo = repo;
        public Task<IEnumerable<GetAssignmentDto>> GetAllAsync() => _repo.GetAllAsync();
        public Task<GetAssignmentDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<GetAssignmentDto> CreateAsync(CreateAssignmentDto dto) => _repo.CreateAsync(dto);
        public Task UpdateAsync(int id, UpdateAssignmentDto dto) => _repo.UpdateAsync(id, dto);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public async Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id)
        {
            return await _repo.GetByIdWithQuestionsAsync(id);
        }

        public async Task<int> CreateAssignmentWithQuestionsAsync(CreateAssignmentDto dto)
        {
            return await _repo.CreateWithQuestionsAsync(dto);
        }
    }
}