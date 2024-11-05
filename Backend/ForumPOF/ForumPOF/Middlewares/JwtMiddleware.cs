using Application.Interfaces.Auth;
using System.Security.Claims;

namespace ForumPOF.Middlewares;

public class JwtMiddleware(
    RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context, IJwtProvider jwtProvider)
    {
        //if (context.Request.Cookies.TryGetValue("tasty-cookies", out string jwt))
        //{
        //    var user = jwtProvider.VerifyToken(jwt);

        //    if (user != null) 
        //        context.User = new ClaimsPrincipal(new ClaimsIdentity(user.Claims, "jwt"));
        //}

        await _next(context);
    }
}
