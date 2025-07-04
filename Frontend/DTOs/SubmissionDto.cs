namespace Frontend.Dto
{
    public class SubmissionDto
    {
        public string UserId { get; set; } = string.Empty;
        public string LearnerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}