using Eshop.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

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
                }
            }

            public async Task<IResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var request = command.Request;

                var user = new ApplicationUser(request.Email);
                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return Results.BadRequest(result.Errors);

                var response = new RegisterUserResponseDto
                {
                    Email = user.Email!,
                    UserId = user.Id
                };

                return Results.Ok(response);
            }
        }
    }

    public class RegisterUserResponseDto
    {
        public required string Email { get; set; }
        public required string UserId { get; set; }
    }
}