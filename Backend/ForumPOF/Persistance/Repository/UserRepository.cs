using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Persistance.Repository;

public class UserRepository(ForumContext context) : IUserRepository
{
    private readonly ForumContext _context = context;

    public async Task<bool> UserExistByEmail(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> UserExistByUsername(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }


    public async Task<User> GetUserById(Ulid id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<bool> CreateUser(User user)
    {
        _context.Add(user);
        return await Save();
    }

    public async Task<bool> DeleteUser(User user)
    {
        _context.Remove(user);
        return await Save();
    }

    public async Task<bool> UpdateUser(User user)
    {
        _context.Update(user);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
