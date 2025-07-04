using Backend.DTOs;
using Backend.Repositories;
using Backend.Repository;
using Microsoft.AspNetCore.Http;

namespace Backend.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _repo;
        private readonly ILearnerRepository _learnerRepo;

        public AssignmentService(IAssignmentRepository repo, ILearnerRepository learnerRepo)
        {
            _repo = repo;
            _learnerRepo = learnerRepo;
        }

        public Task<IEnumerable<GetAssignmentDto>> GetAllAsync() => _repo.GetAllAsync();

        public Task<GetAssignmentDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task<GetAssignmentDto> CreateAsync(CreateAssignmentDto dto) => _repo.CreateAsync(dto);

        public Task UpdateAsync(int id, UpdateAssignmentDto dto) => _repo.UpdateAsync(id, dto);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public async Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id)
        {
            // Default behavior for non-Learner or when userId/role not checked yet
            return await _repo.GetByIdWithQuestionsAsync(id);
        }

        public async Task<int> CreateAssignmentWithQuestionsAsync(CreateAssignmentDto dto)
        {
            return await _repo.CreateWithQuestionsAsync(dto);
        }

        // Overloaded method to support user context (userId & role)
        public async Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id, string? userId, string? role)
        {
            var dto = await _repo.GetByIdWithQuestionsAsync(id);

            if (dto == null) return null;

            if (role == "Learner" && !string.IsNullOrEmpty(userId))
            {
                bool hasSubmitted = await _learnerRepo.HasAlreadySubmitted(userId, id);

                if (hasSubmitted)
                {
                    // Clear the questions
                    dto.Questions.Clear();

                    // Get submission result and attach to DTO
                    var submissionResult = await _learnerRepo.GetSubmissionResultAsync(userId, id);
                    dto.SubmissionResult = submissionResult;
                }
            }

            return dto;
        }

        public async Task<IEnumerable<AssignmentProgressSummaryDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            return await _learnerRepo.GetSubmissionsByAssignmentIdAsync(assignmentId);
        }
    }
}