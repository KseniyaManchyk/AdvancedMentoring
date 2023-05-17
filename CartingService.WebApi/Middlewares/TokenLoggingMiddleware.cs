using System.IdentityModel.Tokens.Jwt;

namespace CartingService.WebApi.Middlewares
{
    public class TokenLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public TokenLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("Token");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request
                               .Headers["Authorization"]
                               .ToString()
                               .Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            _logger.LogInformation($"User encoded token is {token}");
            _logger.LogInformation($"User decoded token is {decodedToken}");

            await _next.Invoke(context);
        }
    }
}
