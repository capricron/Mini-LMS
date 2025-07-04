// Repository/LearnerRepository.cs
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository
{
    public class LearnerRepository : ILearnerRepository
    {
        private readonly AppDbContext _context;

        public LearnerRepository(AppDbContext context) => _context = context;

        public async Task<bool> HasAlreadySubmitted(string learnerId, int assignmentId)
        {
            return await _context.LearnerSubmissions
                .AnyAsync(s => s.LearnerId == learnerId && s.AssignmentId == assignmentId);
        }

        // Repository/LearnerRepository.cs
        public async Task<LearnerSubmission> SubmitAnswersAsync(LearnerSubmission submission)
        {
            await _context.LearnerSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();
            return submission;
        }

        public async Task<SubmissionResultDto?> GetSubmissionResultAsync(string learnerId, int assignmentId)
        {
            return await _context.LearnerSubmissions
                .Where(s => s.LearnerId == learnerId && s.AssignmentId == assignmentId)
                .Select(s => new SubmissionResultDto
                {
                    Score = s.Score,
                    TotalQuestions = s.Answers.Count,
                    CorrectAnswers = s.Answers.Count(a => a.IsCorrect),
                    SubmittedAt = s.SubmittedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}