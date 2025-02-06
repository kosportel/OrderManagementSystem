using Serilog;
using Serilog.Context;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace OrderManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Extract User ID & Email from JWT if available
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userEmail = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                // Enrich logs with User context
                using (LogContext.PushProperty("UserId", userId ?? "Anonymous"))
                using (LogContext.PushProperty("UserEmail", userEmail ?? "Anonymous"))
                {
                    await _next(httpContext); // Proceed with the request pipeline
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                message = "An unexpected error occurred.",
                details = exception.Message // You can disable this in production for security
            };

            var errorJson = JsonSerializer.Serialize(errorResponse);
            return response.WriteAsync(errorJson);
        }
    }
}
