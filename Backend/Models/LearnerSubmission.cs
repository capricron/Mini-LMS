// Models/LearnerSubmission.cs
namespace Backend.Models
{
 public class LearnerSubmission
    {
        public int Id { get; set; }

        // Foreign Key ke User
        public string LearnerId { get; set; } = null!;

        // Navigasi ke User
        public User User { get; set; } = null!;

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public double Score { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SubmittedAnswer> Answers { get; set; } = new List<SubmittedAnswer>();
    }

    public class SubmittedAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public char GivenAnswer { get; set; }
        public bool IsCorrect { get; set; }

        public LearnerSubmission Submission { get; set; } = null!; // Navigasi
        public int SubmissionId { get; set; } // Foreign Key
    }
}