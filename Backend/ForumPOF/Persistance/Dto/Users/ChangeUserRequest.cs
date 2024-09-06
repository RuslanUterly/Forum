namespace Persistance.Dto.Users;

public class ChangeUserRequest
{
    public Ulid Id { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
