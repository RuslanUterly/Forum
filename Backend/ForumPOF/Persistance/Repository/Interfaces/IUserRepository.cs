using Persistance.Models;

namespace Persistance.Repository.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExistByEmail(string email);
    Task<bool> UserExistByUsername(string username);
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserById(Ulid id);
    Task<User> GetUserByEmail(string email);

    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(User user);

    Task<bool> Save();
}
