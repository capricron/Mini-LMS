// DTOs/LoginRequestDto.cs
namespace Backend.DTOs
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty; // Ganti dari Username ke Email
        public string Password { get; set; } = string.Empty;
    }
}