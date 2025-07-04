namespace Backend.DTOs
{
    public class AssignmentProgressSummaryDto
    {
        public string UserId { get; set; } = string.Empty;
        public string LearnerName { get; set; } = string.Empty;
        public double Score { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}