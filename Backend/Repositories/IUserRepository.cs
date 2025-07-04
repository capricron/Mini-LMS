// Repository/IUserRepository.cs
using Backend.Models;

namespace Backend.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}