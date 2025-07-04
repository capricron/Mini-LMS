using Backend.DTOs;

namespace Backend.Services
{
    public interface IAssignmentService
    {
        Task<IEnumerable<GetAssignmentDto>> GetAllAsync();
        Task<GetAssignmentDto?> GetByIdAsync(int id);
        Task<GetAssignmentDto> CreateAsync(CreateAssignmentDto dto);
        Task UpdateAsync(int id, UpdateAssignmentDto dto);
        Task DeleteAsync(int id);

        Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id);

        Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id, string? userId, string? role);

        Task<IEnumerable<AssignmentProgressSummaryDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId);

        Task<int> CreateAssignmentWithQuestionsAsync(CreateAssignmentDto dto);
    }
}