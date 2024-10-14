using Persistance.Models;

namespace Persistance.Repository.Interfaces;

public interface ITopicRepository
{
    Task<bool> TopicExistById(Ulid id);

    Task<IEnumerable<Topic>> GetTopics();
    Task<IEnumerable<Topic>> GetTopicsByTitle(string title);
    Task<IEnumerable<Topic>> GetTopicsByUser(Ulid userId);
    Task<Topic> GetTopicsById(Ulid id);

    Task<bool> CreateTopic(Tag[] tags, Topic topic);
    Task<bool> UpdateTopic(Tag[] tags, Topic topic);
    Task<bool> DeleteTopic(Topic topic);

    Task<bool> Save();
}
