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
            var assignment = new Models.Assignment
            {
                Title = dto.Title,
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

            if (assignment == null) return;

            assignment.Title = dto.Title;
            assignment.Description = dto.Description;
            assignment.IsActive = dto.IsActive;

            // Hapus semua soal lama
            _context.MCQs.RemoveRange(assignment.Questions);

            // Tambahkan soal baru
            foreach (var dtoQuestion in dto.Questions)
            {
                var existingQuestion = assignment.Questions.FirstOrDefault(q => q.Id == dtoQuestion.Id);

                if (existingQuestion == null)
                {
                    // Tambah baru
                    assignment.Questions.Add(new McqQuestion
                    {
                        QuestionText = dtoQuestion.QuestionText,
                        OptionA = dtoQuestion.OptionA,
                        OptionB = dtoQuestion.OptionB,
                        OptionC = dtoQuestion.OptionC,
                        OptionD = dtoQuestion.OptionD,
                        CorrectAnswer = dtoQuestion.CorrectAnswer
                    });
                }
                else
                {
                    // Update data
                    existingQuestion.QuestionText = dtoQuestion.QuestionText;
                    existingQuestion.OptionA = dtoQuestion.OptionA;
                    existingQuestion.OptionB = dtoQuestion.OptionB;
                    existingQuestion.OptionC = dtoQuestion.OptionC;
                    existingQuestion.OptionD = dtoQuestion.OptionD;
                    existingQuestion.CorrectAnswer = dtoQuestion.CorrectAnswer;
                }
            }

            // Hapus soal yang tidak ada di DTO
            var idsToKeep = dto.Questions.Select(q => q.Id).ToList();
            var questionsToRemove = assignment.Questions.Where(q => !idsToKeep.Contains(q.Id)).ToList();

            _context.MCQs.RemoveRange(questionsToRemove);

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

        public async Task<GetAssignmentDto?> GetByIdWithQuestionsAsync(int id)
        {
            return await _context.Assignments
                .Where(a => a.Id == id)
                .Select(a => new GetAssignmentDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    IsActive = a.IsActive,
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

        public async Task<int> CreateWithQuestionsAsync(CreateAssignmentDto dto)
        {
            var assignment = new Models.Assignment
            {
                Title = dto.Title,
                Description = dto.Description,
                IsActive = dto.IsActive,
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
    }
}