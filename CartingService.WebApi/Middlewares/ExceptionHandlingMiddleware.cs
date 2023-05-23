using System.Net;
using CartingService.Domain.ExceptionHandling;

namespace CartingService.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex, logger);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex, ILogger<ExceptionHandlingMiddleware> logger)
    {
        logger.LogError($"Exception occurred with message {ex.Message}");

        if (ex is NotFoundException || ex is MessageQueueConectionException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return context.Response.WriteAsync(ex.Message);
        }

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        return context.Response.WriteAsync($"Exception has been thrown during request processing. {ex.Message}");
    }
}
