using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Dto.Categories;

public class CategoryUpdateRequest
{
    public Ulid Id { get; set; }
    public string Name { get; set; }
}
