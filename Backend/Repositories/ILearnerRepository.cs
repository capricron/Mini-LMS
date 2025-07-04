// Repository/ILearnerRepository.cs
using Backend.DTOs;
using Backend.Models;

namespace Backend.Repository
{
    public interface ILearnerRepository
    {
        Task<bool> HasAlreadySubmitted(string learnerId, int assignmentId);
        Task<LearnerSubmission> SubmitAnswersAsync(LearnerSubmission submission);
        Task<SubmissionResultDto?> GetSubmissionResultAsync(string learnerId, int assignmentId);
    }
}