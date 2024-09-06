using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models;

public class User
{
    public Ulid Id {  get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime Created {  get; set; }
    public DateTime Updated {  get; set; }

    public ICollection<Topic>? Topics { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public ICollection<Comment>? Comments { get; set; }

    public static User Create(Ulid id, string userName, string password, string email, DateTime created)
    {
        return new User()
        {
            Id = id,
            UserName = userName,
            Password = password,
            Email = email,
            Created = created
        };
    }

    public static User Update(User user, string password, string email, DateTime updated)
    {
        return new User
        {
            Id = user.Id,
            UserName = user.UserName,
            Password = password,
            Email = email,
            Created = user.Created,
            Updated = updated
        };
    }
}
