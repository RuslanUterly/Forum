using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repository.Interfaces;

public interface ITopicRepository
{
    Task<ICollection<Topic>> GetTopics();
    Task<ICollection<Topic>> GetTopicsByTitle(string title);
    Task<ICollection<Topic>> GetTopicsByUser(Ulid userId);
    Task<Topic> GetTopicsById(Ulid id);

    Task<bool> CreateTopic(Tag[] tags, Topic topic);
    Task<bool> UpdateTopic(Tag[] tags, Topic topic);
    Task<bool> DeleteTopic(Topic topic);

    Task<bool> Save();
}
