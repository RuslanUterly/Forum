using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repository.Interfaces;

public interface ITagRepository
{
    Task<bool> TagExistByTitle(string title);
    Task<bool> TagExistById(Ulid id);

    Task<ICollection<Tag>> GetTags();
    Task<ICollection<Topic>> GetTopicsByTag(string title);
    Task<Tag> GetTagByTitle(string title);
    Task<Tag> GetTagById(Ulid id);

    Task<bool> CreateTag(Tag tag);
    Task<bool> UpdateTag(Tag tag);
    Task<bool> DeleteTag(Tag tag);

    Task<bool> Save();
}
