using Backend.DTOs;

namespace Backend.Repositories
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<GetAssignmentDto>> GetAllAsync();
        Task<GetAssignmentDto?> GetByIdAsync(int id);
        Task<GetAssignmentDto> CreateAsync(CreateAssignmentDto dto);
        Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id);
        Task UpdateAsync(int id, UpdateAssignmentDto dto);
        Task DeleteAsync(int id);

        Task<int> CreateWithQuestionsAsync(CreateAssignmentDto dto);

    }
}