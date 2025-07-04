// Repository/ILearnerRepository.cs
using Backend.DTOs;
using Backend.Models;

namespace Backend.Repository
{
    public interface ILearnerRepository
    {
        Task<bool> HasAlreadySubmitted(string learnerId, int assignmentId);
        Task<AssignmentProgress> SubmitAnswersAsync(AssignmentProgress submission);
        Task<SubmissionResultDto?> GetSubmissionResultAsync(string learnerId, int assignmentId);

        Task<IEnumerable<AssignmentProgressSummaryDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId);
    }
}