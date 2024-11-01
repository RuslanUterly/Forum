using ForumPOF.Attributes;
using ForumPOF.Extensions;

namespace ForumPOF.Middlewares;

public class LoggerMiddleware(
    RequestDelegate next,
    ILogger<LoggerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata?.GetMetadata<SkipLogging>() != null)
        {
            await next(context);
            return;
        }

        var originalResponseBody = context.Response.Body;

        // Включаем буферизацию тела запроса, чтобы можно было читать его несколько раз
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var requestPath = context.Request.Path.Value;
        var requestQuery = context.Request.QueryString;

        // Сбрасываем позицию на начало тела запроса, чтобы другие компоненты могли его прочитать
        context.Request.Body.Position = 0;

        using (var memory = new MemoryStream())
        {
            context.Response.Body = memory;
            await next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            logger.DynamicLog($"\tRequest.Path: {requestPath}\n" +
                $"\tRequest.Body: {requestBody}\n" +
                $"\tRequest.Query: {requestQuery}\n" +
                $"\tResponse.StatusCode: {context.Response.StatusCode}\n" +
                $"\tResponse.Body: {responseBody}\n", context.Response.StatusCode);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            await memory.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
        }
    }
}
