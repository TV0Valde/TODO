using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TODO.Application.Common.Exceptions;

namespace TODO.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = string.Empty;
            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    error = exception.Message;
                    break;
                case DuplecateTitleException:
                    code = HttpStatusCode.Conflict;
                    error = exception.Message;
                    break;
                default:
                    error = "An unexpected error occurred.";
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                error = new
                {
                    message = exception.Message,
                    statusCode = (int)code
                }
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}