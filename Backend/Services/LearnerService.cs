// Services/LearnerService.cs
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _repo;
        private readonly AppDbContext _context;

        public LearnerService(ILearnerRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<SubmissionResultDto> SubmitAnswersAsync(SubmitAnswerRequestDto dto, string userId)
        {
            if (await _repo.HasAlreadySubmitted(userId, dto.AssignmentId))
                throw new InvalidOperationException("Anda sudah pernah mengirim jawaban untuk tugas ini.");

            var submission = new AssignmentProgress
            {
                UserId = userId,
                AssignmentId = dto.AssignmentId,
                Answers = new List<SubmittedAnswer>()
            };

            var questions = await _context.MCQs
                .Where(q => q.AssignmentId == dto.AssignmentId)
                .ToDictionaryAsync(q => q.Id);

            int correctCount = 0;
            foreach (var answer in dto.Answers)
            {
                if (!questions.TryGetValue(answer.Key, out var question)) continue;

                var submittedAnswer = new SubmittedAnswer
                {
                    QuestionId = answer.Key,
                    GivenAnswer = answer.Value,
                    IsCorrect = char.ToUpper(question.CorrectAnswer[0]) == char.ToUpper(answer.Value)
                };

                if (submittedAnswer.IsCorrect) correctCount++;

                submission.Answers.Add(submittedAnswer);
            }

            double scorePercentage = (double)correctCount / questions.Count * 100;
            submission.Score = Math.Round(scorePercentage, 2); // Contoh: 100.00

            await _repo.SubmitAnswersAsync(submission);

            return new SubmissionResultDto
            {
                Score = submission.Score,
                TotalQuestions = submission.Answers.Count,
                CorrectAnswers = correctCount,
                SubmittedAt = submission.SubmittedAt
            };
        }

        public async Task<SubmissionResultDto?> GetSubmissionResultAsync(string learnerId, int assignmentId)
        {
            return await _repo.GetSubmissionResultAsync(learnerId, assignmentId);
        }
    }
}