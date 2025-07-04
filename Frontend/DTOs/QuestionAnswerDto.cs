public class QuestionAnswerDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public char GivenAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public string CorrectAnswer { get; set; } = string.Empty; // ✅ Sekarang public
}