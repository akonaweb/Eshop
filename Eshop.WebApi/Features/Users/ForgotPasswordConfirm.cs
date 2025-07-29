using Eshop.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Eshop.WebApi.Features.Users
{
    public class ForgotPasswordConfirm
    {
        public record Command(ForgotPasswordConfirmRequestDto Request) : IRequest<IResult>;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Request.Email).NotEmpty();
                RuleFor(x => x.Request.ResetCode).NotEmpty();
                RuleFor(x => x.Request.NewPassword).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, IResult>
        {
            private readonly UserManager<ApplicationUser> userManager;

            public Handler(UserManager<ApplicationUser> userManager)
            {
                this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            }

            public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var model = request.Request;
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return Results.BadRequest("Invalid request.");

                var result = await userManager.ResetPasswordAsync(user, model.ResetCode, model.NewPassword);
                if (!result.Succeeded)
                    return Results.BadRequest(result.Errors);

                return Results.Ok("Password has been reset.");
            }
        }
    }

    public sealed class ForgotPasswordConfirmRequestDto
    {
        public required string Email { get; set; }
        public required string ResetCode { get; set; }
        public required string NewPassword { get; set; }
    }
}