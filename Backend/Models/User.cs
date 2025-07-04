// Models/User.cs
namespace Backend.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // atau gunakan int jika tidak pakai Identity
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Relasi ke Role
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        // Relasi ke Submission
        public ICollection<LearnerSubmission> Submissions { get; set; } = new List<LearnerSubmission>();
    }
}