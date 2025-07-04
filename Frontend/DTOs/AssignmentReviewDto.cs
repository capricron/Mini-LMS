public class AssignmentReviewDto
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Score { get; set; }
    public DateTime SubmittedAt { get; set; }
    public List<QuestionAnswerDto> Answers { get; set; } = new();
}
