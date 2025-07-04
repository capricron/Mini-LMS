namespace Backend.DTOs
{
    public class UpdateAssignmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<MCQDto> Questions { get; set; } = new();

    }
}