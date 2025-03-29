using FluentValidation;
using FluentValidation.Results;

namespace Eshop.WebApi.Exceptions
{
    public class NotFoundException(string message, IEnumerable<ValidationFailure>? errors = null)
        : ValidationException(message, errors)
    { }
}
