using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repository.Interfaces;



public interface IUserRepository
{
    Task<ICollection<User>> GetUsers();
    Task<User> GetUserById(Ulid id);
    Task<User> GetUserByEmail(string email);
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(User user);
    Task<bool> Save();
}
