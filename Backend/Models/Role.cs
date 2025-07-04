// Models/Role.cs
namespace Backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Learner", "Manager"

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}