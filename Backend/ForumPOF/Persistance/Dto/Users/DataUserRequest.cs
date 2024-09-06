using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Dto.Users;

public class DataUserRequest
{
    public Ulid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}