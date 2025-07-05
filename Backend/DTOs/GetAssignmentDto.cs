namespace Backend.DTOs
{
    public class GetAssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Media { get; set; }
        public DateTime CreatedAt { get; set; } // tambahkan baris ini

        public List<MCQDto> Questions { get; set; } = new();

        public SubmissionResultDto? SubmissionResult { get; set; }


    }
}