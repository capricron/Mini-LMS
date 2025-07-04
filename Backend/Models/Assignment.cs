namespace Backend.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Media { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<McqQuestion> Questions { get; set; } = new List<McqQuestion>();
    }
}