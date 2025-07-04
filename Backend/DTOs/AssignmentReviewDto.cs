namespace Backend.DTOs
{
    public class AssignmentReviewDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Score { get; set; }
        public DateTime SubmittedAt { get; set; }

        public List<QuestionAnswerDto> Answers { get; set; } = new();
    }

    public class QuestionAnswerDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public char GivenAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}