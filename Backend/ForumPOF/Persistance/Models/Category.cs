using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models;

public class Category
{
    public Ulid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }

    public ICollection<Topic>? Topics { get; set; }

    public static Category Create(Ulid id, string name, DateTime created)
    {
        return new Category()
        {
            Id = id,
            Name = name,
            Created = created
        };
    }

    public static Category Update(Category category, string name)
    {
        category.Name = name;
        return category;
    }
}
