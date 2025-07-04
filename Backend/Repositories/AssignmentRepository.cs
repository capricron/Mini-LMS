using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<GetAssignmentDto>> GetAllAsync()
        {
            return await _context.Assignments
                .Select(a => new GetAssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }

        public async Task<GetAssignmentDto?> GetByIdAsync(int id)
        {
            return await _context.Assignments
                .Where(a => a.Id == id)
                .Select(a => new GetAssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    IsActive = a.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<GetAssignmentDto> CreateAsync(CreateAssignmentDto dto)
        {
            var assignment = new Assignment
            {
                Title = dto.Title,
                Media = dto.Media,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
            return new GetAssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                IsActive = assignment.IsActive
            };
        }
        public async Task UpdateAsync(int id, UpdateAssignmentDto dto)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Questions)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
                return;

            // 1. Simpan daftar soal baru dari DTO
            var newQuestions = dto.Questions.Select(q => new McqQuestion
            {
                QuestionText = q.QuestionText,
                OptionA = q.OptionA,
                OptionB = q.OptionB,
                OptionC = q.OptionC,
                OptionD = q.OptionD,
                CorrectAnswer = q.CorrectAnswer,
                AssignmentId = id // Pastikan ini di-set
            }).ToList();

            // 2. Hapus semua soal lama dari context (berdasarkan assignment)
            _context.MCQs.RemoveRange(assignment.Questions);

            // 3. Tambahkan semua soal baru
            await _context.MCQs.AddRangeAsync(newQuestions);

            // 4. Update data assignment utama
            assignment.Title = dto.Title;
            assignment.Description = dto.Description;
            assignment.IsActive = dto.IsActive;
            assignment.Media = dto.Media;

            // 5. Simpan perubahan
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id, string role)
        {
            if (role == "Learner")
            {
                return await _context.Assignments
                .Where(a => a.Id == id)
                .Select(a => new GetAssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    IsActive = a.IsActive,
                    Media = a.Media,
                    Questions = a.Questions.Select(q => new MCQDto
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        CorrectAnswer = ""
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            }
            else if (role == "Manager")
            {
                return await _context.Assignments
                .Where(a => a.Id == id)
                .Select(a => new GetAssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    IsActive = a.IsActive,
                    Media = a.Media,
                    Questions = a.Questions.Select(q => new MCQDto
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        OptionA = q.OptionA,
                        OptionB = q.OptionB,
                        OptionC = q.OptionC,
                        OptionD = q.OptionD,
                        CorrectAnswer = q.CorrectAnswer
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            }
            return null;
        }

        public async Task<int> CreateWithQuestionsAsync(CreateAssignmentDto dto)
        {
            var assignment = new Models.Assignment
            {
                Title = dto.Title,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Media = dto.Media,
                Questions = dto.Questions?.Select(q => new Models.McqQuestion
                {
                    QuestionText = q.QuestionText,
                    OptionA = q.OptionA,
                    OptionB = q.OptionB,
                    OptionC = q.OptionC,
                    OptionD = q.OptionD,
                    CorrectAnswer = q.CorrectAnswer
                }).ToList() ?? new()
            };

            await _context.Assignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
            return assignment.Id;
        }

        public async Task<AssignmentReviewDto> GetAssignmentReviewAsync(int assignmentId, string userId)
        {
            var progress = await _context.AssignmentProgresss
                .Include(p => p.Assignment)
                .Include(p => p.Answers)
                .ThenInclude(answer => answer.McqQuestion)
                .FirstOrDefaultAsync(p => p.AssignmentId == assignmentId && p.UserId == userId);

            if (progress == null)
                throw new KeyNotFoundException("Submission tidak ditemukan");

            return new AssignmentReviewDto
            {
                AssignmentId = progress.AssignmentId,
                Title = progress.Assignment?.Title ?? "Unknown",
                Score = progress.Score,
                SubmittedAt = progress.SubmittedAt,
                Answers = progress.Answers.Select(a => new QuestionAnswerDto
                {
                    QuestionId = a.QuestionId,
                    QuestionText = a.McqQuestion.QuestionText,
                    GivenAnswer = a.GivenAnswer,
                    IsCorrect = a.IsCorrect,
                    CorrectAnswer = a.McqQuestion.CorrectAnswer
                }).ToList()
            };
        }
    }

}