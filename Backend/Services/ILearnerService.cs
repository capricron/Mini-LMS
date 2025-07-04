// Services/ILearnerService.cs
using Backend.DTOs;

namespace Backend.Services
{
    public interface ILearnerService
    {
        // Services/ILearnerService.cs
        Task<SubmissionResultDto> SubmitAnswersAsync(SubmitAnswerRequestDto dto, string userId);
        Task<SubmissionResultDto?> GetSubmissionResultAsync(string learnerId, int assignmentId);
    }
}