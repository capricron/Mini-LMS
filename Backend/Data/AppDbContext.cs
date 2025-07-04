// Backend/Data/AppDbContext.cs

using Backend.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<McqQuestion> MCQs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data Assignment & MCQ
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        var assignment1 = new Assignment
        {
            Id = 1,
            Title = "Quiz Matematika Dasar",
            Description = "Pertanyaan pilihan ganda tentang aritmatika dasar",
            IsActive = true
        };

        var mcqs = new List<McqQuestion>
        {
            new McqQuestion
            {
                Id = 1,
                QuestionText = "Berapa hasil dari 5 + 7?",
                OptionA = "10",
                OptionB = "11",
                OptionC = "12",
                OptionD = "13",
                CorrectAnswer = "C",
                AssignmentId = 1
            },
            new McqQuestion
            {
                Id = 2,
                QuestionText = "Mana yang merupakan bilangan prima?",
                OptionA = "4",
                OptionB = "9",
                OptionC = "10",
                OptionD = "11",
                CorrectAnswer = "D",
                AssignmentId = 1
            }
        };

        modelBuilder.Entity<Assignment>().HasData(assignment1);
        modelBuilder.Entity<McqQuestion>().HasData(mcqs);
    }
}