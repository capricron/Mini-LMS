// File: DTOs/LoginResponseDto.cs
namespace Frontend.DTOs
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}