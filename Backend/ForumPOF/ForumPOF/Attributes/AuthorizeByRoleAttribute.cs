using Microsoft.AspNetCore.Authorization;
using Persistance.Enums;

namespace ForumPOF.Attributes;

public class AuthorizeByRoleAttribute : AuthorizeAttribute
{
    public AuthorizeByRoleAttribute()
    {
    }

    public AuthorizeByRoleAttribute(params UserRole[] roles)
    {
        Roles = string.Join(", ", roles);
    }
}
