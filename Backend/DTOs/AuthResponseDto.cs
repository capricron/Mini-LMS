namespace Backend.DTOs
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}