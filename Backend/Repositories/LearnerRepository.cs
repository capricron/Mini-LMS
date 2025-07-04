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

        public async Task<bool> HasAlreadySubmitted(string UserId, int assignmentId)
        {
            return await _context.AssignmentProgresss
                .AnyAsync(s => s.UserId == UserId && s.AssignmentId == assignmentId);
        }

        // Repository/LearnerRepository.cs
        public async Task<AssignmentProgress> SubmitAnswersAsync(AssignmentProgress submission)
        {
            await _context.AssignmentProgresss.AddAsync(submission);
            await _context.SaveChangesAsync();
            return submission;
        }

        public async Task<SubmissionResultDto?> GetSubmissionResultAsync(string UserId, int assignmentId)
        {
            return await _context.AssignmentProgresss
                .Where(s => s.UserId == UserId && s.AssignmentId == assignmentId)
                .Select(s => new SubmissionResultDto
                {
                    Score = s.Score,
                    TotalQuestions = s.Answers.Count,
                    CorrectAnswers = s.Answers.Count(a => a.IsCorrect),
                    SubmittedAt = s.SubmittedAt
                })
                .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<AssignmentProgressSummaryDto>> GetSubmissionsByAssignmentIdAsync(int assignmentId)
        {
            return await _context.AssignmentProgresss
                .Where(s => s.AssignmentId == assignmentId)
                .Select(s => new AssignmentProgressSummaryDto
                {
                    UserId = s.UserId,
                    LearnerName = s.User.Username, // Pastikan User.Name tersedia
                    Score = s.Score,
                    SubmittedAt = s.SubmittedAt
                })
                .ToListAsync();
        }

    }
}