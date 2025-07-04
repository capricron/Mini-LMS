// Models/AssignmentProgress.cs
namespace Backend.Models
{
    public class AssignmentProgress
    {
        public int Id { get; set; }

        // Foreign Key ke User
        public string UserId { get; set; } = null!;

        // Navigasi ke User
        public User User { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public double Score { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SubmittedAnswer> Answers { get; set; } = new List<SubmittedAnswer>();
    }

   public class SubmittedAnswer
{
    public int Id { get; set; }
    public int QuestionId { get; set; } // ID soal dari MCQ
    public char GivenAnswer { get; set; }
    public bool IsCorrect { get; set; }

    // Navigasi ke AssignmentProgress
    public AssignmentProgress AssignmentProgress { get; set; } = null!;
    public int AssignmentProgressId { get; set; }

    // 🔥 TAMBAHKAN FOREIGN KEY EXPLISIT
    public int McqQuestionId { get; set; } // Ini harus sama dengan QuestionId
    public McqQuestion McqQuestion { get; set; } = null!; // Navigasi
}
}