using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class TopicsService(
    IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task Create(string jwt, string Title, Ulid category)
    {
        return Task.CompletedTask;
    }
}
