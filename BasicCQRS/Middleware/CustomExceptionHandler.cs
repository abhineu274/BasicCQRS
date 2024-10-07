using BasicCQRS.Miscellaneous;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace BasicCQRS.Middleware
{
    public class CustomExceptionHandler : IExceptionHandler // Implement the IExceptionHandler interface
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;
            var result = JsonSerializer.Serialize(new { error = exception.Message });
            await context.Response.WriteAsync(result);
            return true;
        }
    }
}

