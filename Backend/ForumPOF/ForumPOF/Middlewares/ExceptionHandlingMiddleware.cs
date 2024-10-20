using Application.Helper;
using System.Net;

namespace ForumPOF.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private ILogger<ExceptionHandlingMiddleware> _logger = logger;
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(
                context,
                ex.Message,
                HttpStatusCode.InternalServerError,
                "Что-то пошло не так");
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        string exMsg,
        HttpStatusCode httpStatusCode,
        string message)
    {
        _logger.LogError(exMsg);

        HttpResponse response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        var error = Result<string>.Fail(response.StatusCode, message);

        await response.WriteAsJsonAsync(new
        {
            error.StatusCode,
            error.Error
        });
    }
}

