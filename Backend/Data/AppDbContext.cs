// Backend/Data/AppDbContext.cs

using Backend.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet untuk model yang sudah ada
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<McqQuestion> MCQs { get; set; } = null!;

    // Tambahkan DbSet baru
    public DbSet<AssignmentProgress> AssignmentProgresss { get; set; } = null!;
    public DbSet<SubmittedAnswer> SubmittedAnswers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data Assignment & MCQ
        SeedData(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
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

        var learnerRole = new Role { Id = 1, Name = "Learner" };
        var managerRole = new Role { Id = 2, Name = "Manager" };

        modelBuilder.Entity<Role>().HasData(learnerRole, managerRole);

        // Contoh user
        var user1 = new User
        {
            Id = "user1",
            Username = "budi",
            Email = "budi@example.com",
            Password = "$2a$12$KViAg6rRQXmv0KOBog2t7.WJmsofzFj3nzw3VdkYVYvv2sfNX/2e2", // Hash dari "password123"
            RoleId = learnerRole.Id
        };

        var user2 = new User
        {
            Id = "user2",
            Username = "andi",
            Email = "andi@example.com",
            Password = "$2a$12$KViAg6rRQXmv0KOBog2t7.WJmsofzFj3nzw3VdkYVYvv2sfNX/2e2", // Hash dari "password123"
            RoleId = managerRole.Id
        };

        modelBuilder.Entity<User>().HasData(user1, user2);
    }
}