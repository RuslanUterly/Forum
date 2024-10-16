using Application.Helper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

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
                "Smth wrong");
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

        Error error = new Error(response.StatusCode, message);

        string result = JsonSerializer.Serialize(error);

        await response.WriteAsJsonAsync(result);
    }
}

