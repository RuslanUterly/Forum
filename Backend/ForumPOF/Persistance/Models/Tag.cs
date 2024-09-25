using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models;

public class Tag
{
    public Ulid Id { get; set; }
    public string Title { get; set; }

    public virtual ICollection<TopicTag> TopicTags { get; set; }

    public static Tag Create(
        Ulid id,
        string title)
    {
        return new Tag()
        {
            Id = id,
            Title = title
        };
    }

    public static Tag Update(
        Tag tag,
        string title)
    {
        tag.Title = title;

        return tag;
    }
}