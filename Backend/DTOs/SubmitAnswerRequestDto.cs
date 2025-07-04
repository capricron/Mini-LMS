// DTOs/SubmitAnswerRequestDto.cs
namespace Backend.DTOs
{
    public class SubmitAnswerRequestDto
    {
        public int AssignmentId { get; set; }
        public Dictionary<int, char> Answers { get; set; } = new();
    }
}