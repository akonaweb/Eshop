using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Eshop.WebApi.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Eshop.WebApi.Filters
{
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionFilter : IExceptionFilter, IFilterMetadata
    {
        private readonly ILogger<GlobalExceptionFilter> logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessValidationException businessValidationException)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Title = "Business Validation Error",
                    Status = StatusCodes.Status409Conflict,
                    Detail = businessValidationException.Message,
                    Errors = GetErrors(businessValidationException) ?? new Dictionary<string, string[]>()
                };

                context.Result = new ObjectResult(validationProblemDetails)
                {
                    StatusCode = StatusCodes.Status409Conflict
                };

                logger.LogError(context.Exception, "Custom validation exception occurred.");
            }
            else if (context.Exception is NotFoundException notFoundException)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Title = "Not Found Error",
                    Status = StatusCodes.Status404NotFound,
                    Detail = notFoundException.Message,
                    Errors = GetErrors(notFoundException) ?? new Dictionary<string, string[]>()
                };

                context.Result = new ObjectResult(validationProblemDetails)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

                logger.LogError(context.Exception, "Not found exception occurred.");
            }
            else if (context.Exception is ValidationException validationException)
            {
                var validationProblemDetails = new ValidationProblemDetails
                {
                    Title = "Validation Error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = validationException.Message,
                    Errors = GetErrors(validationException) ?? new Dictionary<string, string[]>()
                };

                context.Result = new ObjectResult(validationProblemDetails)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };

                logger.LogError(context.Exception, "Validation exception occurred.");
            }
            else if (context.Exception is UnauthorizedException unauthorizedException)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = StatusCodes.Status401Unauthorized,
                    Detail = unauthorizedException.Message
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                logger.LogWarning(context.Exception, "Unauthorized access.");
            }
            else if (context.Exception is ForbiddenException forbiddenException)
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "Forbidden",
                    Status = StatusCodes.Status403Forbidden,
                    Detail = forbiddenException.Message
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

                logger.LogWarning(context.Exception, "Forbidden access.");
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Title = "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = context.Exception.Message
                };

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
                
                logger.LogError(context.Exception, "Unhandled exception occurred.");
            }

            context.ExceptionHandled = true;
        }

        private static Dictionary<string, string[]>? GetErrors(ValidationException validationException)
        {
            return validationException.Errors?
                                .GroupBy(e => e.PropertyName) // Group errors by property name
                                .ToDictionary(
                                    group => group.Key,
                                    group => group.Select(e => e.ErrorMessage).ToArray()
                                );
        }
    }
}
