using System.Net;
using FluentValidation;
using JobApplicationTracker.API.Exceptions;

namespace JobApplicationTracker.API.Middlewares
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
                _logger.LogError(ex, "An unhandled exception occurred during request processing.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";
            object errors = null;

            switch (exception)
            {
                //Handles fluent validation
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "One or more validation errors occurred.";
                    errors = validationException.Errors
                        .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage })
                        .ToList();
                    break;

                // Handles ArgumentException and its derived classes (like ArgumentNullException)
                case ArgumentException _:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case CustomValidationException customValidationException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    message = customValidationException.Message;
                    break;

                // Handles resource not found errors
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    message = notFoundException.Message;
                    break;

                default:
                    // Leaves the status as 500 Internal Server Error
                    break;
            }

            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsJsonAsync(new
            {
                status = statusCode,
                error = message,
                details = errors
            });
        }
    }
}
