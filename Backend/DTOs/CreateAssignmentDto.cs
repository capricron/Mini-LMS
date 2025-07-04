namespace Backend.DTOs
{
    public class CreateAssignmentDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public List<MCQDto> Questions { get; set; } = new();

    }
}