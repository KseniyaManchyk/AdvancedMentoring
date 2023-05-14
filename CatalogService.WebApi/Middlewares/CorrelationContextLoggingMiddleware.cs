using CorrelationId.Abstractions;

namespace CatalogService.WebApi.Middlewares;

public class CorrelationContextLoggingMiddleware
{
    readonly RequestDelegate _next;

    public CorrelationContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext httpContext, ILogger<CorrelationContextLoggingMiddleware> logger, ICorrelationContextAccessor correlationContextAccessor)
    {
        if (Guid.TryParse(correlationContextAccessor.CorrelationContext.CorrelationId, out Guid correlationId))
        {
            using (logger.BeginScope(new Dictionary<string, Guid> { { "CorrelationId", correlationId } }))
            {
                await _next(httpContext);
            }
        }
        else
        {
            await _next(httpContext);
        }
    }
}
