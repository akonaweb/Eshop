using FluentValidation;
using FluentValidation.Results;

namespace Eshop.WebApi.Exceptions
{
    public class BusinessValidationException(string message, IEnumerable<ValidationFailure>? errors = null)
        : ValidationException(message, errors)
    { }
}
