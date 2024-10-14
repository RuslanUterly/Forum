using Persistance.Models;

namespace Persistance.Repository.Interfaces;

public interface ITagRepository
{
    Task<bool> TagExistByTitle(string title);
    Task<bool> TagExistById(Ulid id);

    Task<IEnumerable<Tag>> GetTags();
    Task<IEnumerable<Topic>> GetTopicsByTag(string title);
    Task<Tag> GetTagByTitle(string title);
    Task<Tag> GetTagById(Ulid id);

    Task<bool> CreateTag(Tag tag);
    Task<bool> UpdateTag(Tag tag);
    Task<bool> DeleteTag(Tag tag);

    Task<bool> Save();
}
