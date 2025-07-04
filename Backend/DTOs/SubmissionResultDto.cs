// DTOs/SubmissionResultDto.cs
namespace Backend.DTOs
{
    public class SubmissionResultDto
    {
        public string LearnerName { get; set; }
        public double Score { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}