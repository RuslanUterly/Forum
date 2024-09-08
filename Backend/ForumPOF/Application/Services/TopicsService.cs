using Application.Helper;
using Application.Interfaces.Auth;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class TopicsService(
    ITopicRepository topicRepository,
    IJwtProvider jwtProvider)
{
    private readonly ITopicRepository _topicRepository = topicRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    //public Task Create(string jwt, string Title, string categoryName)
    //{
    //    Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);
    //    Ulid categoryId 
    //    var topic = Topic.Create(Ulid.NewUlid(), Title, )
    //}
}
