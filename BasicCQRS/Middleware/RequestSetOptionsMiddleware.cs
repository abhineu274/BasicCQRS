using System.Net;

namespace BasicCQRS.Middleware
{
    public class RequestSetOptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestSetOptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Test with https://localhost:5001/Privacy/?option=Hello
        public async Task Invoke(HttpContext httpContext)
        {
            var option = httpContext.Request.Query["option"];

            if (!string.IsNullOrWhiteSpace(option))
            {
                httpContext.Items["option"] = WebUtility.HtmlEncode(option);
            }

            await _next(httpContext); //middleware pipeline
        }
    }

    /* This is a middle ware class which we need to add/configure into RequestSetOptionsStartupFilter
     * Here we are setting some value to httpContext which we can use throughout our api request 
     *
     */
}
