namespace Application.DTOs.Users;

public class UserUpdateRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
