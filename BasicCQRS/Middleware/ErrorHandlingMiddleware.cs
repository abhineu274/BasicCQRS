using BasicCQRS.Miscellaneous;
using System;
using System.Net;
using System.Text.Json;

namespace BasicCQRS.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next; // RequestDelegate is a delegate that represents the next middleware in the pipeline
        private readonly ILogger<ErrorHandlingMiddleware> _logger; // ILogger is a generic interface for logging.

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger; //Inject the logger based on the DI from Program.cs. By default, the logger is configured to log to the console.
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            // Set status code based on the exception type
            switch (ex)
            {
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = ex.Message,
                type = ex.GetType().Name,
                stackTrace = ex.StackTrace // Optional: include only in development
            });

            return context.Response.WriteAsync(result);
        }

    }

}
