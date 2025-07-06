using Eshop.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Eshop.WebApi.Features.Users
{
    public class RegisterUser
    {
        public record Command(RegisterRequest Request) : IRequest<IResult>;

        public class Handler : IRequestHandler<Command, IResult>
        {
            private readonly UserManager<ApplicationUser> userManager;

            public Handler(UserManager<ApplicationUser> userManager)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Request.Email).NotEmpty();
                    RuleFor(x => x.Request.Password).NotEmpty();
                    RuleFor(x => x.Request.Password.Length).GreaterThan(7);
                }
            }

            public async Task<IResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var request = command.Request;

                var user = new ApplicationUser(request.Email);
                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return Results.BadRequest(new { Errors = errors });
                }

                var response = new RegisterUserResponseDto
                {
                    Email = user.Email!,
                    UserId = user.Id
                };

                return Results.Ok(response);
            }
        }
    }

    public class RegisterRequest
    {
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required, MinLength(8)]
        public required string Password { get; set; }
    }

    public class RegisterUserResponseDto
    {
        public required string Email { get; set; }
        public required string UserId { get; set; }
    }
}
